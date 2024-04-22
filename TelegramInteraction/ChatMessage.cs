using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramInteraction
{
    public class ChatMessage
    {
        public long To { get; set; }

        public long From { get; set; }

        public string Text { get; set; }
    }
}
