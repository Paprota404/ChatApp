using SignUp.Models;
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
        public async Task<IActionResult> SignUp(SignUpModel model){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            
            var existingUser = await _userManager.FindByNameAsync(model.username);

            if(existingUser != null){
                return Conflict(new {Message="A user with this email already exists."});
            }

            var user = new IdentityUser { UserName = model.username };
            var result = await _userManager.CreateAsync(user, model.password);

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