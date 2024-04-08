using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class isAuthenticatedController : ControllerBase
{


    [HttpGet("check")]
    public IActionResult CheckAuth()
    {

        return Ok(new { isAuthenticated = true });
    }

}