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
    public class ShoppingCart : TelegramCommand
    {
     
        public override string Name { get; set; } = "\U0001F3E0 ShoppingСart";

     

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

            var unpaidOrders = orderRepo.GetOrderListByUser(user.Id).Where(x => !x.IsPaid).ToList();



            InlineKeyboardButton inlineKeyboardButtonDelete = InlineKeyboardButton.WithCallbackData("Удалить с заказа", "DeleteFromOrder");
            InlineKeyboardButton inlineKeyboardButtonPayOrder = InlineKeyboardButton.WithCallbackData("Оплатить заказ", "PayOrder");
            var inlineKeyBoard = new[]
            {
                new[]
                {
                    inlineKeyboardButtonDelete
                }
            };

            var inlineKeyBoardToPay = new[]
            {
                new[]
                {
                   inlineKeyboardButtonPayOrder
                }
            };
            if (unpaidOrders==null || unpaidOrders.Count == 0)
            {
                await botClient.SendTextMessageAsync(chatId, $"No Orders here right now",
                       parseMode: ParseMode.Html, null, false, false, 0, null, null);
            }

            foreach (var unpaidOrder in unpaidOrders)
            {
                Console.WriteLine($"orderId:{unpaidOrder.Id}");
                var orderProducts = unpaidOrder.Products.ToList();
                var sum = 0m;
                if (orderProducts!=null && orderProducts.Count>0)
                {
                    foreach (var item in orderProducts)
                    {
                        Console.WriteLine($"itemname:{item.Name}");
                        sum += item.Price;
                        // send response to incoming message
                        await botClient.SendTextMessageAsync(chatId, $"{unpaidOrder.Id}_{item.Name}: {item.Price}",
                        parseMode: ParseMode.Html, null, false, false, 0, null, new InlineKeyboardMarkup(inlineKeyBoard));
                    }

                    await botClient.SendTextMessageAsync(chatId, $"Сумма: {sum}",
                        parseMode: ParseMode.Html, null, false, false, 0, null, null);
                    await botClient.SendTextMessageAsync(chatId, $"Order Id: {unpaidOrder.Id}",
                       parseMode: ParseMode.Html, null, false, false, 0, null, new InlineKeyboardMarkup(inlineKeyBoardToPay));
                }
                else
                {
                    
                        await botClient.SendTextMessageAsync(chatId, $"No products in order here right now",
                               parseMode: ParseMode.Html, null, false, false, 0, null, null);
                    
                }
            }
            Console.WriteLine("EndProdCommand");
        }

    }
}
