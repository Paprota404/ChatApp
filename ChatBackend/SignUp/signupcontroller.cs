using System.Web.Http;
using SignUp.Models;

namespace SignUp.Controllers
{
    [HttpPost]
    public class SignUpController : ApiController{
        piblic IHttpActionResult SignUp(SignUpModel model){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
        }
    }
}