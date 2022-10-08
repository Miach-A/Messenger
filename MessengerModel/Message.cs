using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerModel
{
    public class Message
    {
        public DateTime Date { get; set; }
        public Guid Guid { get; set; }
        public Chat Chat { get; set; } = null!;
        public Guid ChatGuid { get; set; }
        public User User { get; set; } = null!;
        public Guid UserGuid { get; set; }
        public string Text { get; set; } = string.Empty;
        public ICollection<MessageComment>? MessageComment { get; set; }
    }
}
