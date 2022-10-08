namespace MessengerModel
{
    public class User
    {
        public Guid Guid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public ICollection<UserChats>? UserChats { get; set; }
        public ICollection<Message>? Messages { get; set; }
        public ICollection<UserContacts>? Contacts { get; set; }
    }
}