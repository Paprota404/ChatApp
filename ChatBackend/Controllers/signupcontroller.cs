using AuthDTO;
using Login.Controllers;
using BCrypt.Net;
using Chat.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Friends.Models;
using Messages.Models;

namespace SignUp.Controllers
{
    [Route("api/signup")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public SignUpController(UserManager<IdentityUser> userManager,AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext
;        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] AuthDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userManager.FindByNameAsync(model.UserName);

            if (existingUser != null)
            {
                return Conflict(new { Message = "A user with this username already exists." });
            }

            var user = new IdentityUser { UserName = model.UserName };
            var result = await _userManager.CreateAsync(user, model.Password);

            await AddDefaultFriendAndMessage(user.Id);

        
            if (result.Succeeded)
            {
                
                return Ok();
            }
            else
            {

                return BadRequest(result.Errors);
            }
        }

        private async Task AddDefaultFriendAndMessage(string userId){
                var friendId = "a25c1bb7-07af-42ff-bb6f-94c3eedb7097";

                var sender = await _userManager.FindByIdAsync(friendId);
                var user = await _userManager.FindByIdAsync(userId);


                var newFriendship = new FriendsModel
                {
                    user1_id = user.Id,
                    user2_id = friendId
                };

                var newMessage = new Message{

                    Sender = sender,
                    Receiver = user,
                    Content = "Welcome",
                    SentAt = DateTime.UtcNow
                };
            
                
                _dbContext.friends.Add(newFriendship);
                _dbContext.messages.Add(newMessage);
                _dbContext.SaveChanges();
        }
    }
}