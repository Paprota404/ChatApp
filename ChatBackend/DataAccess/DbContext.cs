using Microsoft.EntityFrameworkCore;
using SignUp.Models;

namespace Chat.Database{
public class AppDbContext : DbContext{
    private readonly IConfiguration _configuration;

    public AppDbContext(IConfiguration configuration){
        _configuration = configuration;
    }
    public DbSet<SignUpModel> Users {get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PgDbConnection"));
    }
}

}