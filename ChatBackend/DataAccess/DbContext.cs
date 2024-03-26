using Microsoft.EntityFrameworkCore;
using Requests.Models;
using Friends.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Messages.Models;

namespace Chat.Database{
public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<FriendRequestModel> friend_requests {get; set;}
    public DbSet<FriendsModel> friends {get;set;}
    public DbSet<Message> messages {get;set;}
     public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
 
}

}