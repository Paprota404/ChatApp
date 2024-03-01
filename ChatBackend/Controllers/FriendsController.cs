

namespace Friends.Controllers{
    //Get users contacts
    //Look in friends table and retrieve second user id then go to users table and get user data

    [Authorize]
    [Route("api/GetFriends")]
    [ApiController]

    public class FriendsController : ControllerBase{

        private readonly IHttpContextAccessor _httpContextAccessor;
    }
}