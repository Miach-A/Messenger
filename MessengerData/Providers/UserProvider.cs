using MessengerData.Repository;
using MessengerModel.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MessengerData.Providers
{
    public class UserProvider
    {

        private readonly ApplicationDbContext _context;
        private IRepository<User> _repository;

        public UserProvider(ApplicationDbContext context, IRepository<User> repository)
        {

            _context = context;
            _repository = repository;

        }
        
        public IRepository<User> GetRepository()
        {

            return _repository;

        }

        public ApplicationDbContext GetDbContext()
        {

            return _context;

        }

        public async Task<UpdateResult<User>> CreateUserAsync(NewUserDTO newUserDTO)
        {

            var hasher = new PasswordHasher<User>();
            var user = new User();
            UpdateUserProperties(user, newUserDTO);
            user.PasswordHash = hasher.HashPassword(user, newUserDTO.Password);
            EntityEntry<User> entry = await _repository.AddAsync(user);

            try
            {
                var saveResult = await _repository.SaveAsync();
                return new UpdateResult<User>{
                    Result = saveResult.Result,
                    Entity = entry.Entity,
                    ErrorMessage = saveResult.ErrorMessage
                };
            }

            catch
            {
                return new UpdateResult<User>() { Result = false };
            }
            
        }

        public async Task<UpdateResult<User>> UpdateUserAsync(Guid guid, UpdateUserDTO updateUserDTO)
        {

            var user = await _repository.FirstOrDefaultAsync(x => x.Guid == guid, null,false);
            var result = new UpdateResult<User> { Result = false };
            if (user == null)
            {
                result.ErrorMessage.Add("User not found");
                return result;
            }

            UpdateUserProperties(user, updateUserDTO);

            try
            {
                var saveResult = await _repository.SaveAsync();
                return new UpdateResult<User>
                {
                    Result = saveResult.Result,
                    Entity = user,
                    ErrorMessage = saveResult.ErrorMessage
                };
            }
            catch
            {
                return new UpdateResult<User>() { Result = false };
            }

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
            userDTO.UserChats = user.UserChats;
            userDTO.Contacts = user.Contacts;

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
    
    }

}
