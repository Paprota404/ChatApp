using AuthDTO;
using Login.Controllers;
using BCrypt.Net;
using Chat.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace SignUp.Controllers
{
    [Route("api/signup")]
    [ApiController]
    public class SignUpController : ControllerBase{
        
        private readonly UserManager<IdentityUser> _userManager;

        public SignUpController(UserManager<IdentityUser> userManager){
            _userManager = userManager;
        }
        
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] AuthDto model){

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            
            var existingUser = await _userManager.FindByNameAsync(model.UserName);

            if(existingUser != null){
                return Conflict(new {Message="A user with this email already exists."});
            }

            var user = new IdentityUser {UserName = model.UserName};
            var result = await _userManager.CreateAsync(user,model.Password);

            if (result.Succeeded)
        {
            // User creation succeeded
            return Ok();
        }
            else
            {
                // User creation failed, return the errors
                return BadRequest(result.Errors);
            }
        }
    }
}