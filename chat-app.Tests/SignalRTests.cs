using Xunit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using ChatHubNamespace;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using AppTestFactory;

namespace SignalR.tests{

    public class SignalRIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory  _factory;

    public SignalRIntegrationTests(CustomWebApplicationFactory  factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task TestSendMessage()
    {
        // Arrange
        var client = _factory.CreateClient();
       
        var hubConnection = new HubConnectionBuilder()
            .WithUrl(client.BaseAddress + "ChatHub")
            .Build();

        var testClient = new TestClient();
        await testClient.ConnectAsync(client.BaseAddress + "ChatHub");

        // Act
        await hubConnection.StartAsync();
        await hubConnection.InvokeAsync("SendMessage", "Hello, World!");

        await Task.Delay(1000);

        // Assert
        Assert.True(testClient.MessageReceiver, "The message was not received by the test client.");
    }

    public class TestClient{
        public bool MessageReceiver {get; private set;} 

        public async Task ConnectAsync(string url){
            var connection = new HubConnectionBuilder().WithUrl(url).Build();

            connection.On<string>("ReceiveMessage", (message) =>
            {
                MessageReceiver = true;
            });

            await connection.StartAsync();
        }
    }
}
}