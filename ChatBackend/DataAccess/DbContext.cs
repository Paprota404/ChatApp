using Microsoft.EntityFrameworkCore;
using Requests.Models;
using Friends.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Messages.Models;

namespace Chat.Database
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<FriendRequestModel> friend_requests { get; set; }
        public DbSet<FriendsModel> friends { get; set; }
        public DbSet<Message> messages { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
               : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Important to call the base implementation

            // Configure the Message entity relationships
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }

    }

}