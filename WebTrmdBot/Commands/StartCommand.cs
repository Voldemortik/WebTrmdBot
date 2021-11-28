using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TegBotTrmd.IRepository;
using TegBotTrmd.Repository;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WebTrmdBot.AbsCommands;

namespace WebTrmdBot.Commands
{
    public class StartCommand : TelegramCommand
    {
     
        public override string Name { get; set; } = "/start";

        public override bool Contains(Message message)
        {
            Console.WriteLine(message.Text);
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }

        public override async Task Execute(Message message, ITelegramBotClient botClient, IOrderRepository orderRepo, IUserRepository userRepo, IProductRepository prodRepo)
        {
            var chatId = message.Chat.Id;
            Console.WriteLine(chatId);

            var user = userRepo.GetUserByChatId(chatId);

            if(user == null)
            {
                var newUser = new TegBotTrmd.Entity.User()
                {
                    ChatId = chatId,
                    Balance = 0
                };
                userRepo.Create(newUser);
            }

            var keyboard = new[]
            {
                    new[]
                    {
                        new KeyboardButton("\U0001F451 Balance")
                    },
                    new []
                    {
                        new KeyboardButton("\U0001F45C Products")
                    },
                    new[]
                    {
                        new KeyboardButton("\U0001F3E0 ShoppingСart")
                    },
            };

            var keyBoard = new ReplyKeyboardMarkup(keyboard);
            await botClient.SendTextMessageAsync(chatId, "Привет!Добро пожаловать в КроссХауз!",
            parseMode: ParseMode.Html, null, false, false, 0, null, keyBoard);
        }
    }
}
