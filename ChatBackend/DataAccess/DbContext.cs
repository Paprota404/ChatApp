using Microsoft.EntityFrameworkCore;
using SignUp.Models;

namespace Chat.Database{
public class AppDbContext : DbContext
{
     public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    
    public DbSet<SignUpModel> users {get; set;}

     
}

}