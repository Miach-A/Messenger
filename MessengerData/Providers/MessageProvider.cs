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

        public async Task<UpdateResult<Message>> CreateMessageAsync(CreateMessageDTO createMessageDTO, ClaimsPrincipal user)
        {
            if (!GetUserGuid(user, out var userGuid))
            {
                return new UpdateResult<Message>("User not found");
            }

            return await CreateMessageAsync(createMessageDTO, userGuid);
        }

        public async Task<UpdateResult<Message>> CreateMessageAsync(CreateMessageDTO createMessageDTO, Guid userGuid)
        {          
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

        public async Task<UpdateResult<Message>> UpdateMessageAsync(UpdateMessageDTO updateMessageDTO, ClaimsPrincipal user)
        {
            if (!GetUserGuid(user, out var userGuid))
            {
                return new UpdateResult<Message>("User not found");
            }

            return await UpdateMessageAsync(updateMessageDTO, userGuid);
        }

        public async Task<UpdateResult<Message>> UpdateMessageAsync(UpdateMessageDTO updateMessageDTO, Guid userGuid)
        {
            var message = _context.Messages
                .FirstOrDefault(x => x.Date == updateMessageDTO.Date 
                                    && x.Guid == updateMessageDTO.Guid 
                                    && x.UserGuid == userGuid);
            if (message == null)
            {
                return new UpdateResult<Message>("Message not found");
            }

            message.Text = updateMessageDTO.Text;
            var saveResult = await SaveAsync("Message provider. ");
            return new UpdateResult<Message>(message, saveResult);
        }

        public IQueryable<Message> GetDeletedMessages(Guid chatGuid, Guid userGuid, DateTime? date = null)
        {
            return _context.DeletedMessage
                //.Include(x => x.Message)
                .Where(x => (date == null ? true : x.Date < date) 
                    && x.ChatGuid == chatGuid 
                    && x.UserGuid == userGuid)
                .Select(x => x.Message);
        }
        public void UpdateMessageProperties(Message message, CreateMessageDTO createMessageDTO)
        {
            message.ChatGuid = createMessageDTO.ChatGuid;
            message.Text = createMessageDTO.Text;
        }

        public MessageDTO UpdateMessageDTO(Message message, MessageDTO messageDTO, string contactName = "")
        {
            messageDTO.Text = message.Text;
            messageDTO.Date = message.Date;
            messageDTO.Guid = message.Guid;
            messageDTO.ChatGuid = message.ChatGuid;
            if (message.User != null)
            {
                messageDTO.ContactName = message.User.Name;
            }
            else
            {
                messageDTO.ContactName = contactName;
            }
            
            if (message.CommentedMessage != null)
            {
                messageDTO.CommentedMessage = ToMessageDTO(message.CommentedMessage.CommentedMessage);
            }
            
            return messageDTO;
        }

        public MessageDTO ToMessageDTO(Message message, string contactName = "")
        {
            return UpdateMessageDTO(message, new MessageDTO(), contactName);
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

        public async Task<SaveResult> DeleteMessageAsync(UpdateMessageDTO updateMessageDTO, Guid userGuid)
        {
            var message = await _context.Messages
                .Include(x => x.CommentedMessage)
                .FirstOrDefaultAsync(x => x.Date == updateMessageDTO.Date && x.Guid == updateMessageDTO.Guid && x.UserGuid == userGuid);
            if (message == null)
            {
                return new SaveResult("Message not found");
            }

            _context.Remove(message);
            if (message.CommentedMessage != null)
            {
                _context.Remove(message.CommentedMessage);
            }
            return await SaveAsync("Message provider. ");
        }

        public async Task<SaveResult> DeleteMessageForMeAsync(UpdateMessageDTO messageDTO, Guid userGuid)
        { 
            var message = await _context.Messages
                .FirstOrDefaultAsync(x => x.Date == messageDTO.Date && x.Guid == messageDTO.Guid && x.UserGuid == userGuid);
            if (message == null)
            {
                return new SaveResult("Message not found");
            }

            var deletedMessage = new DeletedMessage();
            deletedMessage.Date = messageDTO.Date;
            deletedMessage.MessageGuid  = messageDTO.Guid; 
            deletedMessage.UserGuid = userGuid;
            deletedMessage.ChatGuid = messageDTO.ChatGuid;
            message.DeletedMessages.Add(deletedMessage);
            return await SaveAsync("Message provider. ");
        }


    }
}
