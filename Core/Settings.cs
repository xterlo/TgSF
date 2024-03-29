﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TgSF.Core
{
    public static class Settings
    {
        private static ITelegramBotClient _tgBot;
        public static ITelegramBotClient TGBot
        {
            get => _tgBot;
            set
            {
                _tgBot = value;
            }
        }

        static readonly RegistryKey reg = Registry.CurrentUser.CreateSubKey("TgSF",true);

        static readonly string pathTgSF = reg.GetValue("path").ToString();
        static readonly string tokenTgSF = reg.GetValue("token").ToString();
        static readonly string chatidTgSF = reg.GetValue("chatid").ToString();

        private static string _syncPath = pathTgSF is null ? "" : pathTgSF;
        private static string _tgToken = tokenTgSF is null ? "" : tokenTgSF;
        private static string _chatidTgSF = chatidTgSF is null ? "" : chatidTgSF;

        public static string SyncPath
        {
            get { return _syncPath; }
            set {
                reg.SetValue("path", value);
                _syncPath = value;
            }
        }
        public static string TgToken
        {
            get { return _tgToken; }
            set {
                reg.SetValue("token", value);
                _tgToken = value;
            }
        }
        public static string ChatId
        {
            get { return _chatidTgSF; }
            set
            {
                reg.SetValue("chatid", value);
                _chatidTgSF = value;
            }
        }

        public static DBUtils DataBase;
    }
}
