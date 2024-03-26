using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

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
        var userId = _userIdProvider.GetUserId(Context.User);
        await Groups.AddToGroupAsync(Context.Connectionid,userId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception){
        var userId = _userIdProvider.GetUserId(Context.User);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        await base.OnDisconnectedAsync(exception);
    }
}
}