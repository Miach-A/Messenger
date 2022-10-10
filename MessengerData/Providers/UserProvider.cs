using MessengerData.Repository;
using MessengerModel;
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

        public void Test()
        {
            var tt = 1;
        }

        public async Task<IEnumerable<User>> GetUserByGuid(Guid guid, bool AsNoTraking)
        {
            var query = _context.Users.AsQueryable();
            
            if (AsNoTraking)
            {
                query.AsNoTracking();
            }

            return await query.Where(x => x.Guid == guid).Select(x => x).ToArrayAsync();
   
        }
    }
}
