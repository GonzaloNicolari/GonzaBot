using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyEchoBot.Entities
{
    public class BotReview
    {
        public bool isHelpFull { get; set; }
        public bool isEasyToDeal { get; set; }
        public int Punctuation { get; set; }
        public string ThingsToImprove { get; set; }
    }
}
