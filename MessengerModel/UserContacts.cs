using MessengerModel.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerModel
{
    public class UserContacts
    {
        public User User { get; set; } = null!;
        public Guid UserGuid { get; set; }
        public User Contact { get; set; } = null!;
        public Guid ContactGuid { get; set; }
        public string ContactName { get; set; } = null!;
    }
}
