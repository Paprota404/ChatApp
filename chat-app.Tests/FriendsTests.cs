using Friends.Controllers;
using Friends.Services;
using Friends.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Http;
using UserDetails.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Chat.Database;
using Microsoft.AspNetCore.Identity;

namespace Friends.Tests
{
  // Assuming you are using xUnit for testing
 public class FriendsServiceTests
{
    [Fact]
    public async Task GetFriends_ReturnsCorrectUserDetails()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var dbContext = new AppDbContext(options))
        {
            // Add test data to the in-memory database
            dbContext.friends.AddRange(
                new FriendsModel { user1_id = "user1", user2_id = "user2" },
                new FriendsModel { user1_id = "user1", user2_id = "user3" }
            );
            dbContext.SaveChanges();
        

        var userManager = UserManager<IdentityUser>;
        var service = new FriendsService(dbContext, userManager);

        // Act
        var friends = await service.GetFriends("user1");

        // Assert
        Assert.Equal(2, friends.Count);
        Assert.Contains(friends, f => f.UserId == "user2" && f.Username == "user2_username");
        Assert.Contains(friends, f => f.UserId == "user3" && f.Username == "user3_username");
    }
    }
  
}
}