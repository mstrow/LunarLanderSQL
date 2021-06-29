using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Diagnostics;
namespace LanderGame
{
    public static class LanderSQLHelper
    {
        public static DataTable RequestTable(MySqlConnection conn, string statement, List<Parameter> pars)
        {
            List<MySqlParameter> mySqlParameters = new List<MySqlParameter>();
            foreach (var par in pars)
            {
                mySqlParameters.Add(par.Convert());
            }
            DataTable table;
            //try
            //{
            table = MySqlHelper.ExecuteDataset(conn, statement, mySqlParameters.ToArray()).Tables[0];
            //}
            //catch
            //{
            //throw new Exception("Error executing SQL query");
            //}

            return table;
        }

        public static void PrintTable(DataTable table)
        {

            foreach (DataColumn column in table.Columns)
            {
                Debug.Write("| ");
                Debug.Write(column.ColumnName);
            }
            foreach (DataRow row in table.Rows)
            {
                Debug.WriteLine("");
                foreach (object obj in row.ItemArray)
                {
                    string cell = obj.ToString();
                    Debug.Write("| ");
                    Debug.Write(cell);
                }
            }
            Debug.WriteLine("");
            Debug.WriteLine("------------");
            Debug.WriteLine("");
        }
    }
}
