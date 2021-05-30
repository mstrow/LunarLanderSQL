using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace LanderConsole
{
    public class LanderSession
    {
        private string _username;
        private string _password;
        private MySqlConnection _mySqlConnection;
        private bool _loggedIn = false;
        private Match _match;
        private bool _admin;

        public LanderSession(string ConnectionParameters)
        {
            try
            {
                _mySqlConnection = new MySqlConnection(ConnectionParameters);
                _mySqlConnection.Open();
            }
            catch
            {
                throw new Exception("Error creating the mysqlconnection");
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
                Console.WriteLine("Login Message: " + statusMessage);
                if (statusMessage == "BADPASS")
                {
                    string attempts = dataRow["ATTEMPTS"].ToString();
                    Console.WriteLine("Attempts: " + attempts + "/5");
                }
                else if (statusMessage == "NOUSER")
                {
                    NewUser(Username,Password);
                }
                else if (statusMessage == "OK")
                {
                    _loggedIn = true;
                    _username = Username;
                    _password = Password;
                    bool.TryParse(dataRow["ADMIN"].ToString(), out _admin);
                    Console.WriteLine("Logged in as " + _username + "! Admin: " + _admin.ToString());
                }
                else if (statusMessage == "LOCKED")
                {
                    Console.WriteLine("Account is locked");
                }
                
                else
                {
                    throw new Exception("DB returned no status");
                }   
            }
            else
            {
                throw new Exception("Session already logged in!");
            }
            return _loggedIn;
        }

        private void NewUser(string user, string password)
        {
            Console.WriteLine("Creating new user");
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
                Console.WriteLine("New User in: " + statusMessage2);
                _loggedIn = true;
                _username = user;
                _password = password;
            }
            else throw new Exception("Unable to create new user: " + statusMessage2);
        }
        public void ListLanders()
        {
            List<Parameter> parameters = new List<Parameter>();
            if (_loggedIn)
            {
                const string sqlStatement = "CALL getLanders()";
                DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement,parameters);
                LanderSQLHelper.PrintTable(table);
            }
            else
            {
                throw new Exception("Session not logged in!");
            }
        }
        public void PrintMaps()
        {
            List<Parameter> parameters = new List<Parameter>();
            if (_loggedIn)
            {
                const string sqlStatement = "CALL getMaps()";
                DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement,parameters);
                LanderSQLHelper.PrintTable(table);
            }
            else
            {
                throw new Exception("Session not logged in!");
            }
        }

            
        public void PrintChat()
        {
            List<Parameter> parameters = new List<Parameter>();
            if (_loggedIn)
            {
                const string sqlStatement = "CALL getChat()";
                DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement,parameters);
                LanderSQLHelper.PrintTable(table);
            }
            else
            {
                throw new Exception("Session not logged in!");
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
                DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement,parameters);
                DataRow row = table.Rows[0];
                string statusMessage = row["STATUS"].ToString();
                if (statusMessage == "OK")
                {
                    Console.WriteLine("Message sent: " +message);
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
                DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement,parameters);
                DataRow row = table.Rows[0];
                string statusMessage = row["STATUS"].ToString();
                if (statusMessage == "OK")
                {
                    _match = new Match(Convert.ToInt32(row["GameID"]), mapName);
                    GetMap();
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
        private void GetMap()
        {
            List<Parameter> parameters = new List<Parameter>
            {
                new Parameter("@mapName", _match.MapName, MySqlDbType.VarChar,16)
            };
            if (_loggedIn)
            {
                if (_match != null)
                {
                    const string sqlStatement = "CALL getMap(@mapName)";
                    _match.Map = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement, parameters);
                    //LanderSQLHelper.PrintTable(table);
                }
                else throw new Exception("Game not initialized!");
            }
            else
            {
                throw new Exception("Session not logged in!");
            }
        }
        public void UpdateShip()
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
                    _match.PosX = (float) Convert.ToDecimal(row["POSX"]);
                    _match.PosY = (float) Convert.ToDecimal(row["POSY"]);
                    _match.Rotation = (float) Convert.ToDecimal(row["ROTATION"]);
                    _match.Thruster = Convert.ToBoolean(row["ENGINE"]);
                    Console.WriteLine(row["STATUS"] + " " + _match.ToString());
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

        public void ListUsers()
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
                        LanderSQLHelper.PrintTable(table);
                    }
                    else throw new Exception("SQL ERROR: " + statusMessage);
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
                        LanderSQLHelper.PrintTable(table);
                    }
                    else throw new Exception("SQL ERROR: " + statusMessage);
                }
            }
            else
            {
                throw new Exception("Session not logged in!");
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
                        
                        return new User(row["Username"].ToString(),
                            row["Password"].ToString(),
                            Convert.ToBoolean(row["HighScore"]),
                            Convert.ToInt32(row["HighScore"]),
                            Convert.ToInt32(row["LoginAttempts"]),
                            Convert.ToBoolean(row["Locked"])
                        );
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
                    DataTable table = LanderSQLHelper.RequestTable(_mySqlConnection, sqlStatement,parameters);
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
                    Console.WriteLine("Logged out!");
                }
                else throw new Exception("Unable to logout: " + statusMessage2);
                
            }
            else
            {
                throw new Exception("Session not logged in!");
            }
            
        }

        public void PrintCurrentMap()
        {
            LanderSQLHelper.PrintTable(_match.Map);
        }
    }
}
