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
            string jwt = GetJwtTokenFromContext();
            _logger.LogInformation(jwt);

            var senderId = ExtractNameIdentifierFromJwt(jwt);
            _logger.LogInformation("Method invoked");

            // Retrieve the sender and receiver users
            var sender = await _userManager.FindByIdAsync(senderId);
            var receiver = await _userManager.FindByIdAsync(receiverId);

            if (sender == null || receiver == null)
            {
                // Handle the case where the sender or receiver is not found
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
                SenderId = senderId, // Assuming senderId is a string, adjust as necessary
                ReceiverId = receiverId,
                Content = message,
                SentAt = DateTime.UtcNow
            };

            await Clients.Group(groupName).SendAsync("ReceiveMessage", newMessageDTO);

            // Create a new message object


            // Add the message to the database
            _dbContext.messages.Add(newMessage);
            await _dbContext.SaveChangesAsync();
        }

        public async Task GetLatestMessages(string receiverId)
        {
            string jwt = GetJwtTokenFromContext();

            var senderId = ExtractNameIdentifierFromJwt(jwt);

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
            string jwt = GetJwtTokenFromContext();
            _logger.LogInformation(jwt);

            _logger.LogInformation("A client connected to the hub.");

            var currentUserId = ExtractNameIdentifierFromJwt(jwt);
            _logger.LogInformation(currentUserId);

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
            string jwt = GetJwtTokenFromContext();
            _logger.LogInformation(jwt);

            _logger.LogInformation("A client disconnected from the hub.");

            var currentUserId = ExtractNameIdentifierFromJwt(jwt);
            _logger.LogInformation(currentUserId);

            if (currentUserId == null)
            {
                // Handle the case where the current user's ID is not found
                return;
            }

            var groupName = GenerateGroupName(currentUserId, otherUserId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        private string ExtractNameIdentifierFromJwt(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtTokenSansSignature = handler.ReadJwtToken(jwtToken);

            var nameIdentifierClaim = jwtTokenSansSignature.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            return nameIdentifierClaim?.Value;
        }

        private string GetJwtTokenFromContext()
        {
            _logger.LogInformation("called");
            var httpContext = Context.GetHttpContext();
            if (httpContext != null)
            {
                // Attempt to retrieve the access_token query parameter
                if (httpContext.Request.Query.TryGetValue("access_token", out var tokenValues))
                {
                    var token = tokenValues.FirstOrDefault();
                    if (!string.IsNullOrEmpty(token))
                    {
                        _logger.LogInformation($"Token from query string: {token}");
                        return token;
                    }
                }
            }

            return null;
        }
    }
}