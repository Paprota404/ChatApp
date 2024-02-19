using System;
using Microsoft.AspNetCore.Mvc;
using Login.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Chat.Database;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;




namespace Login.Controllers
{
   
    [Route("api/login")]
    public class LoginController : Controller{
        
        private readonly AppDbContext _context;
        private readonly IOptionsSnapshot<JWT> _jwtSettings;

        public LoginController(AppDbContext context, IOptionsSnapshot<JWT> jwtSettings){
            _context = context;
            _jwtSettings = jwtSettings;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model){
           var user = await _context.users.FirstOrDefaultAsync(u => u.username == model.username);

            if(user==null){
                return NotFound();
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(model.password,user.password);

            if(!isValidPassword){
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Value.Key);
            
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = _jwtSettings.Value.Issuer,
                Audience = _jwtSettings.Value.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

           Response.Cookies.Append("jwtToken", jwtToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Only transmit the cookie over HTTPS
                SameSite = SameSiteMode.Lax // Prevent CSRF attacks
            });



            return Ok();
        }
    }
}