using Friends.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UserDetails.Models;
using System.Security.Claims;
using Microsoft.Extensions.Logging;


namespace Friends.Controllers{
    //Get users contacts
    //Look in friends table and retrieve second user id then go to users table and get user data

  
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
            var userIdClaim = httpContext?.User.FindFirst(ClaimTypes.NameIdentifier);


            return userIdClaim?.Value;
        }
    }
}