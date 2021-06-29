using System;
using System.Collections.Generic;
using System.Text;

namespace LanderGame
{
    public struct Tile
    {
        private int _x;
        private int _y;
        private string _texture;

        public Tile(int x, int y, string texture)
        {
            _x = x;
            _y = y;
            _texture = texture;
        }

        public int Y { get => _y; }
        public int X { get => _x; }
        public string Texture { get => _texture;}
    }
}
