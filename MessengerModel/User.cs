namespace MessengerModel
{
    public class User
    {
        public Guid Guid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}