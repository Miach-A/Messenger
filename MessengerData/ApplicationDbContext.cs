using Microsoft.EntityFrameworkCore;

namespace MessengerData
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer
        }

    }
}