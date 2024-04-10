using Friends.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UserDetails.Models;
using System.Security.Claims;
using Microsoft.Extensions.Logging;


namespace Friends.Controllers
{
    //Get users contacts
    //Look in friends table and retrieve second user id then go to users table and get user data

    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class FriendsController : ControllerBase
    {

        private readonly IFriendsService _friendService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ILogger<FriendsController> _logger;

        public FriendsController(IFriendsService friendService, IHttpContextAccessor httpContextAccessor, ILogger<FriendsController> logger)
        {
            _friendService = friendService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpGet("GetFriends")]
        public async Task<IActionResult> GetFriends()
        {
            try
            {
                string authenticatedUserId = GetAuthenticatedUserId();

                List<UserDetailsModel> friendsInfo = await _friendService.GetFriends(authenticatedUserId);

                return Ok(friendsInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private string GetAuthenticatedUserId()
        {
            var context = _httpContextAccessor.HttpContext;
            var nameIdentifier = context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return nameIdentifier;
        }
    }
}