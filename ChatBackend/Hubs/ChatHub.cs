using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Security.Claims;

namespace ChatHubNamespace{


public class ChatHub : Hub{
    public readonly IUserIdProvider _userIdProvider; 
    public async Task SendMessage(string senderId, string receiverId,string message){
        await Clients.User(receiverId).SendAsync("ReceiveMessage",senderId,message);
    }

    public ChatHub(IUserIdProvider userIdProvider){
        _userIdProvider = userIdProvider;
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