using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTrmdBot.AbsCommands
{
    public interface ICommandService
    {
        List<TelegramCommand> Get();
    }
}
