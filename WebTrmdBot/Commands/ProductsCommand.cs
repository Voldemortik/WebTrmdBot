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
    public class ProductsCommand : TelegramCommand
    {

        public override string Name { get; set; } = "\U0001F45C Products";

        public override bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }

        public override async Task Execute(Message message, ITelegramBotClient botClient, IOrderRepository orderRepo, IUserRepository userRepo, IProductRepository prodRepo)
        {
            Console.WriteLine("StartProdCommand");

            var chatId = message.Chat.Id;
            var user = userRepo.GetUserByChatId(chatId);

            if (user == null)
            {
                await botClient.SendTextMessageAsync(chatId, "Юзера не существует. Пожалуйста сначала введите команду /start",
                      parseMode: ParseMode.Html, null, false, false, 0, null);
            }

            var products = prodRepo.GetProductsList().Where(x=>!x.OrderId.HasValue).ToList();

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

            InlineKeyboardButton inlineKeyboardButton = InlineKeyboardButton.WithCallbackData("Add to Cart", "addToCart");

            var inlineKeyBoard = new InlineKeyboardMarkup(
                new InlineKeyboardButton[][]
                {
                    new [] {
                        inlineKeyboardButton
                    },
                });
            if (products.Count()==0)
            {
                await botClient.SendTextMessageAsync(chatId, $"No products here right now!",
                     parseMode: ParseMode.Html, null, false, false, 0, null, null);
            }

            foreach (var product in products)
            {
                Console.WriteLine($"{product.Name}");
                // send response to incoming message
                await botClient.SendTextMessageAsync(chatId, $"{product.Name}: {product.Price}",
                parseMode: ParseMode.Html, null, false, false, 0, null, inlineKeyBoard);
            }
            Console.WriteLine("EndProdCommand");
        }
    }
}
