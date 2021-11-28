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
    public class BalanceCommand : TelegramCommand
    {
        public override string Name { get; set; } = "\U0001F451 Balance";

        public override bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }

        public override async Task Execute(Message message, ITelegramBotClient botClient, IOrderRepository orderRepo, IUserRepository userRepo, IProductRepository prodRepo)
        {
            var chatId = message.Chat.Id;
            var user = userRepo.GetUserByChatId(chatId);

            if (user == null)
            {
                await botClient.SendTextMessageAsync(chatId, "Юзера не существует. Пожалуйста сначала введите команду /start",
                      parseMode: ParseMode.Html, null, false, false, 0, null);
            }

            InlineKeyboardButton inlineKeyboardButton = InlineKeyboardButton.WithCallbackData("Пополнить баланс", "AddBalance");

            var inlineKeyBoard = new InlineKeyboardMarkup(
                new InlineKeyboardButton[][]
                {
                    new [] {
                        inlineKeyboardButton
                    },
                });

           

            await botClient.SendTextMessageAsync(chatId, $"Balance:{user.Balance}",
            parseMode: ParseMode.Html, null, false, false, 0, null, inlineKeyBoard); 
        }

    }
}
