using MessengerModel;
using MessengerModel.ChatModelds;
using MessengerModel.MessageModels;
using MessengerModel.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MessengerData
{
    public class ApplicationDbContext : DbContext
    {
        private IConfiguration _configuration;
        public ApplicationDbContext(IConfiguration configuration):base()
        {
            _configuration = configuration;
        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<Chat> Chats { get; set; } = null!;
        public DbSet<UserChats> UserChats { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SqlConnectionString")).LogTo(Console.WriteLine, LogLevel.Information); ;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(x =>
            {
                x.HasKey(x => x.Guid);
                x.HasIndex(x => x.Name).IsUnique(true);
                x.Property(x => x.Name).IsUnicode(true).HasMaxLength(36);
            });

            modelBuilder.Entity<Message>(x =>
            {
                x.HasKey(x => new { x.Date, x.Guid });
                x.Property(x => x.Date).ValueGeneratedOnAdd();
                x.Property(x => x.Guid).HasDefaultValueSql("NEWID()");
                x.HasOne(x => x.User).WithMany(x => x.Messages).HasForeignKey(x => x.UserGuid).OnDelete(DeleteBehavior.Cascade);
                x.HasOne(x => x.Chat).WithMany(x => x.Messages).HasForeignKey(x => x.ChatGuid).OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<MessageComment>(x =>
            {
                x.HasKey(x => new { x.MessageDate, x.MessageGuid}); //, x.CommentedMessageDate, x.CommentedMessageGuid  //MessageDate //MessageGuid
                x.HasOne(x => x.CommentedMessage).WithMany().HasForeignKey(x => new { x.CommentedMessageDate, x.CommentedMessageGuid }).OnDelete(DeleteBehavior.ClientCascade); // if del mess dell all comment?
                //x.HasOne(x => x.CommentedMessage).WithMany(x => x.MessageComment).HasForeignKey(x => new { x.CommentedMessageDate, x.CommentedMessageGuid }).OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<DeletedMessage>(x =>
            {
                x.HasKey(x => new { x.Date, x.MessageGuid, x.ChatGuid, x.UserGuid });
                x.HasOne(x => x.Message).WithMany().HasForeignKey(x => new { x.Date, x.MessageGuid }).OnDelete(DeleteBehavior.Cascade);
                x.HasOne(x => x.Chat).WithMany(x => x.DeletedMessages).HasForeignKey(x => x.ChatGuid).OnDelete(DeleteBehavior.ClientCascade);
                x.HasOne(x => x.User).WithMany(x => x.DeletedMessages).HasForeignKey(x => x.UserGuid).OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<Chat>(x =>
            {
                x.HasKey(x => x.Guid);
            });

            modelBuilder.Entity<UserChats>(x =>
            {
                x.HasKey(x => new { x.ChatGuid, x.UserGuid });
                x.HasOne(x => x.Chat).WithMany(x => x.ChatUsers).HasForeignKey(x => x.ChatGuid).OnDelete(DeleteBehavior.Cascade);
                x.HasOne(x => x.User).WithMany(x => x.UserChats).HasForeignKey(x => x.UserGuid).OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<UserContacts>(x => 
            {
                x.HasKey(x => new { x.UserGuid, x.ContactGuid });
                x.HasOne(x => x.User).WithMany(x => x.Contacts).HasForeignKey(x => x.UserGuid).OnDelete(DeleteBehavior.ClientCascade);
                x.HasOne(x => x.Contact).WithMany(x => x.IAsContact).HasForeignKey(x => x.ContactGuid).OnDelete(DeleteBehavior.ClientCascade);
            });
        }
    }
}