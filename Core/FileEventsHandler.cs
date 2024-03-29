﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TgSF.Core;

namespace TgSF.Core
{
    public class FileEventsHandler
    {
        private static FileSystemWatcher watcher;

        private enum TypeQuery
        {
            rename,
            update,
            NewFile
        }

        private class QueueClass
        {
            public TypeQuery type;
            public FilesSync fs;
            public FilesSync oldFs = null;

            public QueueClass(TypeQuery tq, FilesSync fs, FilesSync oldFs = null)
            {
                this.type = tq;
                this.fs = fs;
                this.oldFs = oldFs;
            }
        }
        List<QueueClass> queue = new List<QueueClass>();

        public void AttachEvents()
        {
            watcher = new FileSystemWatcher(Settings.SyncPath);


            watcher.NotifyFilter = NotifyFilters.FileName |
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

        public void DeattachEvents()
        {
            if (watcher != null)
            {
                watcher.Changed += OnChanged;
                watcher.Renamed += OnRenamed;
                watcher.Created += OnCreated;
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
        }


        private async void OnChanged(object sender, FileSystemEventArgs e)
        {
            DateTime createTime = File.GetCreationTime(e.FullPath);
            DateTime modifyTime = File.GetLastWriteTime(e.FullPath);
            FilesSync fs = new FilesSync(e.Name, e.FullPath, createTime, modifyTime, 0);
            QueueClass queueClass = new QueueClass(TypeQuery.update, fs);
            
            if (queue.Count == 0)
            {
                queue.Add(queueClass);
                new Thread(WaitForFilePermissions).Start();
            }
            else
                queue.Add(queueClass);
        }

        private async void OnCreated(object sender, FileSystemEventArgs e)
        {
            DateTime createTime = File.GetCreationTime(e.FullPath);
            DateTime modifyTime = File.GetLastWriteTime(e.FullPath);
            FilesSync fs = new FilesSync(e.Name, e.FullPath, createTime, modifyTime, -1);
            QueueClass queueClass = new QueueClass(TypeQuery.NewFile, fs);
            if (queue.Count == 0)
            {
                queue.Add(queueClass);
                new Thread(WaitForFilePermissions).Start();
            }
            else
                queue.Add(queueClass);
        }

        private async void OnRenamed(object sender, RenamedEventArgs e)
        {
            DateTime createTime = File.GetCreationTime(e.FullPath);
            DateTime modifyTime = File.GetLastWriteTime(e.FullPath);
            FilesSync fs = new FilesSync(e.Name, e.FullPath, createTime, modifyTime, 0);
            FilesSync fsOld = new FilesSync(e.OldName, e.OldFullPath, createTime, modifyTime, 0);
            QueueClass queueClass = new QueueClass(TypeQuery.rename, fs,fsOld);
            if (queue.Count == 0)
            {
                queue.Add(queueClass);
                new Thread(WaitForFilePermissions).Start();
            }
            else
                queue.Add(queueClass);
        }

        private async void WaitForFilePermissions()
        {
            Console.WriteLine("START THREAD");
            while (queue.Count > 0)
            {
                try
                {

                    foreach (var file in queue)
                    {
                        switch (file.type)
                        {
                            case TypeQuery.NewFile:
                                {
                                    try
                                    {
                                            Settings.DataBase.AddNewFileToDB(file.fs);
                                            queue.Remove(file);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Create Sync Error: {ex.Message}");
                                    }
                                    break;
                                }
                            case TypeQuery.rename:
                                {
                                    try
                                    {
                                        //if (!(TgFunctions.TgChat.SendFile(file.fs) is null))
                                        //{
                                        file.oldFs.TGMessageID = -1;
                                            Settings.DataBase.UpdateDataToSyncedFile(file.fs, file.oldFs);
                                            queue.Remove(file);
                                        //}
                                    }

                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Create Sync Error: {ex.Message}");
                                    }
                                    break;
                                }
                            case TypeQuery.update:
                                {
                                    try
                                    {
                                        if (file.fs.TGMessageID == 0)
                                        {
                                            var msg = await TgFunctions.TgChat.SendFile(file.fs);
                                            if (!(msg is null))
                                            {
                                                Settings.DataBase.AddMsgInfo(new TgSync(msg));
                                                file.fs.TGMessageID = msg.MessageId;
                                                Settings.DataBase.UpdateDataToSyncedFile(file.fs);
                                                queue.Remove(file);
                                            }
                                        }
                                        else
                                        {
                                            var msg = await TgFunctions.TgChat.EditMessage(file.fs);
                                            Settings.DataBase.UpdateDataToSyncedFile(file.fs);
                                            queue.Remove(file);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Create Sync Error: {ex.Message}");
                                    }
                                    break;
                                }
                        }
                    }
                }
                catch
                {
                    Console.WriteLine($"Queue was cleared, end");
                }
                Thread.Sleep(1000);
            }
        }
    }
}
