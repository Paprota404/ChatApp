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

    private AppDbContext _dbContext;
    private UserManager<IdentityUser> _userManager;

    public FriendsServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new AppDbContext(options);

        var userStoreMock = new Mock<IUserStore<IdentityUser>>();
        var userManagerMock = new Mock<UserManager<IdentityUser>>(
        userStoreMock.Object, null, null, null, null, null, null, null, null);

    // Setup the UserManager mock to return specific users when FindByIdAsync is called
        userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
        .ReturnsAsync((string userId) => new IdentityUser { Id = userId, UserName = $"{userId}_username" });

        _userManager = userManagerMock.Object;

    }

    [Fact]
    public async Task GetFriends_ReturnsCorrectFriends()
    {
        // Arrange: Add test data to the in-memory database
        _dbContext.friends.Add(new FriendsModel { user1_id = "user1", user2_id = "user2" });
        _dbContext.friends.Add(new FriendsModel { user1_id= "user1", user2_id = "user3" });
        _dbContext.SaveChanges();

    // Mock the UserManager to return specific users
    var user1 = new IdentityUser { Id = "user2", UserName = "user2_username" };
    var user2 = new IdentityUser { Id = "user3", UserName = "user3_username" };
   

    // Act: Call the method under test
    var service = new FriendsService(_dbContext, _userManager);
    var result = await service.GetFriends("user1");

    // Assert: Verify the results
    Assert.Equal(2, result.Count);
    Assert.Contains(result, f => f.UserId == "user2" && f.Username == "user2_username");
    Assert.Contains(result, f => f.UserId == "user3" && f.Username == "user3_username");
    }
}
}
