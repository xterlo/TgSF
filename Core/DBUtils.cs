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
using static TgSF.Core.DBTables;

namespace TgSF.Core
{


    public enum DBQuery
    {
        WriteData,
        ReadData
    }

    public class DBUtils : FilesSync
    {
        SqliteConnection db;

        public DBUtils(SqliteConnection db)
        {
            this.db = db;
            if (!File.Exists(@"MainDB.db"))
            {
                string query = @"CREATE TABLE [SyncedFiles]
                                (
	                                [Id] INTEGER PRIMARY KEY NOT NULL,
	                                [FileName] TEXT NOT NULL ,
	                                [FilePath] TEXT NOT NULL,
	                                [FileHash] TEXT NOT NULL,
	                                [CreationTime] DATETIME NOT NULL,
	                                [ModifyTime] DATETIME NOT NULL,
	                                [TGMessageID] TEXT DEFAULT NULL
                                );
                                CREATE TABLE [TGBot]
                                (
	                                [Id] INTEGER PRIMARY KEY NOT NULL,
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
                    MessageBox.Show("Database is created successfully", "TGFS");
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

        public static void ReadData()
        {

        }

        public void WriteDataToSyncedFile(List<FilesSync> files)
        {
            foreach (FilesSync file in files)
            {
                string query = $@"INSERT INTO [SyncedFiles] VALUES (
                NULL,
	            '{file.FileName}',
                '{file.FilePath}',
                '{file.FileHash}',
                '{file.CreationTime}',
                '{file.ModifyTime}',
                '{file.TGMessageID}');
                ";
                var command = new SqliteCommand(query, db);
                try
                {
                    db.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("ZAEBIS", "TGFS");
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
    }
}
