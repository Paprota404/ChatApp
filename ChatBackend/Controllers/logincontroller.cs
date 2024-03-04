using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using AuthDTO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

namespace Login.Controllers
{
   
    [Route("api/login")]
   
    public class LoginController : ControllerBase{
        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration; 
        public LoginController(UserManager<IdentityUser> userManager, IConfiguration configuration){
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] AuthDto model){
            

            //Check if user exits if exist check if password is correct
            var user = await _userManager.FindByNameAsync(model.UserName);

            if(user==null){
                return NotFound(new {Message="A user doesn't exist."});
            }

            var CorrectPassword = await _userManager.CheckPasswordAsync(user,model.Password);

            if(!CorrectPassword){
                return Unauthorized(new {Message="Invalid password"});
            }

            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var SecToken = new JwtSecurityToken(
                _configuration["JWT:Issuer"],
                _configuration["JWT:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials:credentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(SecToken);

            return Ok(new {token=token});
        }

    }

    
}