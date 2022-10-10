using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgSF.Core
{
    public class DBSQLServerUtils
    {
        public static SqliteConnection GetDBConnection()
        {
            string sqlCon = @"Data Source=MainDB.db;Mode=ReadWriteCreate;";
            SqliteConnection con = new SqliteConnection(sqlCon);
            return con;
        }
    }
}
