using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Security.Claims;
using Chat.Database;
using Messages.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Messages.Services;

namespace ChatHubNamespace
{

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ChatHub> _logger;
        private readonly MessageService _messageService;


        public ChatHub(AppDbContext dbContext, UserManager<IdentityUser> userManager, ILogger<ChatHub> logger, MessageService messageService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _logger = logger;
            _messageService = messageService;
        }



        public async Task SendMessage(string receiverId, string message)
        {
            var senderId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation("Method invoked");


            var sender = await _userManager.FindByIdAsync(senderId);
            var receiver = await _userManager.FindByIdAsync(receiverId);

            if (sender == null || receiver == null)
            {

                _logger.LogInformation("No sender or receiver in database");
                return;
            }


            var groupName = GenerateGroupName(senderId, receiverId);


            var newMessage = new Message
            {
                Sender = sender,
                Receiver = receiver,
                Content = message,
                SentAt = DateTime.UtcNow
            };

            var newMessageDTO = new MessageDTOs
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = message,
                SentAt = DateTime.UtcNow
            };

            await Clients.Group(groupName).SendAsync("ReceiveMessage", newMessageDTO);


            _dbContext.messages.Add(newMessage);
            await _dbContext.SaveChangesAsync();
        }

        public async Task GetLatestMessages(string receiverId)
        {
            var senderId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (senderId == null)
            {
                _logger.LogInformation("No sender ID found in the context.");
                return;
            }

            var messages = await _messageService.GetMessagesBetweenUsersAsync(senderId, receiverId);

            await Clients.Caller.SendAsync("ReceiveMessages", messages);
        }

        string GenerateGroupName(string userId1, string userId2)
        {
            var sortedUserIds = new List<string> { userId1, userId2 }.OrderBy(id => id).ToList();
            return string.Join("_", sortedUserIds);
        }

        public async Task StartOneToOneSession(string otherUserId)
        {
            var currentUserId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUserId == null)
            {
                return;
            }

            var groupName = GenerateGroupName(currentUserId, otherUserId);
            _logger.LogInformation(groupName);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task EndOneToOneSession(string otherUserId)
        {

            var currentUserId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUserId == null)
            {
                _logger.LogInformation("No current user ID found in the context.");
                return;
            }

            var groupName = GenerateGroupName(currentUserId, otherUserId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}