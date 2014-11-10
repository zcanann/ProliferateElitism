using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace River
{
    [Serializable()]
    class MapCell
    {
        //Base/height/surface tiles
        public int TileID = -1;
        public List<int> HeightTiles = new List<int>();
        public List<int> SurfaceTiles = new List<int>();

        public EventType EventType = EventType.None;
        public bool Walkable = false;


        public bool Flipped = false;
        public bool IgnoreHeight = false;

        //Could be built into objects perhaps:
        public int SlopeMap = -1;

        public MapCell(int TileID)
        {
            this.TileID = TileID;
        }

        public void SetBaseTile(int TileID, bool Walkable)
        {
            this.TileID = TileID;
            this.Walkable = Walkable;
        }

        public void SetBaseTile(int TileID)
        {
            this.TileID = TileID;
        }

        public void AddHeightTile(int TileID)
        {
            HeightTiles.Add(TileID);
        }

        public void AddSurfaceTile(int TileID)
        {
            SurfaceTiles.Add(TileID);
        }
    }

    public enum EventType
    {
        None,
        PlayerSpawn,
        Exit,
        Waypoint,
        Chest,
        Weaponrack,
        Shop,
        Storage,
        Enchanter,
    }
}
