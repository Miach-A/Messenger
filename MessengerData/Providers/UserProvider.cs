using MessengerData.Repository;
using MessengerModel.UserModels;
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

        public async Task<User> CreateUserAsync(NewUserDTO newUser)
        {
            return new User();
        }

    }
}
