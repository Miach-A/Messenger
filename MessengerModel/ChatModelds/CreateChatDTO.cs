namespace MessengerModel.UserModels
{
    public class CreateChatDTO
    {
        public string[] ContactName { get; set; } = new string[0];
        public bool Public { get; set; } = false;
        public string Name { get; set; } = "";
    }
}
