﻿using Microsoft.AspNetCore.SignalR;
using MessengerData.Providers;
using MessengerData;
using MessengerModel.MessageModels;
using MessengerModel.UserModels;
using Microsoft.EntityFrameworkCore;

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
                await Clients.User(receiver.Guid.ToString()).SendAsync("ReceiveChat", _userProvider.ToChatDTO(result.Entity));
            }
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

        public async Task DeleteMessage(UpdateMessageDTO updateMessageDTO)
        {
            if (Context.UserIdentifier == null
               || updateMessageDTO.Guid == Guid.Empty
               || updateMessageDTO.Date == new DateTime(1,1,1))
            {
                return;
            }

            var result = await _messageProvider.DeleteMessage(updateMessageDTO.Date, updateMessageDTO.Guid, new Guid(Context.UserIdentifier));

            if (result)
            {
                await Clients.Groups(updateMessageDTO.ChatGuid.ToString() ?? "").SendAsync("DeleteMessage", updateMessageDTO);
            }
        }
    }
}
