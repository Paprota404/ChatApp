using System.Web.Http;
using System.Net.Http;
using Login.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Chat.Database;

namespace Login.Controllers
{
   
    [Route("[controller]")]
    public class LoginController : ApiController{
        
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context){
            _context = context;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Login(LoginModel model){
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