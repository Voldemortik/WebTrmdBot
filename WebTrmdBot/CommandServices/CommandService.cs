using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTrmdBot.AbsCommands;
using WebTrmdBot.Commands;

namespace WebTrmdBot.CommandServices
{
    public class CommandService : ICommandService
    {
        private readonly List<TelegramCommand> _commands;

        public CommandService()
        {
            _commands = new List<TelegramCommand>
            {
                new BalanceCommand(),
                new ProductsCommand(),
                new ShoppingCart(),
                new StartCommand()
            };
        }

        public List<TelegramCommand> Get() => _commands;
    }
}
