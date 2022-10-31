using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessengerModel.MessageModels;

namespace MessengerModel
{
    public class MessageComment
    {
        public DateTime MessageDate { get; set; }
        public Message Message { get; set; } = null!;
        public Guid MessageGuid { get; set; }
        public DateTime CommentedMessageDate { get; set; }
        public Message CommentedMessage { get; set; } = null!;
        public Guid CommentedMessageGuid { get; set; }
    }
}
