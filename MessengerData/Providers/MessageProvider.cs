using MessengerData.Extensions;
using MessengerModel;
using MessengerModel.MessageModels;

namespace MessengerData.Providers
{
    public class MessageProvider
    {
        private readonly ApplicationDbContext _context;
        public MessageProvider(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SaveResult> CreateMessageAsync(CreateMessageDTO createMessageDTO)
        {
            Message message = new Message();

            UpdateMessageProperties(message, createMessageDTO);

            if (createMessageDTO.CommentedMessageGuid != null)
            {
                MessageComment messageComment = new MessageComment();
                messageComment.CommentedMessageGuid = (Guid)createMessageDTO.CommentedMessageGuid;
                messageComment.Message = message;
            }

            _context.Messages.Add(message);
            var saveResult =  await _context.SaveAsync("MessageProvider");
            return new UpdateResult<Message>(message, saveResult);
        }

        public void UpdateMessageProperties(Message message, CreateMessageDTO createMessageDTO)
        {
            message.Date = createMessageDTO.Date;
            message.UserGuid = createMessageDTO.UserGuid;
            message.ChatGuid = createMessageDTO.ChatGuid;
            message.Text = createMessageDTO.Text;
        }

        public MessageDTO UpdateMessageDTO(MessageDTO messageDTO, Message message)
        {
            messageDTO.Text = message.Text;
            messageDTO.Date = message.Date;
            messageDTO.Guid = message.Guid;

        }

    }
}
