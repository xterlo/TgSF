using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TgSF.Core;

namespace TgSF.Core
{
    public class FileEventsHandler
    {
        private static FileSystemWatcher watcher;
        public static void AttachEvents()
        {
            watcher = new FileSystemWatcher(Settings.SyncPath);


            watcher.NotifyFilter =  NotifyFilters.FileName |
                                    NotifyFilters.DirectoryName |
                                    NotifyFilters.LastWrite;

            watcher.Changed += OnChanged;
            watcher.Renamed += OnRenamed;
            watcher.Created += OnCreated;
            watcher.Filter = "*.*";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
        }

        public static void WaitForCloseFile(string filePath)
        {
            watcher = new FileSystemWatcher(filePath);
            watcher.NotifyFilter = NotifyFilters.LastAccess;
            
        }

        public static void DeattachEvents()
        {
            if(watcher != null)
            {
                watcher.Changed += OnChanged;
                watcher.Renamed += OnRenamed;
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
        }


        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            //FilesSync fs = new FilesSync(e.,)
            //Settings.DataBase.AddNewFileToDB()
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            DateTime createTime = File.GetCreationTime(e.FullPath);
            DateTime modifyTime = File.GetLastWriteTime(e.FullPath);
            FilesSync fs = new FilesSync(e.Name, e.FullPath, createTime, modifyTime, "later");
            Settings.DataBase.AddNewFileToDB(fs);
        }

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            DateTime createTime = File.GetCreationTime(e.FullPath);
            DateTime modifyTime = File.GetLastWriteTime(e.FullPath);
            FilesSync fs = new FilesSync(e.Name, e.FullPath, createTime, modifyTime, "later");
            FilesSync fsOld = new FilesSync(e.OldName, e.OldFullPath, createTime, modifyTime, "later");
            Settings.DataBase.UpdateDataToSyncedFile(fs, fsOld);
        }
    }
}
