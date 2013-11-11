using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace River
{
    partial class RoomEditor : Form
    {
        private Level LevelPTR;
        private Point CurrentTilePos;

        public RoomEditor(Level LevelPTR)
        {
            this.LevelPTR = LevelPTR;
            this.Show();
            InitializeComponent();
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            LevelPTR.LevelMap.Resize((int)RoomWidth.Value, (int)RoomHeight.Value, -1);


            MapCell NewTile = new MapCell((int)BaseTile.Value);

            //Set state
            NewTile.Walkable = WalkableCB.Checked;
            NewTile.Flipped = FlipCB.Checked;
            NewTile.IgnoreHeight = NoHeightCB.Checked;

            //Set event type
            if (TileEventCBB.SelectedIndex != -1)
                NewTile.EventType = (EventType)TileEventCBB.SelectedIndex;

            //Add height tiles
            for (int ecx = 0; ecx < HeightTilesListBox.Items.Count; ecx++)
                NewTile.AddHeightTile(Convert.ToInt32(HeightTilesListBox.Items[ecx].ToString()));

            //Add surface tiles
            for (int ecx = 0; ecx < SurfaceTilesListBox.Items.Count; ecx++)
                NewTile.AddSurfaceTile(Convert.ToInt32(SurfaceTilesListBox.Items[ecx].ToString()));


            //Apply
            LevelPTR.LevelMap.ChangeTileData(NewTile, CurrentTilePos);


        }

        public void LoadTileData(MapCell Tile, Microsoft.Xna.Framework.Point Position)
        {
            //Bounds check
            if (Position.X >= 0 && Position.X < LevelPTR.LevelMap.MapWidth &&
    (Position.Y >= 0 && Position.Y < LevelPTR.LevelMap.MapHeight))
            {
                //Update info
                XPosition.Text = Position.Y.ToString();
                YPosition.Text = Position.X.ToString();

                CurrentTilePos = Position;

                //Grab ID:
                //Base
                BaseTile.Value = Tile.TileID;
                //Height
                HeightTilesListBox.Items.Clear();
                for (int ecx = 0; ecx < Tile.HeightTiles.Count; ecx++)
                    HeightTilesListBox.Items.Add(Tile.HeightTiles[ecx].ToString());
                //Surface
                SurfaceTilesListBox.Items.Clear();
                for (int ecx = 0; ecx < Tile.SurfaceTiles.Count; ecx++)
                    SurfaceTilesListBox.Items.Add(Tile.SurfaceTiles[ecx].ToString());

                //Get state
                this.WalkableCB.Checked = Tile.Walkable;
                this.FlipCB.Checked = Tile.Flipped;
                this.NoHeightCB.Checked = Tile.IgnoreHeight;

                TileEventCBB.SelectedIndex = (int)Tile.EventType;

            }
        }

        //Removes a height tile
        private void DelHeightButton_Click(object sender, EventArgs e)
        {
            if (HeightTilesListBox.SelectedIndex >= 0 &&
                HeightTilesListBox.SelectedIndex < HeightTilesListBox.Items.Count)
            {
                HeightTilesListBox.Items.RemoveAt(HeightTilesListBox.SelectedIndex);
            }
        }

        //Removes a surface tile
        private void DelSurfaceButton_Click(object sender, EventArgs e)
        {
            if (SurfaceTilesListBox.SelectedIndex >= 0 &&
                SurfaceTilesListBox.SelectedIndex < SurfaceTilesListBox.Items.Count)
            {
                SurfaceTilesListBox.Items.RemoveAt(SurfaceTilesListBox.SelectedIndex);
            }
        }

        //Adds a new height tile
        private void AddHeightButton_Click(object sender, EventArgs e)
        {
            int AddValue = (int)HeightTile.Value;
            HeightTilesListBox.Items.Add(AddValue.ToString());
        }

        //Adds a new surface tile
        private void AddSurfaceButton_Click(object sender, EventArgs e)
        {
            int AddValue = (int)SurfaceTile.Value;
            SurfaceTilesListBox.Items.Add(AddValue.ToString());
        }


        /////////////////
        //SAVING/LOADING
        /////////////////

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (SaveRoomItems.SelectedIndex < 0 ||
                SaveRoomItems.SelectedIndex > SaveRoomItems.Items.Count)
            {
                MessageBox.Show(this, "Improper selection", "You're awful", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] SavedFilePaths = GetFiles();

            //Save format follows Room_{SETID}_{SAVEID}
            //Look for first matching set ID (if any)
            int StartIndex = SavedFilePaths[SaveRoomItems.SelectedIndex].LastIndexOf("Room") + 4;
            int EndIndex = SavedFilePaths[SaveRoomItems.SelectedIndex].LastIndexOf("_");

            //Get the set ID and compare
            int SetID = Convert.ToInt32(SavedFilePaths[SaveRoomItems.SelectedIndex].Substring(StartIndex, EndIndex - StartIndex));

            //Set IDs matched, now compare save IDs
            StartIndex = EndIndex + 1;
            EndIndex = SavedFilePaths[SaveRoomItems.SelectedIndex].LastIndexOf(".");

            int SaveID = Convert.ToInt32(SavedFilePaths[SaveRoomItems.SelectedIndex].Substring(StartIndex, EndIndex - StartIndex));

            Save(SetID, SaveID);
        }

        private void SaveNewButton_Click(object sender, EventArgs e)
        {
            string[] SavedFilePaths = GetFiles();

            int SetID = (int)RoomSet.Value;
            int SaveID = 0; //Default 0

            for (int ecx = 0; ecx < SavedFilePaths.Length; ecx++)
            {
                //Save format follows Room_{SETID}_{SAVEID}
                //Look for first matching set ID (if any)
                int StartIndex = SavedFilePaths[ecx].LastIndexOf("Room") + 4;
                int EndIndex = SavedFilePaths[ecx].LastIndexOf("_");

                //Get the set ID and compare
                string FoundSetID = SavedFilePaths[ecx].Substring(StartIndex, EndIndex - StartIndex);
                if (FoundSetID == SetID.ToString())
                {
                    //Set IDs matched, now compare save IDs
                    StartIndex = EndIndex + 1;
                    EndIndex = SavedFilePaths[ecx].LastIndexOf(".");

                    string FoundSaveID = SavedFilePaths[ecx].Substring(StartIndex, EndIndex - StartIndex);

                    if (FoundSaveID == SaveID.ToString())
                        SaveID++;
                }
            }

            Save(SetID, SaveID);
        }

        private string[] GetFiles()
        {
            string[] FilePaths = Directory.GetFiles("Content/Rooms/", "*.RRM");

            for (int ecx = 0; ecx < FilePaths.Length; ecx++)
                FilePaths[ecx] = FilePaths[ecx].Replace("Content/Rooms/", "");

            return FilePaths;
        }

        private void Save(int SetID, int SaveID)
        {
            string FileName = "Room" + SetID + "_" + SaveID.ToString() + ".RRM";

            //Open stream to the file
            Stream SaveStream = File.Open("Content/Rooms/" + FileName, FileMode.Create);

            //Formatting is in binary (smaller files this way)
            BinaryFormatter BFormatter = new BinaryFormatter();

            //Save our data (Yay for C# class serialization! I don't have to do any real work now)
            //Save size of room
            Point RoomSize = new Point(LevelPTR.LevelMap.MapWidth, LevelPTR.LevelMap.MapHeight);
            BFormatter.Serialize(SaveStream, RoomSize);

            //Save each cell
            for (int Y = 0; Y < RoomSize.Y; Y++)
                for (int X = 0; X < RoomSize.X; X++)
                    BFormatter.Serialize(SaveStream, LevelPTR.LevelMap.MapCells[Y, X]);

            //Close our file stream
            SaveStream.Close();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            if (LoadRoomItems.SelectedIndex < 0 ||
                LoadRoomItems.SelectedIndex > LoadRoomItems.Items.Count)
            {
                MessageBox.Show(this, "Improper selection", "You're awful", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string FileName = LoadRoomItems.Items[LoadRoomItems.SelectedIndex].ToString();

            try
            {
                Stream OpenStream = File.Open("Content/Rooms/" + FileName, FileMode.Open);

                using (OpenStream)
                {

                    //Open binary formatting reader
                    BinaryFormatter BFormatter = new BinaryFormatter();

                    //Load room size
                    Point RoomSize = (Point)BFormatter.Deserialize(OpenStream);
                    LevelPTR.LevelMap.Resize(RoomSize.X, RoomSize.Y, 0);
                    RoomWidth.Value = RoomSize.X;
                    RoomHeight.Value = RoomSize.Y;

                    //Load all tiles/data
                    for (int Y = 0; Y < RoomSize.Y; Y++)
                        for (int X = 0; X < RoomSize.X; X++)
                            LevelPTR.LevelMap.MapCells[Y, X] = (MapCell)BFormatter.Deserialize(OpenStream);

                    OpenStream.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            }


        }


        private void RoomInfoButton_Click(object sender, EventArgs e)
        {
            int PlayerSpawns = 0;
            int Exits = 0;
            int Waypoints = 0;
            int Chests = 0;
            int WeaponRacks = 0;

            string Issues = "";

            for (int Y = 0; Y < LevelPTR.LevelMap.MapHeight; Y++)
                for (int X = 0; X < LevelPTR.LevelMap.MapWidth; X++)
                {
                    switch (LevelPTR.LevelMap.MapCells[Y, X].EventType)
                    {
                        case EventType.None:
                            break;
                        case EventType.PlayerSpawn:
                            PlayerSpawns++;
                            if (PlayerSpawns > 1)
                                Issues += "MULTIPLE PLAYER SPAWNS ";
                            break;
                        case EventType.Exit:
                            Exits++;
                            if (Exits > 1)
                                Issues += "MULTIPLE EXITS ";
                            break;
                        case EventType.Waypoint:
                            Waypoints++;
                            if (Waypoints > 1)
                                Issues += "MULTIPLE WAYPOINTS ";
                            break;
                        case EventType.Chest:
                            Chests++;
                            break;
                        case EventType.Weaponrack:
                            WeaponRacks++;
                            break;
                    }

                }

            if (PlayerSpawns == 0)
                Issues += "NO PLAYER SPAWNS ";

            if (Exits == 0)
                Issues += "NO EXITS ";

            if (Waypoints == 0)
                Issues += "NO WAYPOINTS ";

            MessageBox.Show("Player Spawns: " + PlayerSpawns.ToString() +
                            "\nWayPoints: " + Waypoints.ToString() +
                            "\nExits: " + Exits.ToString() +
                            "\nChests: " + Chests.ToString() +
                            "\nWeaponRacks: " + WeaponRacks.ToString(), Issues);

        }

        private void RoomEditor_Load(object sender, EventArgs e)
        {

        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadRoomItems.Items.Clear();
            SaveRoomItems.Items.Clear();

            LoadRoomItems.Items.AddRange(GetFiles());
            SaveRoomItems.Items.AddRange(GetFiles()); ;
        }

        private void FlipCB_CheckedChanged(object sender, EventArgs e)
        {
            //unused
        }

        private void NoHeightCB_CheckedChanged(object sender, EventArgs e)
        {
            //unused
        }

        private void WalkableCB_CheckedChanged(object sender, EventArgs e)
        {
            //unused
        }


    }
}
