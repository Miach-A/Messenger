using MessengerModel;

namespace MessengerData.Providers
{
    public class UserProvider
    {
        private readonly ApplicationDbContext _context;
        public UserProvider(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUserByGuid(Guid guid)
        {
            var res = await _context.Users.Where(x => x.Guid == guid).Select(x => x).ToArrayAsync();
        }
    }
}
