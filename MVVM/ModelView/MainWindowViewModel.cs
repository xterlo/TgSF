using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System.ComponentModel;
using System.Windows.Input;
using TgSF.Core;

namespace TgSF.MVVM.ModelView
{
    public class MainWindowViewModel : ObserverObject
    {
        public ICommand ChoosePathCommand { get => new RelayCommand(ChoosePath); }
        public ICommand SyncFilesCommand { get => new RelayCommand(SyncFiles); }
        

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

        public void SyncFiles()
        {

        }
    }
}
