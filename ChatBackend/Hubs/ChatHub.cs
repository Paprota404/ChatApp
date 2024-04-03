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

namespace ChatHubNamespace
{

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ChatHub> _logger;


        public ChatHub(AppDbContext dbContext, UserManager<IdentityUser> userManager, ILogger<ChatHub> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _logger = logger;
        }



        public async Task SendMessage(string receiverId, string message)
        {
            var senderId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(senderId))
            {
                // Handle the case where the sender's ID is not found in the claims
                _logger.LogWarning("Sender's Id not found");
                return;
            }
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

            // Send the message to the receiver
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
            var currentUserId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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