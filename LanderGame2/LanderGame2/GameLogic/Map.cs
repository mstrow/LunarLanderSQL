using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LanderGame
{ 
    public class Map
    {
        private string _name;
        private List<Tile> _tiles = new List<Tile>();

        public string Name { get => _name;}
        public List<Tile> Tiles { get => _tiles; }

        public Map (DataTable table)
        {
            _name = table.Rows[0]["Map"].ToString();
            foreach (DataRow row in table.Rows)
            {
                int x = Convert.ToInt32(row["x"]);
                int y = Convert.ToInt32(row["y"]);
                string texture = row["Texture"].ToString();
                Tile tile = new Tile(x, y, texture);
                _tiles.Add(tile);
            }
        }
    }
}
