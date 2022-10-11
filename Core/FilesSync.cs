using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using static TgSF.Core.DBTables;

namespace TgSF.Core
{
    public class FilesSync
    {
        public string FileName;
        public string FilePath;
        public DateTime CreationTime;
        public DateTime ModifyTime;
        public string TGMessageID;

        public FilesSync()
        {
        }

        public FilesSync(FilesSync fs)
        {
            FileName = fs.FileName;
            FilePath = fs.FilePath;            ;
            CreationTime = fs.CreationTime;
            ModifyTime = fs.ModifyTime;
            TGMessageID = fs.TGMessageID;
        }

        public FilesSync(string fileName, string filePath, DateTime creationTime, DateTime modifyTime, string tGMessageID)
        {
            FileName = fileName;
            FilePath = filePath;
            CreationTime = creationTime;
            ModifyTime = modifyTime;
            TGMessageID = tGMessageID;
        }

        public List<FilesSync> Sync()
        {
            List <FilesSync> files = new List<FilesSync>();
            
            foreach (var element in Directory.GetFiles(Settings.SyncPath))
            {
                

                FilePath = element;
                List<string> splittedFile = element.Split('\\').ToList();
                FileName = splittedFile.Last();           

                if (Regex.IsMatch(FileName, @"^[~$]."))
                    continue;

                CreationTime = File.GetCreationTime(element);
                ModifyTime = File.GetLastWriteTime(element);
                TGMessageID = "test";
                FilesSync filesSync = new FilesSync(this);
                if (Settings.DataBase.FindFileId(filesSync) != -1) continue;
                files.Add(filesSync);
            }
            return files;
        }

    }
}
