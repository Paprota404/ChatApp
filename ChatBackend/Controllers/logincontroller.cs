using Microsoft.AspNetCore.Mvc;
using Login.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Chat.Database;

namespace Login.Controllers
{
   
    [Route("api/login")]
    public class LoginController : Controller{
        
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context){
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Login(LoginModel model){
           var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if(user==null){
                return NotFound();
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(model.Password,user.Password);

            if(!isValidPassword){
                return Unauthorized();
            }

            return Ok();
        }
    }
}