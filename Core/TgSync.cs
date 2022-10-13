using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgSF.Core
{
    public class TgSync
    {
        public int MessageID;
        public string Caption;
        public string FileName;
        public DateTime SendTime;

        public TgSync(Telegram.Bot.Types.Message msg)
        {
            MessageID = msg.MessageId;
            Caption = msg.Caption;
            FileName = msg.Document.FileName;
            SendTime = msg.Date;
        }
    }
}
