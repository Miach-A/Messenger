using MessengerData.Extensions;
using MessengerModel;
using MessengerModel.MessageModels;
using MessengerModel.UserModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MessengerData.Providers
{
    public class MessageProvider : DataProvider
    {
        public MessageProvider(ApplicationDbContext context) : base(context) { }

        public async Task<UpdateResult<Message>> CreateMessageAsync(CreateMessageDTO createMessageDTO, Guid userGuid) //ClaimsPrincipal user
        {          
            //if (!GetUserGuid(user, out var userGuid))
            //{
            //    return new UpdateResult<Message>("User not found");
            //}

            Message message = new Message();
            message.UserGuid = userGuid;
            message.Date = DateTime.Now;
            UpdateMessageProperties(message, createMessageDTO);

            if (createMessageDTO.CommentedMessageGuid != null 
                && createMessageDTO.CommentedMessageDate != null)
            {
                var commentedMessage = await _context.Messages.Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.Date == createMessageDTO.CommentedMessageDate 
                                            && x.Guid == createMessageDTO.CommentedMessageGuid);
                if (commentedMessage != null)
                {
                    MessageComment messageComment = new MessageComment();
                    messageComment.CommentedMessage = commentedMessage;
                    messageComment.Message = message;
                    message.CommentedMessage = messageComment;
                }
            }

            _context.Messages.Add(message);
            var saveResult =  await SaveAsync("Message provider.");
            return new UpdateResult<Message>(message, saveResult);
        }

        public async Task<UpdateResult<Message>> UpdateMessageAsync(UpdateMessageDTO updateMessageDTO, Guid userGuid) //ClaimsPrincipal user
        {
            //if (!GetUserGuid(user, out var userGuid))
            //{
            //    return new UpdateResult<Message>("User not found");
            //}

            var message = _context.Messages
                .FirstOrDefault(x => x.Date == updateMessageDTO.Date 
                                    && x.Guid == updateMessageDTO.Guid 
                                    && x.UserGuid == userGuid);
            if (message == null)
            {
                return new UpdateResult<Message>("Message not found");
            }

            message.Text = updateMessageDTO.Text;
            var saveResult = await SaveAsync("Message provider.");
            return new UpdateResult<Message>(message, saveResult);
        }

        public void UpdateMessageProperties(Message message, CreateMessageDTO createMessageDTO)
        {
            message.ChatGuid = createMessageDTO.ChatGuid;
            message.Text = createMessageDTO.Text;
        }

        public MessageDTO UpdateMessageDTO(Message message, MessageDTO messageDTO)
        {
            messageDTO.Text = message.Text;
            messageDTO.Date = message.Date;
            messageDTO.Guid = message.Guid;
            messageDTO.ChatGuid = message.ChatGuid;
            if (message.User != null)
            {
                messageDTO.ContactName = message.User.Name;
            }     
            if (message.CommentedMessage != null)
            {
                messageDTO.CommentedMessage = ToMessageDTO(message.CommentedMessage.CommentedMessage);
            }
            
            return messageDTO;
        }

        public MessageDTO ToMessageDTO(Message message)
        {
            return UpdateMessageDTO(message, new MessageDTO());
        }

        public IEnumerable<MessageDTO> ToMessageDTO(IEnumerable<Message> messages)
        {
            List<MessageDTO> result = new List<MessageDTO>();
            foreach (var message in messages)
            {
                result.Add(UpdateMessageDTO(message, new MessageDTO()));
            }

            return result;
        }

    }
}
