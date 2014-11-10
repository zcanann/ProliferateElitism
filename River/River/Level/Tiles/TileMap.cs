using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace River
{
    //Used to save all information about a room loaded in from resources
    struct Room
    {
        private Point Size;
        public MapCell[,] Data;
        public Vector2 PlayerSpawn;
        public Vector2 Exit;
        public Vector2 WayPointPosition;
        public Vector2[] ChestPositions;
        public Vector2[] WeaponRackPositions;

        public Room(MapCell[,] Data, Point Size, Vector2 PlayerSpawn, Vector2 Exit, Vector2 WayPointPosition,
            Vector2[] ChestPositions, Vector2[] WeaponRackPositions)
        {
            this.Data = Data;
            this.Size = Size;
            this.PlayerSpawn = PlayerSpawn;
            this.Exit = Exit;
            this.WayPointPosition = WayPointPosition;
            this.ChestPositions = ChestPositions;
            this.WeaponRackPositions = WeaponRackPositions;
        }

        public int Width()
        {
            return Size.X;
        }

        public int Height()
        {
            return Size.Y;
        }
    }

    class TileMap
    {
        public MapCell[,] MapCells;
        //private List<Room> Rooms = new List<Room>();
        private Room[][] Rooms;
        public int RoomSetID;
        public int RoomID;

        private Random Random = new Random();

        public int MapWidth;
        public int MapHeight;

        public int[][] ExpPool; //Exp pool for each room

        private Texture2D MouseMap;
        private Texture2D SlopeMaps;

        public Vector2[] EnemySpawn;
        public Vector2 PlayerSpawn;
        public Vector2[] ChestSpawns;
        public Vector2[] WeaponRackSpawns;
        public Vector2 WayPoint;
        public Vector2 Exit;

        public Vector2 ShopSpawn;
        public Vector2 StorageSpawn;
        public Vector2 EnchanterSpawn;

        public enum CardinalDirection
        {
            None = 0,
            North = 1,
            West = 2,
            East = 3,
            South = 4,
        }

        private enum Connector
        {
            Single,
            Triple,
            Full,
        }

        public TileMap()
        {
            RoomSetID = 0;
            RoomID = -1;
        }

        public void LoadContent(ContentManager Content)
        {
            this.MouseMap = Content.Load<Texture2D>(@"Textures\Tiles\MouseMap");
            this.SlopeMaps = Content.Load<Texture2D>(@"Textures\Tiles\SlopeMap");

            string[] RoomFilePaths = Directory.GetFiles(Content.RootDirectory + "/Rooms/", "*.RRM");

            List<int> SetRoomCount = new List<int>(); //Used to assist in creation of jaggad room array
            int StartIndex;
            int EndIndex;
            int SetID = 0;
            int SubCount = 0;

            //Get a count of the number of rooms for each tile set
            for (int ecx = 0; ecx < RoomFilePaths.Length; ecx++)
            {
                //Get set ID
                StartIndex = RoomFilePaths[ecx].LastIndexOf("Room") + 4;
                EndIndex = RoomFilePaths[ecx].LastIndexOf("_");
                int NextID = Convert.ToInt32(RoomFilePaths[ecx].Substring(StartIndex, EndIndex - StartIndex));

                if (SetID == NextID)
                    SubCount++;

                //If not equal or at end, add it
                else
                {
                    SetRoomCount.Add(SubCount);
                    SubCount = 1;
                    SetID = NextID;
                }
            }
            //Add the last one
            SetRoomCount.Add(SubCount);
            SubCount = 0;
            SetID = 0;

            //Set up array of rooms
            Rooms = new Room[SetRoomCount.Count][];
            for (int ecx = 0; ecx < SetRoomCount.Count; ecx++)
                Rooms[ecx] = new Room[SetRoomCount[ecx]];

            try
            {
                for (int ecx = 0; ecx < RoomFilePaths.Length; ecx++)
                {
                    Stream OpenStream = File.Open(RoomFilePaths[ecx], FileMode.Open);

                    using (OpenStream)
                    {
                        //Remove all except name
                        string WorkingRoomPath = RoomFilePaths[ecx].Replace(Content.RootDirectory + "/Rooms/", "");

                        //Get set ID
                        StartIndex = RoomFilePaths[ecx].LastIndexOf("Room") + 4;
                        EndIndex = RoomFilePaths[ecx].LastIndexOf("_");
                        int NextID = Convert.ToInt32(RoomFilePaths[ecx].Substring(StartIndex, EndIndex - StartIndex));

                        if (SetID == NextID)
                            SubCount++;
                        else
                        {
                            SubCount = 1;
                            SetID = NextID;
                        }

                        //Open binary formatting reader
                        BinaryFormatter BFormatter = new BinaryFormatter();

                        //Load room size
                        Point RoomSize = (Point)BFormatter.Deserialize(OpenStream);
                        List<Vector2> RoomChestSpawns = new List<Vector2>();
                        List<Vector2> RoomWeaponRackSpawns = new List<Vector2>();
                        MapCell[,] LoadedData = new MapCell[RoomSize.Y, RoomSize.X];

                        //Load all tiles/data. Temporarily uses global variables and adds them to the room.
                        for (int Y = 0; Y < RoomSize.Y; Y++)
                            for (int X = 0; X < RoomSize.X; X++)
                            {
                                LoadedData[Y, X] = (MapCell)BFormatter.Deserialize(OpenStream);

                                switch (LoadedData[Y, X].EventType)
                                {
                                    case EventType.None:
                                        break;
                                    case EventType.PlayerSpawn:
                                        PlayerSpawn.X = X;
                                        PlayerSpawn.Y = Y;
                                        AdjustTileCoordsToReal(ref PlayerSpawn);
                                        break;
                                    case EventType.Exit:
                                        Exit.X = X;
                                        Exit.Y = Y;
                                        AdjustTileCoordsToReal(ref Exit);
                                        break;
                                    case EventType.Waypoint:
                                        WayPoint.X = X;
                                        WayPoint.Y = Y;
                                        AdjustTileCoordsToReal(ref WayPoint);
                                        break;
                                    case EventType.Chest:
                                        Vector2 ChestPos = new Vector2(X, Y);
                                        AdjustTileCoordsToReal(ref ChestPos);
                                        RoomChestSpawns.Add(ChestPos);
                                        break;
                                    case EventType.Weaponrack:
                                        Vector2 WeaponRackPos = new Vector2(X, Y);
                                        AdjustTileCoordsToReal(ref WeaponRackPos);
                                        RoomWeaponRackSpawns.Add(WeaponRackPos);
                                        break;
                                    case EventType.Shop:
                                        ShopSpawn.X = X;
                                        ShopSpawn.Y = Y;
                                        AdjustTileCoordsToReal(ref ShopSpawn);
                                        break;
                                    case EventType.Storage:
                                        StorageSpawn.X = X;
                                        StorageSpawn.Y = Y;
                                        AdjustTileCoordsToReal(ref StorageSpawn);
                                        break;
                                    case EventType.Enchanter:
                                        EnchanterSpawn.X = X;
                                        EnchanterSpawn.Y = Y;
                                        AdjustTileCoordsToReal(ref EnchanterSpawn);
                                        break;

                                }
                            }

                        //Add new room
                        Rooms[SetID][SubCount - 1] = new Room(LoadedData, RoomSize, PlayerSpawn,
                            Exit, WayPoint, RoomChestSpawns.ToArray(), RoomWeaponRackSpawns.ToArray());

                        OpenStream.Close();

                    } //End using openstream

                } //End for loop

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            SetUpExpShare();

        }

        #region Exp pool calculations for each level

        private const float Anchor = 1f / 10f;
        private static float EtoA = 0f;

        private static float PercentShare(int TrueLevel)
        {
            return (float)(Anchor * EtoA / Math.Pow(EtoA, TrueLevel)) * 1f;
        }

        private void SetUpExpShare()
        {
            EtoA = (float)Math.Pow(Math.E, Anchor);

            //Initiate jagged arrays
            float[][] LevelsPerLevel = new float[Rooms.Length][];
            ExpPool = new int[Rooms.Length][];

            for (int ecx = 0; ecx < LevelsPerLevel.Length; ecx++)
            {
                LevelsPerLevel[ecx] = new float[Rooms[ecx].Length];
                ExpPool[ecx] = new int[Rooms[ecx].Length];
            }

            float Next = 0f;

            //Set share of levels for each level
            for (int ecx = 0; ecx < LevelsPerLevel.Length; ecx++)
            {
                for (int edx = 0; edx < LevelsPerLevel[ecx].Length; edx++)
                {
                    Next = PercentShare(ecx + edx);
                    LevelsPerLevel[ecx][edx] = Next * Player.LevelCap;
                }
            }

            //Walk through leveling a pseudo character to determine how large the exp
            //pool would need to be for a player to catch up

            for (int ecx = 0; ecx < ExpPool.Length; ecx++)
            {
                for (int edx = 0; edx < ExpPool[ecx].Length; edx++)
                {
                    ExpPool[ecx][edx] = (int)((float)Player.ExpPerLevel * LevelsPerLevel[ecx][edx]);

                }
            }

        }

        #endregion

        public void LoadNextMap()
        {

            RoomID++;

            if (RoomID >= Rooms[RoomSetID].Length)
            {
                RoomSetID++;
                RoomID = 0;
            }

            if (RoomSetID >= Rooms.Length)
                throw new Exception("No more rooms");

            Resize(Rooms[RoomSetID][RoomID].Width(), Rooms[RoomSetID][RoomID].Height(), -1);

            //TODO: May need to do .CopyTo if we start altering data
            MapCells = Rooms[RoomSetID][RoomID].Data;

            PlayerSpawn = Rooms[RoomSetID][RoomID].PlayerSpawn;
            Exit = Rooms[RoomSetID][RoomID].Exit;
            WayPoint = Rooms[RoomSetID][RoomID].WayPointPosition;
            ChestSpawns = Rooms[RoomSetID][RoomID].ChestPositions;
            WeaponRackSpawns = Rooms[RoomSetID][RoomID].WeaponRackPositions;

            if ((RoomSetID == 0 && RoomID == 0))
                EnemySpawn = new Vector2[0];
            else
                PlaceEnemies(0.035f);
        }

        public bool IsInTown()
        {
            if (RoomSetID == 0 && RoomID == 0)
                return true;

            return false;
        }

        public void Resize(int NewWidth, int NewHeight, int FillTile)
        {
            //Save old data
            MapCell[,] OldData = MapCells;

            //Clear all data & add empty
            MapCells = new MapCell[NewHeight, NewWidth];

            for (int Y = 0; Y < NewHeight; Y++)
            {
                for (int X = 0; X < NewWidth; X++)
                    MapCells[Y, X] = new MapCell(FillTile);
            }

            int DataHeight = 0;
            int DataWidth = 0;

            if (OldData != null)
                DataHeight = OldData.GetLength(0);
            if (DataHeight > 0)
                DataWidth = OldData.GetLength(1);

            if (NewHeight < DataHeight)
                DataHeight = NewHeight;
            if (NewWidth < DataWidth)
                DataWidth = NewWidth;

            //Copy over any old data
            for (int Y = 0; Y < DataHeight; Y++)
                for (int X = 0; X < DataWidth; X++)
                    MapCells[Y, X] = OldData[Y, X];

            MapWidth = NewWidth;
            MapHeight = NewHeight;

            Camera.WorldWidth = ((MapWidth + 1) * Tile.TileStepX);
            Camera.WorldHeight = ((MapHeight + 5) * Tile.TileStepY); //??absurd

        }

        public void ChangeTileData(MapCell NewTile, Point Location)
        {
            if (Location.X >= 0 && Location.X < MapWidth &&
                (Location.Y >= 0 && Location.Y < MapHeight))
                MapCells[Location.Y, Location.X] = NewTile;
        }

        public MapCell GetTileData(int X, int Y)
        {
            if (X >= 0 && X < MapWidth &&
                (Y >= 0 && Y < MapHeight))
            {
                return MapCells[Y, X];
            }

            return null;
        }

        private void AddRoom(Point SourceLocation, int RoomID)
        {
            for (int Y = SourceLocation.Y; Y < Rooms[RoomSetID][RoomID].Height() + SourceLocation.Y; Y++)
                for (int X = SourceLocation.X; X < Rooms[RoomSetID][RoomID].Width() + SourceLocation.X; X++)
                {
                    //Only copy in data for visible tiles
                    if (Rooms[RoomSetID][RoomID].Data[Y - SourceLocation.Y, X - SourceLocation.X].TileID != -1)
                    {
                        //Bounds checking
                        if (X < 0 || X >= MapWidth ||
                        Y < 0 || Y >= MapHeight)
                            continue; //Nope -- no space where we need a tile
                        MapCells[Y, X] = Rooms[RoomSetID][RoomID].Data[Y - SourceLocation.Y, X - SourceLocation.X];
                    }
                }
        }

        private void AdjustTileCoordsToReal(ref Vector2 TileCoords)
        {
            //Set coords based on yolo
            TileCoords.X = TileCoords.X * Tile.TileStepX + Tile.TileStepX;
            TileCoords.Y = TileCoords.Y * Tile.TileStepY;

            //Adjust X for odd rows
            if (TileCoords.Y % 2 != 0)
                TileCoords.X += Tile.OddRowXOffset;
        }

        //Scatter enemies throughout the map
        private void PlaceEnemies(float MobDensity)
        {

            int WalkableTiles = 0;
            for (int Y = 0; Y < MapHeight; Y++)
            {
                for (int X = 0; X < MapWidth; X++)
                {
                    if (MapCells[Y, X].Walkable == true)
                        WalkableTiles++;
                }
            }

            int Count = (int)((float)WalkableTiles * MobDensity);


            EnemySpawn = new Vector2[Count];
            Point GenCoords;

            for (int ecx = 0; ecx < Count; ecx++)
            {
                //Set coords based on yolo
                do
                    GenCoords = new Point(Random.Next(0, MapWidth), Random.Next(0, MapHeight));
                while (MapCells[GenCoords.Y, GenCoords.X].TileID == -1 ||
                    MapCells[GenCoords.Y, GenCoords.X].Walkable == false);

                EnemySpawn[ecx].X = GenCoords.X * Tile.TileStepX + Tile.TileStepX / 2;
                EnemySpawn[ecx].Y = GenCoords.Y * Tile.TileStepY + Tile.HeightTileOffset / 2;

                //Adjust X for odd rows
                if (GenCoords.Y % 2 != 0)
                    EnemySpawn[ecx].X += Tile.OddRowXOffset;
            }

        }


        //Returns inverse of a cardinal direction
        private CardinalDirection InverseDirection(CardinalDirection Direction)
        {
            switch (Direction)
            {
                case CardinalDirection.North:
                    return CardinalDirection.South;
                case CardinalDirection.South:
                    return CardinalDirection.North;
                case CardinalDirection.West:
                    return CardinalDirection.East;
                case CardinalDirection.East:
                    return CardinalDirection.West;
                default:
                    return CardinalDirection.None;
            }
        }

        private int RandomEven(int Min, int Max)
        {
            int Val;
            do
            {
                Val = Random.Next(Min, Max);
            } while (Val % 2 == 1);

            return Val;
        }

        private int RandomOdd(int Min, int Max)
        {
            int Val;
            do
            {
                Val = Random.Next(Min, Max);
            } while (Val % 2 == 0);

            return Val;
        }

        //hrmmm
        private int GetVariationTile(int TileID)
        {
            // if (Random.Next(0, 100) < 20)
            // {
            //     return TileID + Tile.FloorTiles.Length / 2;
            // }
            return 0;
        }



        #region Public methods

        public bool IsValidSquare(Point CellPoint)
        {
            if (CellPoint.X >= 0 && CellPoint.X < MapWidth &&
                CellPoint.Y >= 0 && CellPoint.Y < MapHeight)
                return true;

            return false;
        }

        public int GetOverallHeight(Point WorldPoint)
        {
            return 0;
            Point MapCellPoint = WorldToMapCell(WorldPoint);

            if (IsValidSquare(MapCellPoint))
            {
                int Height = MapCells[MapCellPoint.Y, MapCellPoint.X].HeightTiles.Count * Tile.HeightTileOffset;
                Height += (int)GetSlopeHeightAtWorldPoint(WorldPoint);

                return Height;
            }

            return 0;
        }

        public int GetOverallHeight(Vector2 WorldPoint)
        {
            return GetOverallHeight(new Point((int)WorldPoint.X, (int)WorldPoint.Y));
        }

        public float GetSlopeHeightAtWorldPoint(Point WorldPoint)
        {
            return 0;
            Point LocalPoint;
            Point MapPoint = WorldToMapCell(WorldPoint, out LocalPoint);

            if (IsValidSquare(MapPoint))
            {
                int SlopeMap = MapCells[MapPoint.Y, MapPoint.X].SlopeMap;

                return GetSlopeMapHeight(LocalPoint, SlopeMap);
            }
            else
                return 0f;
        }

        public float GetSlopeHeightAtWorldPoint(Vector2 WorldPoint)
        {
            return GetSlopeHeightAtWorldPoint(new Point((int)WorldPoint.X, (int)WorldPoint.Y));
        }

        public float GetSlopeMapHeight(Point LocalPixel, int SlopeMap)
        {
            return 0;

            Point TexturePoint = new Point(SlopeMap * MouseMap.Width + LocalPixel.X, LocalPixel.Y);

            Color[] SlopeColor = new Color[1];

            if (new Rectangle(0, 0, SlopeMaps.Width, SlopeMaps.Height).Contains(TexturePoint.X, TexturePoint.Y))
            {
                SlopeMaps.GetData(0, new Rectangle(TexturePoint.X, TexturePoint.Y, 1, 1), SlopeColor, 0, 1);

                float Offset = ((255f - SlopeColor[0].R) / 255f) * Tile.HeightTileOffset;

                return Offset;
            }

            return 0;
        }

        public bool IsWalkableCell(Vector2 WorldPoint)
        {
            return IsWalkableCell(new Point((int)WorldPoint.X, (int)WorldPoint.Y));
        }

        public bool IsWalkableCell(Point WorldPoint)
        {
            if (GetCellAtWorldPoint(WorldPoint) != null)
                return GetCellAtWorldPoint(WorldPoint).Walkable;
            else
                return false;
        }


        public bool IsExitCell(Vector2 WorldPoint)
        {
            return IsExitCell(new Point((int)WorldPoint.X, (int)WorldPoint.Y));
        }

        public bool IsExitCell(Point WorldPoint)
        {
            if (GetCellAtWorldPoint(WorldPoint) != null)
            {
                if (GetCellAtWorldPoint(WorldPoint).EventType == EventType.Exit)
                    return true;
            }
            return false;
        }


        public MapCell GetCellAtWorldPoint(Point WorldPoint)
        {
            Point MapPoint = WorldToMapCell(WorldPoint);
            if (IsValidSquare(MapPoint))
                return MapCells[MapPoint.Y, MapPoint.X];
            else
                return null;
        }

        public MapCell GetCellAtWorldPoint(Vector2 WorldPoint)
        {
            return GetCellAtWorldPoint(new Point((int)WorldPoint.X, (int)WorldPoint.Y));
        }

        public Point WorldToMapCell(Vector2 WorldPoint) // Overload - UNUSED?
        {
            return WorldToMapCell(new Point((int)WorldPoint.X, (int)WorldPoint.Y));
        }

        public Point WorldToMapCell(Point WorldPoint) //Overload
        {
            Point Dummy;
            return WorldToMapCell(WorldPoint, out Dummy);
        }

        //Used to convert a raw point to a specific cell
        public Point WorldToMapCell(Point WorldPoint, out Point LocalPoint)
        {
            Point MapCell = new Point(
               (int)(WorldPoint.X / MouseMap.Width),
               ((int)(WorldPoint.Y / MouseMap.Height)) * 2);

            int LocalPointX = WorldPoint.X % MouseMap.Width;
            int LocalPointY = WorldPoint.Y % MouseMap.Height;

            int DeltaX = 0;
            int DeltaY = 0;

            Color[] TColor = new Color[1];

            if (new Rectangle(0, 0, MouseMap.Width, MouseMap.Height).Contains(LocalPointX, LocalPointY))
            {
                MouseMap.GetData(0, new Rectangle(LocalPointX, LocalPointY, 1, 1), TColor, 0, 1);

                if (TColor[0] == Color.Red) // Red
                {
                    DeltaX = -1;
                    DeltaY = -1;
                    LocalPointX = LocalPointX + (MouseMap.Width / 2);
                    LocalPointY = LocalPointY + (MouseMap.Height / 2);
                }

                else if (TColor[0] == Color.Multiply(Color.Green, 2)) //Color.Green is 128, so mult by 2
                {
                    DeltaX = -1;
                    DeltaY = 1;
                    LocalPointX = LocalPointX + (MouseMap.Width / 2);
                    LocalPointY = LocalPointY - (MouseMap.Height / 2);
                }

                else if (TColor[0] == Color.Yellow) // Yellow
                {
                    DeltaY = -1;
                    LocalPointX = LocalPointX - (MouseMap.Width / 2);
                    LocalPointY = LocalPointY + (MouseMap.Height / 2);
                }

                else if (TColor[0] == Color.Blue) // Blue
                {
                    DeltaY = 1;
                    LocalPointX = LocalPointX - (MouseMap.Width / 2);
                    LocalPointY = LocalPointY - (MouseMap.Height / 2);
                }
            }

            MapCell.X += DeltaX;
            MapCell.Y += DeltaY;

            LocalPoint = new Point(LocalPointX, LocalPointY);

            return MapCell;
        } //END WorldToMapCell()

        #endregion

    }
}