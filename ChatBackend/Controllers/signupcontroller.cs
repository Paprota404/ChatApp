using SignUp.Models;
using Login.Controllers;
using BCrypt.Net;
using Chat.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace SignUp.Controllers
{
    [Route("api/signup")]
    public class SignUpController : Controller{
        
        private readonly AppDbContext _context;

        public SignUpController(AppDbContext context){
            _context = context;
        }
        
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel model){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            

            model.password = BCrypt.Net.BCrypt.HashPassword(model.password);

            var user = new SignUpModel { email = model.email, password = model.password }; 
            _context.users.Add(user); 
            await _context.SaveChangesAsync();

            return Ok();

        }
    }
}