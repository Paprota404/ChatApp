using Microsoft.AspNetCore.Mvc;

namespace Hello.Controllers{

    [Route("api/[controller]")]
    [ApiController]
    public class HelloController: ControllerBase{
        [HttpGet]
        public ActionResult<string> Hello(){
            return "Hello";
        }
    }
}