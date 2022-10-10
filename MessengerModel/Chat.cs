using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerModel
{
    public class Chat
    {
        public Guid Guid { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Public { get; set; } = false;
        public ICollection<UserChats> ChatUsers { get; set; } = null!;
        public ICollection<Message>? Messages { get; set; }
        public ICollection<DeletedMessage>? DeletedMessages { get; set; }
    }
}
