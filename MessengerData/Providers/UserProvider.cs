using MessengerData.Repository;
using MessengerModel.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
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

        public async Task<CreateResult<User>> CreateUserAsync(NewUserDTO newUserDTO)
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
                var saveResult = await _repository.SaveAsync();
                return new CreateResult<User>{
                    Result = saveResult.Result,
                    Entity = entry.Entity,
                    ErrorMessage = saveResult.ErrorMessage
                };
            }
            catch
            {
                return new CreateResult<User>() { Result = false };
            }
            
        }

    }

}
