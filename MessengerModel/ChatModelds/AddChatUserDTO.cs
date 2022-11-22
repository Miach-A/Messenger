namespace MessengerModel.UserModels
{
    public class AddChatUserDTO
    {
        public Guid guid { get; set; }
        public string[] ContactName { get; set; } = new string[0];   
    }
}
