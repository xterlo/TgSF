using Microsoft.Win32;

using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgSF.Core;
using static TgSF.Core.TgFunctions;

namespace TgSF.MVVM.ModelView
{
    public class MainWindowViewModel : ObserverObject
    {
        public ICommand WindowLoadedCommand { get => new RelayCommand(WindowLoaded); }
        public ICommand ChoosePathCommand { get => new RelayCommand(ChoosePath); }
        public ICommand SyncFilesCommand { get => new RelayCommand(SyncFiles); }
        public ICommand TgTokenCheckerCommand { get => new RelayCommand<Task>((e) => { TgTokenChecker(); }); }
        public ICommand TgChatIdCheckerCommand { get => new RelayCommand<Task>((e) => { TgChatIdChecker(); }); }

        private bool isHiddenAuth = false;

        private string _tgToken= Settings.TgToken;
        public string TgToken
        {
            get => _tgToken;
            set
            {
                _tgToken = value;
                Settings.TgToken = value;
                OnPropertyChanged(nameof(TgToken));
            }
        }

        private string _syncPath = Settings.SyncPath;
        public string SyncPath
        {
            get => _syncPath;
            set
            {
                _syncPath = value;
                Settings.SyncPath = value;
                OnPropertyChanged(nameof(SyncPath));
            }
        }

        private string _chatID = Settings.ChatId;
        public string ChatId
        {
            get => _chatID;
            set
            {
                _chatID = value;
                Settings.ChatId = value;
                OnPropertyChanged(nameof(ChatId));
            }
        }

        private bool _isChatIdEnabled=false;
        public bool IsChatIdEnabled
        {
            get => _isChatIdEnabled;
            set {
                _isChatIdEnabled = value;
                OnPropertyChanged(nameof(IsChatIdEnabled));
            }

        }

        public async void WindowLoaded()
        {
            FileEventsHandler fileEventsHandler = new FileEventsHandler();
            isHiddenAuth = true;
            if (!string.IsNullOrEmpty(Settings.TgToken))
                await TgTokenChecker();

            if (!string.IsNullOrEmpty(Settings.ChatId) && !(Settings.TGBot is null))
                await TgChatIdChecker();
            
            Settings.DataBase =new DBUtils(DBSQLServerUtils.GetDBConnection()) ;
            isHiddenAuth = false;


            #region Do After Check Changes When Program Was Closed
            SyncFiles();
            fileEventsHandler.AttachEvents();

            #endregion
        }

        public void ChoosePath()
        {
            VistaFolderBrowserDialog DialogPath = new VistaFolderBrowserDialog();
            if (DialogPath.ShowDialog() == true)
            {
                SyncPath = DialogPath.SelectedPath;
            }
        }

        public async Task TgTokenChecker()
        {
            IsChatIdEnabled = await TgAuth.TgTokenChecker(isHiddenAuth);
        }

        public async Task TgChatIdChecker()
        {
            IsChatIdEnabled = await TgAuth.TgChatIdChecker(isHiddenAuth);
        }

        public async void SyncFiles()
        {
            
            //List<FilesSync> files = Settings.DataBase.Sync();
            //Settings.DataBase.AddNewFileToDB(files.ToArray());                                
            
        }
    }
}
