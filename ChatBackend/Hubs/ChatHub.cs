using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Security.Claims;
using Chat.Database;
using Messages.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatHubNamespace{


public class ChatHub : Hub{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;


    public ChatHub(AppDbContext dbContext,UserManager<IdentityUser> userManager){
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task SendMessage(string senderId, string receiverId,string message){
    var currentUserId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    // Check if the sender's ID matches the current user's ID
    if (senderId != currentUserId)
    {
        // If the IDs do not match, return early to prevent unauthorized message sending
        return;
    }
    
    var sender = await _userManager.FindByIdAsync(senderId);
    var receiver = await _userManager.FindByIdAsync(receiverId);

    if (sender == null || receiver == null)
    {
        // Handle the case where the sender or receiver is not found
        return;
    }

    await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message);

    // Create a new message object
    var newMessage = new Message
    {
        Sender = sender,
        Receiver = receiver,
        Content = message,
        SentAt = DateTime.UtcNow
    };

    // Add the message to the database
    _dbContext.messages.Add(newMessage);
    await _dbContext.SaveChangesAsync();
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