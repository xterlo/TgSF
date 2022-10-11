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

        public void ReadData()
        {

        }

        public int FindFileId(FilesSync file)
        {
            string query = $@"SELECT Id 
                              FROM 
                                [SyncedFiles]
                              WHERE
	                            FileName='{file.FileName}' AND                                
                                FilePath='{file.FilePath}';";
            int id;
            var a = SendQueryToDB(query, true);
            if (a is null)
                return -1;
            else
                return int.Parse(a.ToString());
            
        }

        public void UpdateDataToSyncedFile(FilesSync file,FilesSync oldFile)
        {
            string query = $@"UPDATE [SyncedFiles]
                              SET
	                              [FileName] = '{file.FileName}',                                  
	                              [FilePath] = '{file.FilePath}', 
	                              [ModifyTime] = '{file.ModifyTime}'
                              WHERE [Id] = {FindFileId(oldFile)}; ";
            SendQueryToDB(query);
        }

        public void AddNewFileToDB(params FilesSync[] files)
        {
            foreach (FilesSync file in files)
            {
                string query = $@"INSERT INTO [SyncedFiles] 
                                  VALUES (
                                    NULL,
	                                '{file.FileName}',
                                    '{file.FilePath}',                                    
                                    '{file.CreationTime}',
                                    '{file.ModifyTime}',
                                    '{file.TGMessageID}');";
                SendQueryToDB(query);


            }
        }

        private object SendQueryToDB(string query,bool read=false)
        {
            var command = new SqliteCommand(query, db);
            try
            {
                db.Open();
                if (read)
                {
                    object v = command.ExecuteScalar();
                    return v;
                }
                else 
                    command.ExecuteNonQuery();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
                return false;
            }
            finally
            {
                if ((db.State == ConnectionState.Open))
                {
                    db.Close();
                }
            }
            return true;
        }
    }
}
