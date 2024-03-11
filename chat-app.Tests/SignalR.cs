using Moq;
using Xunit;
using Microsoft.AspNetCore.SignalR;
using ChatHubNamespace;


namespace SignalR.tests{

    public class SignalRTests{

        [Fact]
        public async Task SendMessage_BroadcastsMessageToAll()
        {
            var mockClients = new Mock<IHubClients>();
            var mockGroups = new Mock<IGroupManager>();
            var mockContext = new Mock<HubCallerContext>();
            var mockHub = new Mock<Hub>();

            var hub = mockHub.Object;

    // Act
            await hub.Clients.All.SendAsync("ReceiveMessage", "TestUser", "Hello, World!");

            // Assert
            mockClients.Verify(c => c.All.SendAsync(It.IsAny<string>(),It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}