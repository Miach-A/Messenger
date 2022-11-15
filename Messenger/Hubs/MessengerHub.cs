using Microsoft.AspNetCore.SignalR;
using MessengerData.Providers;
using MessengerData;
using MessengerModel.MessageModels;
using MessengerModel.UserModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MessengerModel.ChatModelds;

namespace Messenger.Hubs
{
    public class MessengerHub:Hub
    {
        private readonly MessageProvider _messageProvider;
        private readonly UserProvider _userProvider;
        private readonly ApplicationDbContext _context;    

        public MessengerHub(MessageProvider messengerProvider, ApplicationDbContext context, UserProvider userProvider)
        {
            _messageProvider = messengerProvider;
            _context = context;
            _userProvider = userProvider;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await UserRegistration();
        }

        public async override Task OnDisconnectedAsync(Exception? exception)
        {

            if (Context.UserIdentifier == null)
            {
                await base.OnDisconnectedAsync(exception);
                return;
            }

            await UserUnRegistration();
            await base.OnDisconnectedAsync(exception);
        }

        private async Task UserRegistration()
        {
            if (Context.UserIdentifier == null)
            {
                return;
            }

            var groups = await _context.UserChats.Where(x => x.UserGuid == new Guid(Context.UserIdentifier)).Select(x => x.ChatGuid).ToArrayAsync();
            foreach (var groupGuid in groups)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, groupGuid.ToString());     
            }
        }

        private async Task UserUnRegistration()
        {
            if (Context.UserIdentifier == null)
            {
                return;
            }

            var groups = await _context.UserChats.Where(x => x.UserGuid == new Guid(Context.UserIdentifier)).Select(x => x.ChatGuid).ToArrayAsync();
            foreach (var groupGuid in groups)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupGuid.ToString());
            }
        }

        public async Task AddChat(CreateChatDTO createChatDTO)
        {
            if (Context.UserIdentifier == null)
            {
                return;
            }

            var receiver = await _context.Users.FirstOrDefaultAsync(x => x.Name == createChatDTO.ContactName);
            if (receiver == null)
            {
                return;
            }

            var result = await _userProvider.AddChat(new Guid(Context.UserIdentifier), createChatDTO.ContactName);
            if (result)
            {
                var chatDTO = _userProvider.ToChatDTO(result.Entity);
                await Clients.Caller.SendAsync("ReceiveChat", chatDTO);
                await Clients.User(receiver.Guid.ToString()).SendAsync("ReceiveChat", chatDTO);
            }
        }

        public async Task NewChat(Guid chatGuid)
        {
            if (Context.UserIdentifier == null)
            {
                return;
            }

            var newChatUsers = await _context.UserChats
                .Where(x => x.ChatGuid == chatGuid)
                .Select(x => x)
                .ToArrayAsync();

            var tasks = new List<Task>();
            foreach (var item in newChatUsers)
            {
                tasks.Add(Clients.User(item.UserGuid.ToString()).SendAsync("ReceiveNewChat", chatGuid));
            }

            await Task.WhenAll();
        }

        public async Task SendMessage(CreateMessageDTO messageDTO)
        {
            if (Context.UserIdentifier == null
                || messageDTO.ChatGuid == Guid.Empty
                || Context.User == null)
            {
                return;
            }

            _userProvider.GetClaimValue(Context.User, ClaimTypes.Name,out var contactName);

            var result = await _messageProvider.CreateMessageAsync(messageDTO, new Guid(Context.UserIdentifier));
            if (result)
            {
                await Clients.Groups(messageDTO.ChatGuid.ToString() ?? "").SendAsync("ReceiveMessage", _messageProvider.ToMessageDTO(result.Entity, contactName));
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

        public async Task DeleteMessage(UpdateMessageDTO updateMessageDTO)
        {
            if (Context.UserIdentifier == null
               || updateMessageDTO.Guid == Guid.Empty
               || updateMessageDTO.Date == new DateTime(1,1,1))
            {
                return;
            }

            var result = await _messageProvider.DeleteMessageAsync(updateMessageDTO, new Guid(Context.UserIdentifier));
            if (result)
            {
                await Clients.Groups(updateMessageDTO.ChatGuid.ToString() ?? "").SendAsync("DeleteMessage", updateMessageDTO);
            }
        }

        public async Task DeleteMessageForMe(UpdateMessageDTO messageDTO)
        {
            if (Context.UserIdentifier == null
               || messageDTO.Guid == Guid.Empty
               || messageDTO.Date == new DateTime(1, 1, 1))
            {
                return;
            }

            var result = await _messageProvider.DeleteMessageForMeAsync(messageDTO, new Guid(Context.UserIdentifier));
            if (result)
            {
                await Clients.Groups(messageDTO.ChatGuid.ToString() ?? "").SendAsync("DeleteMessageForMe", messageDTO);
            }
        }
        
    }
}
