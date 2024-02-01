using System.Web.Http;
using SignUp.Models;
using BCrypt.Net;

namespace SignUp.Controllers
{
    [HttpPost]
    public class SignUpController : ApiController{
        public IHttpActionResult SignUp(SignUpModel model){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            

            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
        }
    }
}