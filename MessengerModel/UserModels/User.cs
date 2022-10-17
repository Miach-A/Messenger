using MessengerModel.MessageModels;

namespace MessengerModel.UserModels
{
    public class User
    {
        public Guid Guid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public ICollection<UserChats> UserChats { get; set; } = new List<UserChats>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<UserContacts> Contacts { get; set; } = new List<UserContacts>();
        public ICollection<UserContacts> IAsContact { get; set; } = new List<UserContacts>();
        public ICollection<DeletedMessage> DeletedMessages { get; set; } = new List<DeletedMessage>();

    }
}