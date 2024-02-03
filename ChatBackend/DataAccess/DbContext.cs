public class AppDbContext : DbContext{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){
optionsBuilder.UseNpgsql(Configuration.GetConnectionString("PgDbConnection"));
    }

    public DbSet<SignUpModel> Users {get; set;}
}