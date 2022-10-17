using MessengerData.Extensions;
using MessengerModel;
using MessengerModel.ChatModelds;
using MessengerModel.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;

namespace MessengerData.Providers
{
    public class UserProvider
    {
        private readonly ApplicationDbContext _context;

        public UserProvider(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public bool GetUserGuid(ClaimsPrincipal user, out Guid guid)
        {
            guid = Guid.Empty;
            string? userGuidString = user.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userGuidString == null)
            {
                return false;
            }

            try
            {
                guid = new Guid(userGuidString);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<UpdateResult<User>> CreateUserAsync(CreateUserDTO newUserDTO)
        {
            var hasher = new PasswordHasher<User>();
            var user = new User();

            UpdateUserProperties(user, newUserDTO);

            user.PasswordHash = hasher.HashPassword(user, newUserDTO.Password);
            EntityEntry<User> entry = await _context.Users.AddAsync(user);

            var saveResult = await _context.SaveAsync("User provider");
            return new UpdateResult<User>(entry.Entity, saveResult);
        }

        public async Task<SaveResult> ChangePasswordAsync(Guid userGuid, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Guid == userGuid);
            if (user == null)
            {
                return new UpdateResult<User>("User provider. Change password. User not found.");
            }

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, password);
            return await _context.SaveAsync("User provider"); 
        }
        
        public async Task<UpdateResult<User>> UpdateUserAsync(Guid guid, UpdateUserDTO updateUserDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Guid == guid);
            if (user == null)
            {
                return new UpdateResult<User>("User provider. Update user. User not found.");
            }

            UpdateUserProperties(user, updateUserDTO);
            return new UpdateResult<User>(user, await _context.SaveAsync("User provider"));
        }

        public User UpdateUserProperties(in User user, UpdateUserDTO newUserDTO)
        {
            user.Name = newUserDTO.Name;
            user.FirstName = newUserDTO.FirstName;
            user.LastName = newUserDTO.LastName;
            user.PhoneNumber = newUserDTO.PhoneNumber;

            return user;
        }

        public IEnumerable<UserDTO> ToUserDTO (IEnumerable<User> users)
        {
            List<UserDTO> result = new List<UserDTO>();
            foreach (var user in users)
            {
                result.Add(UpdateUserDTO(user, new UserDTO()));
            }

            return result;
        }

        public UserDTO ToUserDTO(User user)
        {
            return UpdateUserDTO(user, new UserDTO());
        }

        public IEnumerable<ContactDTO> ToContactDTO(IEnumerable<User> users)
        {
            List<ContactDTO> result = new List<ContactDTO>();
            foreach (var user in users)
            {
                result.Add(UpdateContactDTO(user, new ContactDTO()));
            }

            return result;
        }

        public ContactDTO ToContactDTO(User user)
        {
            return UpdateContactDTO(user, new ContactDTO());
        }

        public ChatDTO ToChatDTO(Chat chat)
        {
            return UpdateChatDTO(chat, new ChatDTO());
        }

        public ChatDTO UpdateChatDTO(Chat chat, ChatDTO chatDTO)
        {
            chatDTO.Guid = chat.Guid;
            chatDTO.Name = chat.Name;
            chatDTO.Public = chat.Public;

            foreach (var item in chat.ChatUsers)
            {
                chatDTO.Users.Add(ToContactDTO(item.User));
            }

            return chatDTO;
        }

        public UserDTO UpdateUserDTO (User user, UserDTO userDTO)
        {
            userDTO.Name = user.Name;
            userDTO.FirstName = user.FirstName;
            userDTO.LastName = user.LastName;
            userDTO.PhoneNumber = user.PhoneNumber;
            foreach (var chat in user.UserChats)
            {                             
                userDTO.Chats.Add(ToChatDTO(chat.Chat));
            }

            foreach (var contact in user.Contacts)
            {
                userDTO.Contacts.Add(ToContactDTO(contact.Contact));
            }

            return userDTO;
        }

        public ContactDTO UpdateContactDTO(User user, ContactDTO contactDTO)
        {
            contactDTO.Name = user.Name;
            contactDTO.FirstName = user.FirstName;
            contactDTO.LastName = user.LastName;
            contactDTO.PhoneNumber = user.PhoneNumber;

            return contactDTO;
        }
    
        public async Task<SaveResult> AddContact(Guid userGuid, string contactName)
        {
            var user = await _context.Users
                .Include(x => x.Contacts).ThenInclude(x => x.Contact)
                .FirstOrDefaultAsync(x => x.Guid == userGuid);

            var contact = await _context.Users.FirstOrDefaultAsync(x => x.Name == contactName);
            if (user == null 
                || contact == null
                || user.Contacts.Where(x => x.ContactGuid == contact.Guid).Count() > 0)
            {
                return new SaveResult("User provider. Add contact. User not found or contact is exist.");
            }

            user.Contacts.Add(new UserContacts() { User = user, Contact = contact, ContactName = contactName });
    
            return await _context.SaveAsync("User provider");
        }    

        public async Task<SaveResult> DeleteContact(Guid userGuid, string contactName)
        {
            var user = await _context.Users
                .Include(x => x.Contacts)
                    .ThenInclude(x => x.Contact)
                .FirstOrDefaultAsync(x => x.Guid == userGuid);
            if (user == null)
            {
                return new SaveResult("User provider. Delete contact. User not found.");
            }

            var userContactsForDelete = user.Contacts.FirstOrDefault(x => x.Contact.Name == contactName);
            if (userContactsForDelete == null)
            {
                return new SaveResult("User provider. Delete contact. Contact not found.");
            }

            user.Contacts.Remove(userContactsForDelete);
            return await _context.SaveAsync("User provider");
        }
    
        public async Task<UpdateResult<Chat>> AddChat(Guid userGuid, string contactName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Guid == userGuid);
            var contact = await _context.Users.FirstOrDefaultAsync(x => x.Name == contactName);
            if (user == null || contact == null)
            {
                return new UpdateResult<Chat>("User provider. Add chat. User or contact name not found");
            }
            
            Chat chat = new Chat();
            chat.Name = string.Concat(user.Name,"-",contact.Name);
            chat.ChatUsers.Add(new UserChats { Chat = chat, User = user });
            chat.ChatUsers.Add(new UserChats { Chat = chat, User = contact });
            
            _context.Chats.Add(chat);

            return  new UpdateResult<Chat>(chat, await _context.SaveAsync("User provider"));
        }
    }
}
