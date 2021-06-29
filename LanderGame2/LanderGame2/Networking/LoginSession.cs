using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LanderGame
{
    public class LoginSession
    {
        private string _username;
        private string _password;
        private MySqlConnection _mySqlConnection;
        private bool _loggedIn = false;
        private Match _match;
        private bool _admin;

        public Match Match { get => _match; set => _match = value; }

        public bool LoggedIn
        {
            get => _loggedIn;
            set => _loggedIn = value;
        }

        public bool IsAdmin
        {
            get => _admin;
            set => _admin = value;
        }

        public LoginSession(string ConnectionParameters)
        {
            try
            {
                _mySqlConnection = new MySqlConnection(ConnectionParameters);
                _mySqlConnection.Open();
            }
            catch
            {
                MessageBox.Show("Error creating the mysqlconnection");
                
            }

        }
        public bool Login(string Username, string Password)
        {
            if (!_loggedIn)
            {
                List<Parameter> parameters = new List<Parameter>
                {
                    new Parameter("@user", Username, MySqlDbType.VarChar, 16),
                    new Parameter("@pass", Password, MySqlDbType.VarChar, 32)
                };
                const string sqlStatement = "CALL userLogin(@user, @pass)";
                DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement, parameters);
                DataRow dataRow = table.Rows[0];
                string statusMessage = dataRow["STATUS"].ToString();
                if (statusMessage == "BADPASS")
                {
                    string attempts = dataRow["ATTEMPTS"].ToString();
                    MessageBox.Show("Bad login! Attempts: " + attempts + "/5");
                }
                else if (statusMessage == "NOUSER")
                {
                    DialogResult dialogResult = MessageBox.Show("User does not exist. Create new user '" + Username + "'?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (dialogResult == DialogResult.Yes)
                    {
                        NewUser(Username, Password);
                        _loggedIn = true;
                    }
                    else _loggedIn = false;
                }
                else if (statusMessage == "OK")
                {
                    _loggedIn = true;
                    _username = Username;
                    _password = Password;
                    bool.TryParse(dataRow["ADMIN"].ToString(), out _admin);
                    MessageBox.Show("Logged in as " + _username + "! Admin: " + _admin.ToString());
                }
                else if (statusMessage == "LOCKED")
                {
                    MessageBox.Show("Account is locked! Please contact Administrator");
                }

                else
                {
                    MessageBox.Show("DB returned no status");
                }
            }
            else
            {
                MessageBox.Show("Session already logged in!");
            }
            return _loggedIn;
        }

        private void NewUser(string user, string password)
        {
            List<Parameter> parameters = new List<Parameter>
            {
                new Parameter("@user", user, MySqlDbType.VarChar, 16),
                new Parameter("@pass", password, MySqlDbType.VarChar, 32)
            };
            const string sqlStatement = "CALL newUser(@user, @pass)";
            DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement, parameters);
            DataRow dataRow = table.Rows[0];
            string statusMessage2 = dataRow["STATUS"].ToString();
            if (statusMessage2 == "OK")
            {
                MessageBox.Show("New User in: " + statusMessage2);
                _loggedIn = true;
                _username = user;
                _password = password;
            }
            else MessageBox.Show("Unable to create new user: " + statusMessage2);
        }
        public DataTable ListLanders()
        {
            List<Parameter> parameters = new List<Parameter>();
            if (_loggedIn)
            {
                const string sqlStatement = "CALL getLanders()";
                return LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement, parameters);
            }
            else
            {
                MessageBox.Show("Session not logged in!");
                return new DataTable();
            }
        }
        public DataTable ListMaps()
        {
            List<Parameter> parameters = new List<Parameter>();
            if (_loggedIn)
            {
                const string sqlStatement = "CALL getMaps()";
                return LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement, parameters);
            }
            else
            {
                MessageBox.Show("Session not logged in!");
                return new DataTable();
            }
        }


        public DataTable GetChat()
        {
            List<Parameter> parameters = new List<Parameter>();
            if (_loggedIn)
            {
                const string sqlStatement = "CALL getChat()";
                return LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement, parameters);
            }
            else
            {
                MessageBox.Show("Session not logged in!");
                return new DataTable();
            }
        }
        public void SendChat(string message)
        {

            if (_loggedIn)
            {
                List<Parameter> parameters = new List<Parameter>
                {
                    new Parameter("@playerName", _username,MySqlDbType.VarChar,16),
                    new Parameter("@message",message,MySqlDbType.VarChar,255)
                };
                const string sqlStatement = "CALL sendChat(@playerName, @message)";
                DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement, parameters);
                DataRow row = table.Rows[0];
                string statusMessage = row["STATUS"].ToString();
                if (statusMessage == "OK")
                {
                    Console.WriteLine("Message sent: " + message);
                }
                else throw new Exception("SQL ERROR: " + statusMessage);
            }
            else
            {
                throw new Exception("Session not logged in!");
            }
        }
        public void NewMatch(string landerName, string mapName)
        {

            if (_loggedIn)
            {
                List<Parameter> parameters = new List<Parameter>
                {
                    new Parameter("@landerName", landerName,MySqlDbType.VarChar,16),
                    new Parameter("@playerName",_username,MySqlDbType.VarChar,16),
                    new Parameter("@mapName", mapName,MySqlDbType.VarChar,16)
                };
                const string sqlStatement = "CALL newGame(@landerName, @playerName, @mapName)";
                DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement, parameters);
                DataRow row = table.Rows[0];
                string statusMessage = row["STATUS"].ToString();
                if (statusMessage == "OK")
                {
                    _match = new Match(Convert.ToInt32(row["GameID"]), GetMap(mapName));
                    Game gam = new Game(this);
                    gam.Run();
                }
                else throw new Exception("SQL ERROR: " + statusMessage);
            }
            else
            {
                throw new Exception("Session not logged in!");
            }
        }

        public void NewUserAdmin(User user)
        {

            if (_loggedIn)
            {
                if (_admin)
                {
                    List<Parameter> parameters = new List<Parameter>
                    {
                        new Parameter("@username", _username, MySqlDbType.VarChar, 16),
                        new Parameter("@password", _password, MySqlDbType.VarChar, 32),
                        new Parameter("@targetName", user.Username, MySqlDbType.VarChar, 16),
                        new Parameter("@targetPass", user.Password, MySqlDbType.VarChar, 32),
                        new Parameter("@targetIs_Admin", Convert.ToInt16(user.IsAdmin).ToString(), MySqlDbType.Bit),
                        new Parameter("@targetHighScore", user.HighScore.ToString(), MySqlDbType.Int32),
                        new Parameter("@targetLoginAttempts", user.LoginAttempts.ToString(), MySqlDbType.Int32),
                        new Parameter("@targetLocked", Convert.ToInt16(user.Locked).ToString(), MySqlDbType.Bit)
                    };
                    const string sqlStatement =
                        "CALL newUserAdmin(@username, @password, @targetName, @targetPass, @targetIs_Admin, @targetHighScore, @targetLoginAttempts, @targetLocked)";
                    DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement, parameters);
                    DataRow row = table.Rows[0];
                    string statusMessage = row["STATUS"].ToString();
                    if (statusMessage == "OK")
                    {
                        Console.WriteLine("New user " + user.Username + ": " + statusMessage);
                    }
                    else throw new Exception("SQL ERROR: " + statusMessage);
                }
                else throw new Exception("Not Admin!");
            }
            else
            {
                throw new Exception("Session not logged in!");
            }
        }
        public void EditUserAdmin(User user, string newUsername = null)
        {
            if (newUsername == null)
            {
                newUsername = user.Username;
            }
            if (_loggedIn)
            {
                if (_admin)
                {
                    List<Parameter> parameters = new List<Parameter>
                    {
                        new Parameter("@username", _username, MySqlDbType.VarChar, 16),
                        new Parameter("@password", _password, MySqlDbType.VarChar, 32),
                        new Parameter("@targetOldName", user.Username, MySqlDbType.VarChar, 16),
                        new Parameter("@targetNewName", newUsername, MySqlDbType.VarChar, 16),
                        new Parameter("@targetPass", user.Password, MySqlDbType.VarChar, 32),
                        new Parameter("@targetIs_Admin", Convert.ToInt16(user.IsAdmin).ToString(), MySqlDbType.Bit),
                        new Parameter("@targetHighScore", user.HighScore.ToString(), MySqlDbType.Int32),
                        new Parameter("@targetLoginAttempts", user.LoginAttempts.ToString(), MySqlDbType.Int32),
                        new Parameter("@targetLocked", Convert.ToInt16(user.Locked).ToString(), MySqlDbType.Bit)
                    };
                    const string sqlStatement =
                        "CALL updateUserAdmin(@username, @password, @targetOldName, @targetNewName, @targetPass, @targetIs_Admin, @targetHighScore, @targetLoginAttempts, @targetLocked)";
                    DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement, parameters);
                    DataRow row = table.Rows[0];
                    string statusMessage = row["STATUS"].ToString();
                    if (statusMessage == "OK")
                    {
                        Console.WriteLine("Edit user " + newUsername + ": " + statusMessage);
                    }
                    else throw new Exception("SQL ERROR: " + statusMessage);
                }
                else throw new Exception("Not Admin!");
            }
            else
            {
                throw new Exception("Session not logged in!");
            }
        }

        public void DeleteUserAdmin(string target)
        {

            if (_loggedIn)
            {
                if (_admin)
                {
                    List<Parameter> parameters = new List<Parameter>
                    {
                        new Parameter("@username", _username, MySqlDbType.VarChar, 16),
                        new Parameter("@password", _password, MySqlDbType.VarChar, 32),
                        new Parameter("@target", target, MySqlDbType.VarChar, 16),
                    };
                    const string sqlStatement =
                        "CALL deleteUserAdmin(@username, @password, @target)";
                    DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement, parameters);
                    DataRow row = table.Rows[0];
                    string statusMessage = row["STATUS"].ToString();
                    if (statusMessage == "OK")
                    {
                        Console.WriteLine("Deleted user: " + target);
                    }
                    else throw new Exception("SQL ERROR: " + statusMessage);
                }
                else throw new Exception("Not Admin!");
            }
            else
            {
                throw new Exception("Session not logged in!");
            }
        }
        public void DeleteChatAdmin(string messageId)
        {

            if (_loggedIn)
            {
                if (_admin)
                {
                    List<Parameter> parameters = new List<Parameter>
                    {
                        new Parameter("@username", _username, MySqlDbType.VarChar, 16),
                        new Parameter("@password", _password, MySqlDbType.VarChar, 32),
                        new Parameter("@target", messageId, MySqlDbType.VarChar, 16),
                    };
                    const string sqlStatement =
                        "CALL deleteChatAdmin(@username, @password, @target)";
                    DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement, parameters);
                    DataRow row = table.Rows[0];
                    string statusMessage = row["STATUS"].ToString();
                    if (statusMessage == "OK")
                    {
                        MessageBox.Show("Deleted message ID: " + messageId);
                    }
                    else MessageBox.Show("SQL ERROR: " + statusMessage);
                }
                else MessageBox.Show("Not Admin!");
            }
            else
            {
                MessageBox.Show("Session not logged in!");
            }
        }
        private Map GetMap(string mapName)
        {
            List<Parameter> parameters = new List<Parameter>
            {
                new Parameter("@mapName", mapName, MySqlDbType.VarChar,16)
            };
            if (_loggedIn)
            {
                const string sqlStatement = "CALL getMap(@mapName)";
                DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement, parameters);
                return new Map(table);  
            }
            else
            {
                throw new Exception("Session not logged in!");
            }
        }
        public string UpdateShip()
        {
            List<Parameter> parameters = new List<Parameter>
            {
                new Parameter("@gameID", _match.SessionId.ToString(), MySqlDbType.Int32)
            };
            if (_loggedIn)
            {
                if (_match != null)
                {
                    const string sqlStatement = "CALL updateShip(@gameID)";
                    DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement, parameters);
                    DataRow row = table.Rows[0];
                    string status = row["STATUS"].ToString();
                    Console.WriteLine(status + " " + _match.ToString());
                    
                    if(status == "OK")
                    {
                        _match.PosX = (float)Convert.ToDecimal(row["POSX"]);
                        _match.PosY = (float)Convert.ToDecimal(row["POSY"]);
                        _match.VelX = (float)Convert.ToDecimal(row["VELX"]);
                        _match.VelY = (float)Convert.ToDecimal(row["VELY"]);
                        _match.Rotation = (float)Convert.ToDecimal(row["ROTATION"]);
                        _match.Thruster = Convert.ToBoolean(row["ENGINE"]);
                        return "OK";
                    }
                    else
                    {
                        return status;
                    }
                }
                else throw new Exception("Not in a match!");
            }
            else
            {
                throw new Exception("Session not logged in!");
            }
        }

        public void EndMatch()
        {
            List<Parameter> parameters = new List<Parameter>
            {
                new Parameter("@gameID", _match.SessionId.ToString(), MySqlDbType.Int32)
            };
            if (_loggedIn)
            {
                if (_match != null)
                {
                    const string sqlStatement = "CALL endGame(@gameID)";
                    DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement, parameters);
                    DataRow row = table.Rows[0];
                    Console.WriteLine("Game Ended: " + row["STATUS"]);
                }
                else throw new Exception("Not in a match!");
            }
            else
            {
                throw new Exception("Session not logged in!");
            }
        }

        public DataTable ListUsers()
        {
            if (_loggedIn)
            {
                if (_admin)
                {
                    List<Parameter> parameters = new List<Parameter>
                    {
                        new Parameter("@username", _username, MySqlDbType.VarChar, 16),
                        new Parameter("@password", _password, MySqlDbType.VarChar, 32)
                    };
                    const string sqlStatement =
                        "CALL listUsersAdmin(@username, @password)";
                    DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement, parameters);
                    DataRow row = table.Rows[0];
                    string statusMessage = row["STATUS"].ToString();
                    if (statusMessage == "OK")
                    {
                        return table;
                    }
                    else{
                        MessageBox.Show("SQL ERROR: " + statusMessage);
                        return new DataTable();
                    };
                }
                else
                {
                    List<Parameter> parameters = new List<Parameter>();
                    const string sqlStatement =
                        "CALL listUsers()";
                    DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement, parameters);
                    DataRow row = table.Rows[0];
                    string statusMessage = row["STATUS"].ToString();
                    if (statusMessage == "OK")
                    {
                        return table;
                    }
                    else
                    {
                        MessageBox.Show("SQL ERROR: " + statusMessage);
                        return new DataTable();
                    }
                }
            }
            else
            {
                MessageBox.Show("Session not logged in!");
                return new DataTable();
            }
        }

        public User GetUserByNameAdmin(string target)
        {
            if (_loggedIn)
            {
                if (_admin)
                {
                    List<Parameter> parameters = new List<Parameter>
                    {
                        new Parameter("@username", _username, MySqlDbType.VarChar, 16),
                        new Parameter("@password", _password, MySqlDbType.VarChar, 32),
                        new Parameter("@target", target, MySqlDbType.VarChar, 16)
                    };
                    const string sqlStatement =
                        "CALL getUserByNameAdmin(@username, @password, @target)";
                    DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement, parameters);
                    DataRow row = table.Rows[0];
                    string statusMessage = row["STATUS"].ToString();
                    if (statusMessage == "OK")
                    {
                        User user = new User();
                        user.Username = row["Username"].ToString();
                        user.Password = row["Password"].ToString();
                        user.IsAdmin = Convert.ToBoolean(row["is_Admin"]);
                        user.HighScore = Convert.ToInt32(row["HighScore"]);
                        user.HighScore = Convert.ToInt32(row["LoginAttempts"]);
                        user.Locked = Convert.ToBoolean(row["Locked"]);
                        return user;
                    }
                    else throw new Exception("SQL ERROR: " + statusMessage);
                }
                else throw new Exception("Not Admin!");
            }
            else
            {
                throw new Exception("Session not logged in!");
            }
        }
        public void ControlRocket(float rotationAngle, bool thruster)
        {

            if (_loggedIn)
            {
                if (_match != null)
                {
                    List<Parameter> parameters = new List<Parameter>
                    {
                        new Parameter("@engine", Convert.ToInt16(thruster).ToString(), MySqlDbType.Bit),
                        new Parameter("@angle", rotationAngle.ToString(), MySqlDbType.Float),
                        new Parameter("@gameID", _match.SessionId.ToString(), MySqlDbType.Int32)
                    };
                    const string sqlStatement = "CALL controlRocket(@engine, @angle, @gameID)";
                    DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement, parameters);
                    DataRow row = table.Rows[0];
                    Console.WriteLine("Updated rocket: " + row["STATUS"]);
                }
                else
                {
                    throw new Exception("Game session not initialized!");
                }
            }
            else
            {
                throw new Exception("Session not logged in!");
            }
        }
        public void Logout()
        {
            if (_loggedIn)
            {
                List<Parameter> parameters = new List<Parameter>
                {
                    new Parameter("@user", _username, MySqlDbType.VarChar, 16),
                    new Parameter("@pass", _password, MySqlDbType.VarChar, 32)
                };
                const string sqlStatement = "CALL userLogout(@user, @pass)";
                DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement, parameters);
                DataRow dataRow = table.Rows[0];
                string statusMessage2 = dataRow["STATUS"].ToString();
                if (statusMessage2 == "OK")
                {
                    _mySqlConnection.Close();
                    _loggedIn = false;
                    MessageBox.Show("Logged out. Goodbye");
                }
                else throw new Exception("Unable to logout: " + statusMessage2);

            }
            else
            {
                throw new Exception("Session not logged in!");
            }

        }
        
    }
}
