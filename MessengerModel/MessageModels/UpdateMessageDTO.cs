namespace MessengerModel.MessageModels
{
    public class UpdateMessageDTO
    {
        public DateTime Date { get; set; }
        public Guid Guid { get; set; }
        public Guid ChatGuid { get; set; }
        public Guid? CommentedMessageGuid { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
