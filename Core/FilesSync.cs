using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static TgSF.Core.DBTables;

namespace TgSF.Core
{
    public class FilesSync
    {
        public string FileName;
        public string FilePath;
        public string FileHash;
        public DateTime CreationTime;
        public DateTime ModifyTime;
        public string TGMessageID;

        public FilesSync()
        {
        }

        public FilesSync(FilesSync fs)
        {
            FileName = fs.FileName;
            FilePath = fs.FilePath;
            FileHash = fs.FileHash;
            CreationTime = fs.CreationTime;
            ModifyTime = fs.ModifyTime;
            TGMessageID = fs.TGMessageID;
        }

        public List<FilesSync> Sync()
        {
            List <FilesSync> files = new List<FilesSync>();
            foreach (var element in Directory.GetFiles(Settings.SyncPath))
            {
                Console.WriteLine(element);
                List<string> splittedFile = element.Split('\\').ToList<string>();
                FileName = splittedFile.Last();
                FilePath = element;
                FileHash= GetFileHash();
                CreationTime = File.GetCreationTime(element);
                ModifyTime = File.GetLastWriteTime(element);
                TGMessageID = "test";
                FilesSync filesSync = new FilesSync(this);
                files.Add(filesSync);
            }
            return files;
        }

        private string GetFileHash()
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(FilePath))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

    }
}
