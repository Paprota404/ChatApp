using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Security.Claims;
using Chat.Database;
using Messages.Models;

namespace ChatHubNamespace{


public class ChatHub : Hub{
    private readonly AppDbContext _dbContext;

    public ChatHub(AppDbContext dbContext){
        _dbContext = dbContext;
    }

    public async Task SendMessage(string senderId, string receiverId,string message){
        await Clients.User(receiverId).SendAsync("ReceiveMessage",senderId,message);

         var newMessage = new Message
    {
        Sender = senderId,
        Receiver = receiverId,
        Content = message,
        SentAt = DateTime.UtcNow
    };
    }

    public override async Task OnConnectedAsync(){
         var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                // Assuming you want to add the user to a group named after their user ID
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception){
        var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId != null)
        {    
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }
        await base.OnDisconnectedAsync(exception);
    }
}
}