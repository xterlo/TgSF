using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Telegram.Bot;
using TgSF.Core;
using Xceed.Words.NET;

namespace TgSF.MVVM.ModelView
{
    public class MainWindowViewModel : ObserverObject
    {
        public ICommand ChoosePathCommand { get => new RelayCommand(ChoosePath); }
        public ICommand SyncFilesCommand { get => new RelayCommand(SyncFiles); }
        public ICommand TgTokenCheckerCommand { get => new RelayCommand(TgTokenChecker); }
        

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

        public void ChoosePath()
        {
            VistaFolderBrowserDialog DialogPath = new VistaFolderBrowserDialog();
            if (DialogPath.ShowDialog() == true)
            {
                SyncPath = DialogPath.SelectedPath;
            }
        }

        public async void TgTokenChecker()
        {
            var tgBot = new TelegramBotClient(TgToken);
            try
            {
                var tgBotAuth = await tgBot.GetMeAsync();
                MessageBox.Show($"Bot: {tgBotAuth.Username}", "Success");
                Settings.TGBot = tgBot;
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException)
            {
                MessageBox.Show("Неправильный токен", "Error");
            }

        }

        public void SyncFiles()
        {

            string extractPath = @"C:\Users\xterl\Documents\tgfs";
            string wordFile = Path.Combine(extractPath, "test1.docx");
            string folder = "word/media";
            using (ZipArchive archive = ZipFile.OpenRead(Path.Combine(extractPath, "test1.docx")))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.IndexOf(folder) == 0)
                    {
                        entry.ExtractToFile(Path.Combine(extractPath, entry.Name));
                    }
                }
            }
        }
    }
}
