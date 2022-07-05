using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using ToDoBot.Clients;
using ToDoBot.Models;

namespace ToDoBot
{
    public class ToDoListBot
    {
        MusicClient musicClient = new();
        WeatherClient weatherClient = new();
        RandomFactClient randomFactClient = new();
        HolidayInfoClient holidayInfoClient = new();
        DataBaseClient dataBaseClient = new();
        public string oldMessageText;
        private string _country;
        private string _city;
        private string _task;
        TelegramBotClient botClient = new TelegramBotClient("5463803886:AAFouraIk-zvOIyh2VzOBrdeKkAPZk8L62Y");
        CancellationToken cancellationToken = new CancellationToken();
        ReceiverOptions receiverOptions = new ReceiverOptions { AllowedUpdates = { } };
        public async Task Start()
        {
            botClient.StartReceiving(HandlerUpdateAsync, HandlerError, receiverOptions, cancellationToken);
            var botMe = await botClient.GetMeAsync();
            Console.WriteLine($"Bot started working");

        }

        private Task HandlerError(ITelegramBotClient botClient, Exception exception, CancellationToken token)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Error in telegram api\n {apiRequestException.ErrorCode}" +
                $"\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private async Task HandlerUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message.Text != null)
            {
                await HandlerMessageAsync(botClient, update.Message);
            }
            if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery.Data != null)
            {
                await HandlerCallbackAsync(botClient, update.CallbackQuery);
            }
        }

        private async Task HandlerCallbackAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            if (callbackQuery.Data.EndsWith("delete") == true)
            {
                await DeleteToDo(callbackQuery.Data.Substring(0, callbackQuery.Data.Length - 6), callbackQuery.Message.Chat.Id );
                return;
            }
        }

        private async Task HandlerMessageAsync(ITelegramBotClient botClient, Message message)
        {

            if (message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Welcome to ToDoBot. To continue press /settings");
                return;
            }
            else if (message.Text == "/settings")
            {
                oldMessageText = message.Text;
                await botClient.SendTextMessageAsync(message.Chat.Id, "Firstly, let's figure out some details. Enter /setcountry");
                return;
            }
            else if (message.Text == "/setcountry")
            {
                oldMessageText = message.Text;
                InlineKeyboardMarkup inlineKeyboardMarkup = new
                    (
                    InlineKeyboardButton.WithUrl("To learn about your country's code press this link", "https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements")
                    );
                await botClient.SendTextMessageAsync(message.Chat.Id, "Please, enter the country. I need this information so I can tell you about holidays in your country." +
                    "\nATTENTION PLEASE\n" +
                    "Use iso-3166 format for your country so I can find info correctly.", replyMarkup: inlineKeyboardMarkup);
                return;
            }
            else if (message.Text == "/setcity")
            {
                oldMessageText = message.Text;
                await botClient.SendTextMessageAsync(message.Chat.Id, "Now enter city you live, so I cam give you weather forecast.");
                return;

            }
            else if (message.Text == "/keyboard" || message.Text == "Return")
            {
                oldMessageText = message.Text;
                ReplyKeyboardMarkup replyKeyboardMapkup = new
                   (
                   new[]
                       {
                            new KeyboardButton[] { "See all tasks", "Add new task" },
                            new KeyboardButton[] {"Statistics of day", "Delete task" }
                       }
                   )
                {
                    ResizeKeyboard = true
                };
                await botClient.SendTextMessageAsync(message.Chat.Id, "Choose any option", replyMarkup: replyKeyboardMapkup);
                return;
            }
            else if (message.Text == "Add new task")
            {
                oldMessageText = message.Text;
                await botClient.SendTextMessageAsync(message.Chat.Id, "Write down your task");
                return;

            }
            else if (message.Text == "See all tasks")
            {
                var tasks = GetAllToDoS(message.Chat.Id).Result;
                InlineKeyboardMarkup inlineKeyboardMarkup = tasks.ConvertAll(task => new[] { InlineKeyboardButton.WithCallbackData(task.task.s, task.task.s) }).ToArray();
                await botClient.SendTextMessageAsync(message.Chat.Id, "Your tasks", replyMarkup: inlineKeyboardMarkup);
                return;
            }
            else if (message.Text == "Delete task")
            {
                
                var tasks = GetAllToDoS(message.Chat.Id).Result;
                InlineKeyboardMarkup inlineKeyboardMarkup = tasks.ConvertAll(task => new[] { InlineKeyboardButton.WithCallbackData(task.task.s, task.task.s+ "delete") }).ToArray();
                await botClient.SendTextMessageAsync(message.Chat.Id, "Please choose task you want to delete.", replyMarkup : inlineKeyboardMarkup);
            
                return;

            }
            else if (message.Text == "Statistics of day")
            {

                oldMessageText = message.Text;
                ReplyKeyboardMarkup replyKeyboardMapkup = new
                 (
                     new[]
                     {
                            new KeyboardButton[] { "Weather forecast", "Holiday today" },
                            new KeyboardButton[] {"Song of day", "Fact of day" },
                            new KeyboardButton[] {"Return"}
                     }
                 )
                {
                    ResizeKeyboard = true
                };
                await botClient.SendTextMessageAsync(message.Chat.Id, "Please, choose whatever you want.", replyMarkup: replyKeyboardMapkup);
                return;

            }
            else if(message.Text == "/myinfo")
            {
                _city = dataBaseClient.GetInfo(message.Chat.Id.ToString()).Result.City;
                _country = dataBaseClient.GetInfo(message.Chat.Id.ToString()).Result.Country;
                await botClient.SendTextMessageAsync(message.Chat.Id, $"Your country: {_country}\nYour city: {_city}\nIf you want to change info press /settings");
                return;

            }
            else if (message.Text == "Song of day")
            {
                var result = GetMusicUri().Result.Results[0];
                if (result != null)
                {
                    var title = GetMusic().Result.tracks.track[0];
                    string name = title.name;
                    string artist = title.artist.name;
                    InlineKeyboardMarkup inlineKeyboardMarkup = new
                        (
                        InlineKeyboardButton.WithUrl("Listen", result.Url)
                        );
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"The track \"{name}\" from {artist} is first in charts today.\n" +
                        $"Click the link to listen to it!", replyMarkup: inlineKeyboardMarkup);
                    return;
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Unexpected error");
                    return;
                }
            }
            else if (message.Text == "Weather forecast")
            {
                _city = dataBaseClient.GetInfo(message.Chat.Id.ToString()).Result.City;
                var result = GetWeather(_city);
                if (result.Result != null)
                {
                    double temp = result.Result.List[0].Main.Temp;
                    double pres = result.Result.List[0].Main.Pressure;
                    double hum = result.Result.List[0].Main.Humidity;
                    string weather = result.Result.List[0].Weather[0].Description;
                    string sunrise = result.Result.City.Sunrise;
                    string sunset = result.Result.City.Sunset;
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Weather forecast for today in {_city}:" +
                        $"\nWeather: {weather}" +
                        $"\nTemperature: {temp};" +
                        $"\nPressure: {pres};" +
                        $"\nHumidity: {hum}" +
                        $"\nSunrise time: {sunrise}" +
                        $"\nSunset time: {sunset}");
                    return;
                }
                else await botClient.SendTextMessageAsync(message.Chat.Id, "I am sorry, but it seems that i can't find weather for your city. Pick up bigger one or doublecheck the correctness of current city using /myinfo");
                return;
            }
            else if (message.Text == "Holiday today")
            {
                _country = dataBaseClient.GetInfo(message.Chat.Id.ToString()).Result.Country;
                var result = GetHolidayInfo(message);
                string s = " ";
                if (result.Result.Response.Holidays.Count == 0 || result.Result == null)
                {
                    s = "I am really sorry, but I have no info about any holiday in your country for today.\nOr maybe you gave me the name of country in wrong format. To doublecheck enter /myinfo";
                }
                for (int i = 0; i < result.Result.Response.Holidays.Count; i++)
                {
                    s += $"The name of holiday is: {result.Result.Response.Holidays[i].Name}.\n\nDescription: {result.Result.Response.Holidays[i].Description}\n";
                }
                await botClient.SendTextMessageAsync(message.Chat.Id, s);
                return;
            }
            else if (message.Text == "Fact of day")
            {
                var result = GetRandomFact().Result.Text;
                if (result != null)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, result);
                    return;
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Unexpected error");
                    return;
                }
            }
            switch (oldMessageText)
            {
                case "/setcountry":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Now enter /setcity");
                    _country = message.Text;
                    break;

                case "/setcity":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Thank you so much. Everything is ready.\nEnter /keyboard to continue.");
                    _city = message.Text;
                    await PostInfo(message.Chat.Id, _country, _city);
                    break;

                case "Add new task":
                    _task = message.Text;
                    await PostTask( _task, message.Chat.Id);
                    await botClient.SendTextMessageAsync(message.Chat.Id, "The task was added.");
                    break;
            }  

        }
        public async Task<List<GetTasksModel>> GetAllToDoS(long message)
        {
            var result = await dataBaseClient.GetAllTasks(message.ToString());
            if (result == null)
                return null;
            else return result;
        }
        public async Task DeleteToDo(string task, long message)
        {
            await dataBaseClient.DeleteTodo(task, message.ToString());
            await botClient.SendTextMessageAsync(message, "Deleted");


        }
        public async Task PostTask(string task, long message)
        {
            await dataBaseClient.PostTodo(task, message.ToString());

        }
        public async Task PostInfo(long message, string country, string city)
        {
            var result = await dataBaseClient.GetInfo(message.ToString());
            if (result == null)
            { await dataBaseClient.PostInfo(message.ToString(), country, city); }
            else
            {
                await dataBaseClient.DeleteInfo(message.ToString());
                await dataBaseClient.PostInfo(message.ToString(), country, city);
            }
        }
        public async Task<WeatherModel> GetWeather(string city)
        {
            var result = await weatherClient.GetWeatherByCity(city);
            if (result == null)
                return null;
            else return result;
        }
        public async Task<HolidayInfo> GetHolidayInfo(Message message)
        {
            var result =  holidayInfoClient.GetHolidayInfoAsync(message.Date, _country).Result;
            if (result == null)
                return null;
            else return result;
        }
        public async Task<RandomFactModel> GetRandomFact()
        {
            var result = await randomFactClient.GetRandomFactAsync();
            if (result == null)
                return null;
            else return result;
        }
        public async Task<MusicModel> GetMusicUri()
        {
            var result = await musicClient.GetFirstChartSongUri();
            if (result == null)
                return null;
            else return result;
        }
        public async Task<SongModel> GetMusic()
        {
            var result = await musicClient.GetFirstChartSong();
            if (result == null)
                return null;
            else return result;
        }
    }
}
