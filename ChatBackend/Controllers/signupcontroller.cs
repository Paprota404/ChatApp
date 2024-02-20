using SignUp.Models;
using Login.Controllers;
using BCrypt.Net;
using Chat.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace SignUp.Controllers
{
    [Route("api/signup")]
    [ApiController]
    public class SignUpController : ControllerBase{
        
        private readonly AppDbContext _context;

        public SignUpController(AppDbContext context){
            _context = context;
        }
        
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel model){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            
            bool userExists = await _context.users.AnyAsync(u => u.username == model.username);

            if(userExists){
                return Conflict(new {Message="A user with this email already exists."});
            }

            model.password = BCrypt.Net.BCrypt.HashPassword(model.password);

            var user = new SignUpModel { username = model.username, password = model.password }; 
            _context.users.Add(user); 
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}