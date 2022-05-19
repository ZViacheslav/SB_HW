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
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

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

            string token = "5305716537:AAEXs1BGZFsO1f0yKAbr1rfIgLQPJkZWOl0";

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

            //bot.OnMessage += MessageListener;

            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();

            #endregion

        }

        #region NewMethods

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            //if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                var message = update.Message;
                if (message.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, добрый путник!");
                    return;
                }
                if (message.Text.ToLower() == "wago")
                {
                    FileSend("_Ваго.txt", "", update);
                }
            }

            if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
            {
                Console.WriteLine(new String('-', 50));
                Console.WriteLine($"     FileId: {update.Message.Document.FileId}");
                Console.WriteLine($"   FileName: {update.Message.Document.FileName}");
                Console.WriteLine($"   FileSize: {update.Message.Document.FileSize} байт");

                DownLoad(update.Message.Document.FileId, update.Message.Document.FileName);
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        #endregion

        #region OldMethods      
        //private static void MessageListener(object sender, MessageEventArgs e)
        //{
        //    string text = $"{DateTime.Now.ToLongTimeString()}: {e.Message.Chat.FirstName} {e.Message.Chat.Id} {e.Message.Text}";

        //    Console.WriteLine($"{text} TypeMessage: {e.Message.Type.ToString()}");


        //    if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
        //    {
        //        Console.WriteLine(e.Message.Document.FileId);
        //        Console.WriteLine(e.Message.Document.FileName);
        //        Console.WriteLine(e.Message.Document.FileSize);

        //        DownLoad(e.Message.Document.FileId, e.Message.Document.FileName);
        //    }

        //    if (e.Message.Text == null) return;

        //    var messageText = e.Message.Text;


        //    bot.SendTextMessageAsync(e.Message.Chat.Id,
        //        $"{messageText}"
        //        );
        //}

        static async void DownLoad(string fileId, string path)
        {
            var file = await bot.GetFileAsync(fileId);
            FileStream fs = new FileStream("_" + path, FileMode.Create);
            await bot.DownloadFileAsync(file.FilePath, fs);
            fs.Close();

            fs.Dispose();
        }

        static async void FileSend(string fileId, string path, Update update)
        {
            using (var stream = System.IO.File.OpenRead(path+fileId))
            {
                InputOnlineFile iof = new InputOnlineFile(stream); //оставляем также 
                iof.FileName = fileId;
                var send = await bot.SendDocumentAsync(update.Message.Chat.Id, iof, "Получи текстовой файл!");
            }
        }

        //public Task<Message> SendDocumentAsync(ChatId chatId, InputOnlineFile imageFile, string caption = "",
        //    bool disableNotification = false,
        //    int replyToMessageId = 0,
        //    IReplyMarkup replyMarkup = null,
        //    CancellationToken cancellationToken = default)
        //{
        //    var additionalParameters = new Dictionary<string, object>
        //    {
        //        {"caption", caption}
        //    };

        //    return SendMessageAsync(MessageType.DocumentMessage, chatId, document, disableNotification,
        //        replyToMessageId,
        //        replyMarkup, additionalParameters, cancellationToken);
        //}
        #endregion


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
