using System.Web.Http;
using System.Net.Http;
using SignUp.Models;
using Login.Controllers;

using BCrypt.Net;
using Chat.Database;
using Microsoft.EntityFrameworkCore;

namespace SignUp.Controllers
{
   [Route("[controller]")]
    public class SignUpController : ApiController{

        private readonly AppDbContext _context;

        public SignUpController(AppDbContext context){
            _context = context;
        }

        [HttpPost]
        public async Task<IHttpActionResult> SignUp(SignUpModel model){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            

            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var user = new SignUpModel { Email = model.Email, Password = model.Password }; 
            _context.Users.Add(user); 
            await _context.SaveChangesAsync();

            return Ok();

        }
    }
}