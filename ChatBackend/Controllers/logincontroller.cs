using System.Web.Http;
using Login.Models;
using BCrypt.Net;

namespace Login.Controllers
{
    [HttpPost]
    public class LoginController : ApiController{
        public IHttpActionResult Login(LoginModel model){
            var user =

            if(user==null){
                return NotFound();
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(model.Password,user.Password);

            if(!isValidPassword){
                return Unauthorized();
            }
        }
    }
}