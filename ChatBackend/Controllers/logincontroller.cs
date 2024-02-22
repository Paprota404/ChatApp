using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using AuthDTO;
using Microsoft.Extensions.Configuration;

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

            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier,user.Id),
            new Claim(ClaimTypes.Name,user.UserName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["JWT:Issuer"],
            _configuration["JWT:Audience"],
            claims,
            expires: DateTime.Now.AddHours(2), // Set token expiration
            signingCredentials: creds);

            var tokenHandler = new JwtSecurityTokenHandler(); 
            var tokenString = tokenHandler.WriteToken(token);

           

            Response.Cookies.Append("JwtToken", tokenString, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(120) // Set your desired expiration time
            });

            return Ok();
        }

    }

    
}