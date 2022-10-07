using MessengerModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MessengerData
{
    public class ApplicationDbContext : DbContext
    {
        private IConfiguration _configuration;
        public ApplicationDbContext(IConfiguration configuration):base()
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SqlConnectionString"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Guid);
            modelBuilder.Entity<Message>().HasKey(x => new {x.Date, x.Guid});
            modelBuilder.Entity<DeletedMessage>().HasKey(x => x.Guid);
            modelBuilder.Entity<Chat>().HasKey(x => x.Guid);
            //modelBuilder.Entity<User>()
        }
    }
}