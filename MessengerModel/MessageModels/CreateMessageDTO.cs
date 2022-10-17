namespace MessengerModel.MessageModels
{
    public class CreateMessageDTO : UpdateMessageDTO
    {
        public Guid UserGuid { get; set; }
    }
}
