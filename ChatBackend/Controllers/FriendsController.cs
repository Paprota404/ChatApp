using Friends.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UserDetails.Models;
using System.Security.Claims;
using Microsoft.Extensions.Logging;


namespace Friends.Controllers{
    //Get users contacts
    //Look in friends table and retrieve second user id then go to users table and get user data

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class FriendsController : ControllerBase{

        private readonly IFriendsService _friendService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ILogger<FriendsController> _logger;

        public FriendsController(IFriendsService friendService, IHttpContextAccessor httpContextAccessor,ILogger<FriendsController> logger){
            _friendService = friendService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpGet("GetFriends")]
        public async Task<IActionResult> GetFriends(){
            try{
                var jwtToken = Request.Headers["Authorization"];

            // Log the received JWT token
                _logger.LogInformation($"Received JWT Token: {jwtToken}");

                string userId = GetAuthenticatedUserId();

                _logger.LogInformation($"Authenticated user ID: {userId}");

                List<UserDetailsModel> FriendsInfo =  await _friendService.GetFriends(userId);

                return Ok(FriendsInfo);
                
            } catch(Exception ex){
                return StatusCode(500, ex.Message);
            }
        }

        private string GetAuthenticatedUserId(){
            var httpContext = _httpContextAccessor.HttpContext;
            var authenticatedUser = httpContext.User.Identity.Name;
_logger.LogInformation($"Authenticated User: {authenticatedUser}");
            _logger.LogInformation($"Request Method: {httpContext?.Request.Method}");
            _logger.LogInformation($"Request Path: {httpContext?.Request.Path}");
            var userIdClaim = httpContext?.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            if (userIdClaim != null)
            {
                // Log specific claim information
                _logger.LogInformation($"Claim Type: {userIdClaim.Type}, Claim Value: {userIdClaim.Value}");
                
                // Or log the entire claim object as a string
                _logger.LogInformation($"UserIdClaim: {userIdClaim}");
                
                return userIdClaim.Value;
            }
    
    // Log if the claim is not found
            _logger.LogInformation("UserIdClaim not found");

            return null;
        }
    }
}