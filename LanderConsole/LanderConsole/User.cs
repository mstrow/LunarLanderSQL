namespace LanderConsole
{
    public struct User
    {
        private string _username;
        private string _password;
        private bool _isAdmin;
        private int _highScore;
        private int _loginAttempts;
        private bool _locked;

        public User(string Username, string Password, bool IsAdmin, int HighScore, int LoginAttempts, bool Locked)
        {
            _username = Username;
            _password = Password;
            _isAdmin = IsAdmin;
            _highScore = HighScore;
            _loginAttempts = LoginAttempts;
            _locked = Locked;
        }

        public string Username
        {
            get => _username;
            set => _username = value;
        }

        public string Password
        {
            get => _password;
            set => _password = value;
        }

        public bool IsAdmin
        {
            get => _isAdmin;
            set => _isAdmin = value;
        }

        public int HighScore
        {
            get => _highScore;
            set => _highScore = value;
        }

        public int LoginAttempts
        {
            get => _loginAttempts;
            set => _loginAttempts = value;
        }

        public bool Locked
        {
            get => _locked;
            set => _locked = value;
        }
    }
}
