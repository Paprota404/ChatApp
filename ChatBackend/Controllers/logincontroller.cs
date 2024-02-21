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
using System.Text;




namespace Login.Controllers
{
   
    [Route("api/login")]
   
    public class LoginController : ControllerBase{
        
        private readonly AppDbContext _context;
        private IConfiguration _config;

        public LoginController(AppDbContext context, IConfiguration config){
            _context = context;
            _config = config;
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

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Sectoken = new JwtSecurityToken(_config["JWT:Issuer"], _config["JWT:Issuer"],
            null,
            expires:DateTime.Now.AddMinutes(180),
            signingCredentials:credentials);

            var token  = new JwtSecurityTokenHandler().WriteToken(Sectoken);

           Response.Cookies.Append("jwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Only transmit the cookie over HTTPS
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(3)
            });



            return Ok();
        }
    }
}