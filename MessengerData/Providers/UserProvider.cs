using MessengerData.Repository;
using MessengerModel.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


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

        public async Task<SaveUserResult> CreateUserAsync(NewUserDTO newUserDTO)
        {

            var hasher = new PasswordHasher<User>();
            var user = new User
            {
                Name = newUserDTO.Name,
                FirstName = newUserDTO.FirstName,
                LastName = newUserDTO.LastName,
                PhoneNumber = newUserDTO.PhoneNumber
            };
            user.PasswordHash = hasher.HashPassword(user, newUserDTO.Password);
            var entry = await _repository.AddAsync(user);

            try
            {
                await _repository.SaveAsync();
                return new SaveUserResult
                {
                    Result = true,
                    User = entry.Entity
                };
            }
            catch (Exception ex)
            {
                var fff = 1;
                return new SaveUserResult
                {
                    Result = false
                };
            }
            

            //return entry.Entity;
        }

    }

    public class SaveUserResult
    {
        public bool Result { get; set; }
        public User? User { get; set; } 
    }
}
