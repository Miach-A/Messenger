using MessengerModel.ChatModelds;

namespace MessengerModel.UserModels
{
    public class UserDTO : UpdateUserDTO
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<ChatDTO> Chats { get; set; } = new List<ChatDTO>();
        public ICollection<ContactDTO> Contacts { get; set; } = new List<ContactDTO>();
    }
}
