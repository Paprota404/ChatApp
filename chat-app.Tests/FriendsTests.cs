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

            var userId = 
        }
    }
}