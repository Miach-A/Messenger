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

        public async Task<SaveResult<User>> CreateUserAsync(NewUserDTO newUserDTO)
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
                return new SaveResult<User>
                {
                    Result = true,
                    Entity = entry.Entity
                };
            }
            catch (DbUpdateException exeption)
            {
                var result = new SaveResult<User>() { Result = false };
                if (exeption.InnerException is SqlException)
                {
                    if (((SqlException)exeption.InnerException).Number == 2601){
                        result.ErrorMessage.Add("Duplicate name");
                    }
                }
                return result;
            }
            catch
            {
                return new SaveResult<User>() { Result = false };
            }
            
        }

    }

}
