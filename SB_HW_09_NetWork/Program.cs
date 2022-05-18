using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Example_941
{
    class Program
    {
        static TelegramBotClient bot;

        static void Main(string[] args)
        {
            #region Задание
            // Создать бота, позволяющего принимать разные типы файлов, 
            // *Научить бота отправлять выбранный файл в ответ
            // 
            // https://data.mos.ru/
            // https://apidata.mos.ru/
            // 
            // https://vk.com/dev
            // https://vk.com/dev/manuals

            // https://dev.twitch.tv/
            // https://discordapp.com/developers/docs/intro
            // https://discordapp.com/developers/applications/
            // https://discordapp.com/verification

            //https://api.telegram.org/bot<token>/METHOD_NAME

            //string token = File.ReadAllText(@"E:\001_IT\3_C#\3_Профессия С# разработчик [Skillbox]_(2020)\boominfo.ru - 9. Работа с сетью\Исходники модуль 9\Theme_09\Example_931\Example_941\bin\Debug\token.txt");

            #endregion

            string token = File.ReadAllText(@"D:\token.txt");

            #region WebClient
            //WebClient wc = new WebClient() { Encoding = Encoding.UTF8 };
            //int update_id = 0;
            //string startUrl = $@"https://api.telegram.org/bot{token}/";

            //while (true)
            //{
            //    string url = $"{startUrl}getUpdates?offset={update_id}";
            //    var r = wc.DownloadString(url);

            //    //Console.WriteLine(r);
            //    //Console.WriteLine();

            //    var msgs = JObject.Parse(r)["result"].ToArray();

            //    foreach (dynamic msg in msgs)
            //    {
            //        update_id = Convert.ToInt32(msg.update_id) + 1;

            //        string userMessage = msg.message.text;
            //        string userId = msg.message.from.id;
            //        string useFirstrName = msg.message.from.first_name;
            //        string last_name = msg.message.from.last_name;
            //        string language_code = msg.message.from.language_code;

            //        string text = $"{useFirstrName} {last_name} {userId} {userMessage} {language_code} ";
            //        Console.WriteLine(text);

            //        if (userMessage == "hi")
            //        {
            //            string responseText = $"Здравствуйте, {useFirstrName}";
            //            url = $"{startUrl}sendMessage?chat_id={userId}&text={responseText}";
            //            Console.WriteLine("+");
            //            wc.DownloadString(url);
            //        }
            //    }

            //    Thread.Sleep(100);
            //}
            #endregion

            #region TelegramBot

            #region Proxy

            //// Содержит параметры HTTP-прокси для System.Net.WebRequest класса.
            //var proxy = new WebProxy()
            //{
            //    Address = new Uri($"http://203.30.190.121:80"),
            //    UseDefaultCredentials = false,
            //    //Credentials = new NetworkCredential(userName: "login", password: "password")
            //};

            //// Создает экземпляр класса System.Net.Http.HttpClientHandler.
            //var httpClientHandler = new HttpClientHandler() { Proxy = proxy };

            //// Предоставляет базовый класс для отправки HTTP-запросов и получения HTTP-ответов 
            //// от ресурса с заданным URI.
            //HttpClient hc = new HttpClient(httpClientHandler);



            #endregion

            bot = new TelegramBotClient(token);

            bot.OnMessage += MessageListener;

            #endregion

        }

        private static void MessageListener(object sender, MessageEventArgs e)
        {
            string text = $"{DateTime.Now.ToLongTimeString()}: {e.Message.Chat.FirstName} {e.Message.Chat.Id} {e.Message.Text}";

            Console.WriteLine($"{text} TypeMessage: {e.Message.Type.ToString()}");


            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
            {
                Console.WriteLine(e.Message.Document.FileId);
                Console.WriteLine(e.Message.Document.FileName);
                Console.WriteLine(e.Message.Document.FileSize);

                DownLoad(e.Message.Document.FileId, e.Message.Document.FileName);
            }

            if (e.Message.Text == null) return;

            var messageText = e.Message.Text;


            bot.SendTextMessageAsync(e.Message.Chat.Id,
                $"{messageText}"
                );
        }

        static async void DownLoad(string fileId, string path)
        {
            var file = await bot.GetFileAsync(fileId);
            FileStream fs = new FileStream("_" + path, FileMode.Create);
            await bot.DownloadFileAsync(file.FilePath, fs);
            fs.Close();

            fs.Dispose();
        }
    }
}

//{
//    "ok":true,"result":[
//        {
//        "update_id":64153304,
//        "message":
//            { 
//                "message_id":15,
//                "from":
//                { 
//                    "id":195095194,
//                    "is_bot":false,
//                    "first_name":"\u0412\u044f\u0447\u0435\u0441\u043b\u0430\u0432",
//                    "username":"ZVO2020",
//                    "language_code":"ru"
//                },
//                "chat":
//                { 
//                    "id":195095194,
//                    "first_name":"\u0412\u044f\u0447\u0435\u0441\u043b\u0430\u0432",
//                    "username":"ZVO2020",
//                    "type":
//                    "private"
//                },
//                "date":1652776423,
//                "text":"2"
//            }
//        }]
//}
