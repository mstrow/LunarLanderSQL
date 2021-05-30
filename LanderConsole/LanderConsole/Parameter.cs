using System.Data.SqlClient;
using MySql.Data.MySqlClient;
namespace LanderConsole
{
    public struct Parameter
    {
        private string _argument;
        private string _value;
        private MySqlDbType _type;
        private int _size;
        
        public Parameter(string argument, string value, MySqlDbType type, int size = 0)
        {
            _argument = argument;
            _value = value;
            _type = type;
            _size = size;
        }
        public MySqlParameter Convert()
        {
            MySqlParameter par;
            if (_size == 0)
            {
                par = new MySqlParameter(_argument, _type);
            }
            else
            {
                par = new MySqlParameter(_argument, _type, _size);
            }
            
            
            par.Value = _value;
            return par;
        }
    }
}
