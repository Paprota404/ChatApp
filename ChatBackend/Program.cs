using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Chat.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Requests.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AppDbContext")), ServiceLifetime.Scoped);

// Add services to the container.
builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod();

        builder.AllowCredentials();
    });
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "DirectMe";
        options.Cookie.HttpOnly = true; // Make the cookie HttpOnly for added security
        options.Cookie.SameSite = SameSiteMode.None; // Adjust as needed
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; 
        options.Cookie.MaxAge = TimeSpan.FromHours(1); // Set the expiration time of the cookie
        options.LoginPath = "/Login"; 
        options.ExpireTimeSpan = TimeSpan.FromDays(30);

    });

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
   
builder.Services.AddScoped<IFriendRequestService, FriendRequestService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}


app.UseCors("MyCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();





app.Run();

