using sampleserver.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace sampleserver.Models
{
    public class TelecommandData
    {
        public TelecommandSender sender;
        public TelecommandData()
        {
            sender = new TelecommandSender();
        }
        public async Task SendTelecommand(string command)
        {
            await sender.SendTelecommandAsync(command);
        }
    }
}
