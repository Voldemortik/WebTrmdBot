using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TegBotTrmd.IRepository;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WebTrmdBot.AbsCommands
{
    public abstract class TelegramCommand
    {
        public abstract string Name { get; set; }

        public abstract Task Execute(Message message, ITelegramBotClient client,IOrderRepository orderRepo,IUserRepository userRepo,IProductRepository prodRepo);
        public abstract bool Contains(Message message);
    }
}
