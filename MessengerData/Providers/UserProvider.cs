using MessengerData.Repository;
using MessengerModel.UserModels;
using Microsoft.AspNetCore.Identity;


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
            user.UpdateUser(newUserDTO);
            user.PasswordHash = hasher.HashPassword(user, newUserDTO.Password);
            var entry = await _repository.AddAsync(user);

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

        public async Task<UpdateResult<User>> UpdateUser(NewUserDTO newUserDTO)
        {
            var user = await _repository.FirstOrDefaultAsync(x => x.Name == newUserDTO.Name,null,false);
            var result = new UpdateResult<User> { Result = false };
            if (user == null)
            {
                result.ErrorMessage.Add("User not found");
                return result;
            }
            user.UpdateUser(newUserDTO);

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

    }

}
