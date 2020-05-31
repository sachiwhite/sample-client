using sampleserver.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace sampleserver.Models
{
    public class TelecommandData
    {
        public ITelecommandSender sender;
        public TelecommandData(ITelecommandSender sender)
        {
            this.sender = sender;
        }
        public async Task SendTelecommand(string command)
        {
            await sender.SendTelecommandAsync(command);
        }
    }
}
