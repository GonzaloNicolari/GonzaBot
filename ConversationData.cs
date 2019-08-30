using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyEchoBot
{
    public class ConversationData
    {
        public string Tiemstamp { get; set; }
        public string ChannelId { get; set; }
        public bool PromptedUserForName { get; set; } = false;

    }
}
