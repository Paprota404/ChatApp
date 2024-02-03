using System.Web.Http;
using SignUp.Models;
using BCrypt.Net;

namespace SignUp.Controllers
{
    [HttpPost]
    public class SignUpController : ApiController{
        public async IHttpActionResult SignUp(SignUpModel model){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            

            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var user = new SignUpModel { Email = model.Email, Password = model.Password }; 
            _context.Users.Add(user); 
            await _context.SaveChangesAsync();

        }
    }
}