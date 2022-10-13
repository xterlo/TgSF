using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace TgSF.Core
{
    public class TgFunctions
    {
        public class TgAuth : TgFunctions
        {
            public static async Task<bool> TgTokenChecker(bool isHiddenAuth)
            {

                var tgBot = new TelegramBotClient(Settings.TgToken);
                try
                {
                    var tgBotAuth = await tgBot.GetMeAsync();
                    Settings.TGBot = tgBot;
                    if (!isHiddenAuth) MessageBox.Show($"Bot: {tgBotAuth.Username}", "Success");
                    return true;


                }
                catch (Telegram.Bot.Exceptions.ApiRequestException)
                {
                    MessageBox.Show("Wrong token", "Error");
                    return false;
                }

            }

            public static async Task<bool> TgChatIdChecker(bool isHiddenAuth)
            {
                var tgBot = Settings.TGBot;

                try
                {

                    var tgBotAuth = await tgBot.GetChatAsync(Settings.ChatId);
                    if (!isHiddenAuth) MessageBox.Show($"Chat with: {tgBotAuth.Username}", "Success");
                    return true;
                }
                catch (Telegram.Bot.Exceptions.ApiRequestException e)
                {
                    MessageBox.Show(e.Message, "Error");
                    return false;
                }
            }

        }

        public class TgChat : TgFunctions
        {
            public static async Task<Telegram.Bot.Types.Message> SendFile(FilesSync file)
            {
                var fileStream = new FileStream(file.FilePath,FileMode.Open);
                var fileToSend = new InputOnlineFile(fileStream,file.FileName);
                try
                {
                    var a = await Settings.TGBot.SendDocumentAsync(Settings.ChatId, fileToSend, caption: file.FilePath);
                    return a;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return null;
                }
            }

            public static async Task<bool> EditMessage(FilesSync file)
            {
                var fileStream = new FileStream(file.FilePath, FileMode.Open);
                var b = new InputMediaDocument(new InputMedia(fileStream, file.FileName));
                try
                {
                    await Settings.TGBot.EditMessageMediaAsync(file.TGMessageID.ToString(), b);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
