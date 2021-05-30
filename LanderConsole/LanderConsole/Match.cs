using System.Data;

namespace LanderConsole
{
    public class Match
    {
        private int _sessionId;
        private string _mapNameName;
        private float _posX, _posY;
        private float _rotation;
        private bool _thruster;
        private DataTable _map;
        public Match(int sessionId, string mapName)
        {
            SessionId = sessionId;
            MapName = mapName;
        }
        public override string ToString()
        {
            return $"X:{_posX}, Y:{_posY}, Rotation:{_rotation}, Thruster: {_thruster.ToString()}";
        }
        public int SessionId
        {
            get => _sessionId;
            set => _sessionId = value;
        }

        public string MapName
        {
            get => _mapNameName;
            set => _mapNameName = value;
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

        public DataTable Map
        {
            get => _map;
            set => _map = value;
        }
    }
}
