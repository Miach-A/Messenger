using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerModel
{
    public class Message
    {
        public Guid Guid { get; set; }
        public DateTime Date { get; set; }
        public Chat Chat { get; set; } = null!;
        public Guid ChatGuid { get; set; }
        public User User { get; set; }
        public Guid UserGuid { get; set; }
        public string Text { get; set; } = String.Empty;
    }
}
