using System.Data;

namespace LanderGame
{
    public class Match
    {
        private int _sessionId;
        private float _posX, _posY, _velX, _velY;
        private float _rotation;
        private bool _thruster;
        private Map _map;
        public Match(int sessionId, Map map)
        {
            SessionId = sessionId;
            _map = map;
            
        }
        public override string ToString()
        {
            return $"X:{_posX}, Y:{_posY}, Rotation:{_rotation}, Thruster: {_thruster.ToString()}, Velocity: X: {_velX}, Y: {_velY}";
        }
        public int SessionId
        {
            get => _sessionId;
            set => _sessionId = value;
        }


        public float PosX
        {
            get => _posX;
            set => _posX = value;
        }

        public float PosY
        {
            get => _posY;
            set => _posY = value;
        }

        public float Rotation
        {
            get => _rotation;
            set => _rotation = value;
        }

        public bool Thruster
        {
            get => _thruster;
            set => _thruster = value;
        }
        public Map Map { get => _map; set => _map = value; }
        public float VelX { get => _velX; set => _velX = value; }
        public float VelY { get => _velY; set => _velY = value; }
    }
}
