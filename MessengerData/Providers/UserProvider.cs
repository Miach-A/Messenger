using MessengerModel;
using Microsoft.EntityFrameworkCore;

namespace MessengerData.Providers
{
    public class UserProvider
    {
        private readonly ApplicationDbContext _context;
        public UserProvider(ApplicationDbContext context)
        {
            _context = context;
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
