using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgSF.Core
{
    public class DBTables 
    {
        public class SyncedFiles 
        {
            //public SyncedFiles()
            //{
                
            //}
        }

        public class TGBot
        {
            public int Id;
            public int MessageID;
            public string FileName;
            public DateTime ModifyTime;
            public string Caption;
        }


    }
}
