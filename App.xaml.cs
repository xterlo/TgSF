using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TgSF.Core;

namespace TgSF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            RegistryKey currentUserKey = Registry.CurrentUser;
            RegistryKey helloKey = currentUserKey.CreateSubKey("TgSF");

            if (helloKey.GetValue("token") is null) helloKey.SetValue("token", ""); 
                else Settings.TgToken = helloKey.GetValue("token").ToString();

            if (helloKey.GetValue("path") is null) helloKey.SetValue("path", "");
                else Settings.TgToken = helloKey.GetValue("token").ToString();

            Console.WriteLine(helloKey);
            base.OnStartup(e);
        }
    }
}
