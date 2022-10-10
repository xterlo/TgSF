using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TgSF.Core
{
    

    public enum DBQuery
    {
        WriteData,
        ReadData
    }
    
    public class DBUtils
    {
        SqliteConnection db;

        public DBUtils(SqliteConnection db)
        {
            this.db = db;
            if (!File.Exists(@"MainDB.db"))
            {
                string query = @"CREATE TABLE [SyncedFiles]
                                (
	                                [Id] INT NOT NULL PRIMARY KEY,
	                                [FileName] TEXT NOT NULL ,
	                                [FilePath] TEXT NOT NULL,
	                                [FileHash] TEXT NOT NULL,
	                                [CreationTime] DATETIME NOT NULL,
	                                [ModifyTime] DATETIME NOT NULL,
	                                [TGMessageID] TEXT DEFAULT NULL
                                );
                                CREATE TABLE [TGBot]
                                (
	                                [Id] INT NOT NULL PRIMARY KEY,
	                                [MessageID] INT NOT NULL,
	                                [FileName] TEXT NOT NULL,
	                                [ModifyTime] DATETIME NOT NULL,
	                                [Caption] TEXT NOT NULL
                                );
                                ";

                var command = new SqliteCommand(query, db);
                try
                {
                    db.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Database is created successfully", "MyProgram");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR");
                }
                finally
                {
                    if ((db.State == ConnectionState.Open))
                    {
                        db.Close();
                    }
                }
            }
           
        }

        public void ReadData()
        {

        }

        public void WriteData()
        {

        }
    }
}
