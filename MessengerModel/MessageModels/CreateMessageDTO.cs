namespace MessengerModel.MessageModels
{
    public class CreateMessageDTO
    {   
        public Guid ChatGuid { get; set; }
        public Guid? CommentedMessageGuid { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
