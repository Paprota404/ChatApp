using Microsoft.AspNetCore.Mvc;
using Login.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Chat.Database;

//if user already exist and password is incorrect return error

namespace Login.Controllers
{
   
    [Route("api/login")]
    public class LoginController : Controller{
        
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context){
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model){
           var user = await _context.users.FirstOrDefaultAsync(u => u.email == model.email);

            if(user==null){
                return NotFound();
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(model.password,user.password);

            if(!isValidPassword){
                return Unauthorized();
            }

            return Ok();
        }
    }
}