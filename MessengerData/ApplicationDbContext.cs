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
            modelBuilder.Entity<UserChats>().HasKey(x => new { x.ChatGuid, x.UserGuid });
            modelBuilder.Entity<UserContacts>().HasKey(x => new { x.UserGuid, x.ContactGuid });

            //modelBuilder.Entity<User>().HasMany(x => x.Chats).WithMany(x => x.Users).UsingEntity<Dictionary<string, object>>("ChatUsers");
            //modelBuilder.Entity<User>().HasMany(x => x.Contacts).WithMany(x => x.Contacts).UsingEntity<Dictionary<string, object>>("UserContacts");
            //    ,x => x.HasOne<User>().WithMany().HasForeignKey("UserGuid")
            //    ,y => y.HasOne<Chat>().WithMany().HasForeignKey("ChatGuid"));

            modelBuilder.Entity<Message>().HasOne(x => x.User).WithMany(x => x.Messages).HasForeignKey(x => x.UserGuid).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeletedMessage>().HasOne(x => x.Message).WithMany().HasForeignKey(x => x.MessageGuid).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DeletedMessage>().HasOne(x => x.Chat).WithMany().HasForeignKey(x => x.ChatGuid).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<DeletedMessage>().HasOne(x => x.Chat).WithMany().HasForeignKey(x => x.ChatGuid).OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<UserChats>().HasOne(x => x.Chat).WithMany(x => x.ChatUsers).HasForeignKey(x => x.ChatGuid).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserChats>().HasOne(x => x.User).WithMany(x => x.UserChats).HasForeignKey(x => x.UserGuid).OnDelete(DeleteBehavior.ClientCascade);

        }
    }
}