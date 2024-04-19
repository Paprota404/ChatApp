using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Chat.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Requests.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Friends.Services;
using DirectMe.Authentication;
using ChatHubNamespace;
using Microsoft.AspNetCore.SignalR;
using Messages.Services;
using System.Data.SqlClient;

public partial class Program{

public static void Main(string[] args)
{

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5, // Maximum number of retries
                        maxRetryDelay: TimeSpan.FromSeconds(30), // Maximum delay between retries
                        errorNumbersToAdd: null); // Additional error numbers to retry on, if any
                }));

// Add services to the container

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder.WithOrigins("https://directme-eta.vercel.app/")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
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

builder.Services.AddAuthentication(options =>
    {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});



builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
   
builder.Services.AddScoped<IFriendRequestService, FriendRequestService>();
builder.Services.AddScoped<IFriendsService, FriendsService>();
builder.Services.AddScoped<MessageService>();

builder.Logging.AddConsole();
builder.Logging.AddDebug(); 


var app = builder.Build();



app.Use(async (context, next) =>
{
    if (context.Request.Cookies.TryGetValue("access_token", out var jwtToken))
    {
        context.Request.Headers.Append("Authorization", $"Bearer {jwtToken}");
    }

    await next.Invoke();
});
app.UseDefaultFiles();
app.UseStaticFiles(); 



app.UseRouting();

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


app.MapHub<ChatHub>("/ChatHub");


app.UseCors("MyCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();

}
}