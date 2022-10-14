using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerModel.UserModels
{
    public class UserDTO : UpdateUserDTO
    {
        public ICollection<UserChats> UserChats { get; set; } = new List<UserChats>();
        public ICollection<ContactDTO> Contacts { get; set; } = new List<ContactDTO>(); //UserContacts
    }
}
