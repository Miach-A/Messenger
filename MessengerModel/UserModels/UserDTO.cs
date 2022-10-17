using MessengerModel.ChatModelds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerModel.UserModels
{
    public class UserDTO : UpdateUserDTO
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<ChatDTO> Chats { get; set; } = new List<ChatDTO>();
        public ICollection<ContactDTO> Contacts { get; set; } = new List<ContactDTO>();
    }
}
