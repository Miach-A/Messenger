using MessengerModel.ChatModelds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerModel.MessageModels
{
    public class MessageDTO
    {
        public DateTime Date { get; set; }
        public Guid Guid { get; set; }
        public Guid ChatGuid { get; set; }
        public string ContactName { get; set; } = string.Empty;
        public MessageDTO? CommentedMessage { get; set; } = null;
        public string Text { get; set; } = string.Empty;
    }
}
