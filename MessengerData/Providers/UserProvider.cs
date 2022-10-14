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
        private ApplicationDbContext _context;

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

            var saveResult = await SaveAsync();
            return new UpdateResult<User>(saveResult){ Entity = entry.Entity };
        }

        public async Task<SaveResult> ChangePasswordAsync(Guid userGuid, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Guid == userGuid);
            if (user == null)
            {
                return new UpdateResult<User>(false, "User provider. Change password. User not found."); //{ Result = false, ErrorMessage = new List<string> { "User provider. Change password. User not found." } };
            }

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, password);

            return await SaveAsync(); 
        }
        
        public async Task<UpdateResult<User>> UpdateUserAsync(Guid guid, UpdateUserDTO updateUserDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Guid == guid);
            var result = new UpdateResult<User> { Result = false };
            if (user == null)
            {
                result.ErrorMessage.Add("User not found");
                return new UpdateResult<User>(false, "User provider. Update user. User not found."); //{ Result = false, ErrorMessage = new List<string> { "User provider. Update user. User not found." } };
            }

            UpdateUserProperties(user, updateUserDTO);

            return new UpdateResult<User>(await SaveAsync()) { Entity = user };
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

        public UserDTO UpdateUserDTO (User user, UserDTO userDTO)
        {
            userDTO.Name = user.Name;
            userDTO.FirstName = user.FirstName;
            userDTO.LastName = user.LastName;
            userDTO.PhoneNumber = user.PhoneNumber;
            foreach (var chat in user.UserChats)
            {
                var chatDTO = new ChatDTO{ Guid = chat.ChatGuid, Name = chat.Chat.Name, Public = chat.Chat.Public };
                foreach (var item in chat.Chat.ChatUsers)
                {
                    chatDTO.Users.Add(ToContactDTO(item.User));
                }
                                
                userDTO.Chats.Add(chatDTO);
            }

            foreach (var contact in user.Contacts)
            {
                userDTO.Contacts.Add(new ContactDTO { Name = contact.Contact.Name, FirstName = contact.Contact.FirstName, LastName = contact.Contact.LastName, PhoneNumber = contact.Contact.PhoneNumber });
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
                return new SaveResult(false, "User provider. Add contact. User not found or contact is exist.");// { Result = false, ErrorMessage = new List<string> { "User provider. Add contact. User not found or contact is exist." } };
            }

            user.Contacts.Add(new UserContacts() { User = user, Contact = contact, ContactName = contactName });
    
            return await SaveAsync();
        }    

        public async Task<SaveResult> DeleteContact(Guid userGuid, string contactName)
        {
            var user = await _context.Users
                .Include(x => x.Contacts)
                    .ThenInclude(x => x.Contact)
                .FirstOrDefaultAsync(x => x.Guid == userGuid);
            if (user == null)
            {
                return new SaveResult(false, "User provider. Delete contact. User not found.");// { Result = false, ErrorMessage = new List<string> { "User provider. Delete contact. User not found." } };
            }

            var userContactsForDelete = user.Contacts.FirstOrDefault(x => x.Contact.Name == contactName);
            if (userContactsForDelete == null)
            {
                return new SaveResult(false, "User provider. Delete contact. Contact not found.");
            }

            user.Contacts.Remove(userContactsForDelete);
            return await SaveAsync();
        }
    
        public async Task<SaveResult> AddChat(Guid userGuid, string contactName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Guid == userGuid);
            var contact = await _context.Users.FirstOrDefaultAsync(x => x.Name == contactName);
            if (user == null || contact == null)
            {
                return new SaveResult(false,"User provider. Add chat. User or contact name not found"); //{ Result = false , ErrorMessage = new List<string>() { "User or contact name not found" } };
            }
            
            Chat chat = new Chat();
            chat.Name = string.Concat(user.Name,"-",contact.Name);
            chat.ChatUsers.Add(new UserChats { Chat = chat, User = user });
            chat.ChatUsers.Add(new UserChats { Chat = chat, User = contact });
            
            _context.Chats.Add(chat);

            return  new UpdateResult<Chat>(await SaveAsync()){ Entity = chat };
        }

        public async Task<SaveResult> SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return new SaveResult { Result = true };
            }
            catch (DbUpdateException exeption)
            {
                var result = new SaveResult();
                result.Result = false;
                if ((exeption.InnerException as SqlException)?.Number == 2601)
                {
                    result.ErrorMessage.Add("User provider. Save changes. Duplicate field");
                }

                if (result.ErrorMessage.Count() == 0)
                {
                    result.ErrorMessage.Add("User provider. Save changes. Unhandled db update exception");
                }

                return result;
            }
            catch
            {
                return new SaveResult(false, "User provider. Save changes. Unhandled exception.");
            }
        }
    }
}
