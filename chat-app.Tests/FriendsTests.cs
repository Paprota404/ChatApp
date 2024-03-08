using Friends.Controllers;
using Friends.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Friends.Tests
{
    public class FriendsControllerTests
    {
        [Fact]
        public async Task GetFriends_ReturnsOkResult_WithFriendsInfo()
        {
            var mockFriendService = new Mock<IFirendService>();
            var mockLogger = new Mock<ILogger<FriendsController>>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var userId = "testUserId";
            var friendsInfo = new List<UserDetailsModel> { new UserDetailsModel { Id = 1, Username = "Friend 1" } };

            mockFriendService.Setup(service => service.GetFriends(userId)).ReturnsAsync(friendsInfo);
            mockHttpContextAccessor.Setup(accessor => accessor.HttpContext.User.Identity.Us ername).Returns(userId);
            mockHttpContextAccessor.Setup(accessor => accessor.HttpContext.User.FindFirst(It.IsAny<string>())).Returns(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId));

            var controller = new FriendsController(mockFriendService.Object,mockHttpContextAccessor.Object,mockLogger.Object);

            //Act
            var result = await controller.GetFriends();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<UserDetailsModel>>(okResult);
            Assert.Equal(friendsInfo,model);
        }
    }
}