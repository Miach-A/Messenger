using Microsoft.AspNetCore.SignalR;
using MessengerData.Providers;
using MessengerData;
using MessengerModel.MessageModels;

namespace Messenger.Hubs
{
    public class MessengerHub:Hub
    {
        private readonly MessageProvider _messageProvider;
        private readonly ApplicationDbContext _context;

        public MessengerHub(MessageProvider messengerProvider, ApplicationDbContext context)
        {
            _messageProvider = messengerProvider;
            _context = context;
        }

        public async Task SendMessage(CreateMessageDTO messageDTO)
        {
            if (Context.UserIdentifier == null
                || messageDTO.ChatGuid == Guid.Empty)
            {
                return;
            }

            var result = await _messageProvider.CreateMessageAsync(messageDTO, new Guid(Context.UserIdentifier));
            if (result)
            {
                await Clients.Groups(messageDTO.ChatGuid.ToString() ?? "").SendAsync("ReceiveMessage", _messageProvider.ToMessageDTO(result.Entity));
            }
        }

        public async Task EditMessage(UpdateMessageDTO messageDTO)
        {
            if (Context.UserIdentifier == null
               || messageDTO.Guid == Guid.Empty)
            {
                return;
            }

            var result = await _messageProvider.UpdateMessageAsync(messageDTO, new Guid(Context.UserIdentifier));

            if (result)
            {
                await Clients.Groups(messageDTO.ChatGuid.ToString() ?? "").SendAsync("EditMessage", _messageProvider.ToMessageDTO(result.Entity));
            }
        }
    }
}
