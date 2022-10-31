using MessengerModel.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerModel.ChatModelds
{
    public class ChatDTO
    {
        public Guid Guid { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Public { get; set; } = false;
        public ICollection<ContactDTO> Users { get; set; } = new List<ContactDTO>();
    }
}
