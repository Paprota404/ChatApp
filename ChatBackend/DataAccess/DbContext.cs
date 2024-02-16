using Microsoft.EntityFrameworkCore;
using SignUp.Models;
using Requests.Models;
using Friends.Models;


namespace Chat.Database{
public class AppDbContext : DbContext
{
    public DbSet<SignUpModel> users {get; set;}
    public DbSet<FriendRequestModel> friend_requests {get; set;}
    public DbSet<FriendsModel> friends {get;set;}
     public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    
   

     
}

}