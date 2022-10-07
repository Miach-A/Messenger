namespace MessengerModel
{
    public class User
    {
        public Guid Guid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public List<Chat>? Chats { get; set; }
        public List<Message>? Messages { get; set; }
    }
}