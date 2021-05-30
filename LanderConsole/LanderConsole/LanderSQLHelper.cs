using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
namespace LanderConsole
{
    public static class LanderSQLHelper
    {
        public static DataTable RequestTable(MySqlConnection conn, string statement, List<Parameter> pars)
        {
            List<MySqlParameter> mySqlParameters = new List<MySqlParameter>();
            foreach(var par in pars)
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
                Console.Write("| ");
                Console.Write(column.ColumnName);
            }
            foreach (DataRow row in table.Rows)
            {
                Console.WriteLine("");
                foreach (object obj in row.ItemArray)
                {
                    string cell = obj.ToString();
                    Console.Write("| ");
                    Console.Write(cell);
                }
            }
            Console.WriteLine("");
            Console.WriteLine("------------");
            Console.WriteLine("");
        }
    }
}
