using Xunit;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using ChatHubNamespace;
using Microsoft.AspNetCore.Mvc.Testing;


namespace SignalR.tests{

    public class SignalRIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly WebApplicationFactory<Startup> _factory;

    public SignalRIntegrationTests(WebApplicationFactory<Startup> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task TestSendMessage()
    {
        // Arrange
        var client = _factory.CreateClient();
        var baseUrl = "http://localhost:5108"
        var hubConnection = new HubConnectionBuilder()
            .WithUrl(baseUrl + "ChatHub")
            .Build();

        var testClient = new TestClient();

        // Act
        await hubConnection.StartAsync();
        await hubConnection.InvokeAsync("SendMessage", "Hello, World!");

        // Assert
        // You would typically use a mock or a test client to verify the message was received
        // and processed as expected.
    }

    public class TestClient{
        
    }
}
}