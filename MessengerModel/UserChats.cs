using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerModel
{
    public class UserChats
    {
        public User User { get; set; } = null!;
        public Guid UserGuid { get; set; }
        public Chat Chat { get; set; } = null!;
        public Guid ChatGuid { get; set; }
    }
}
