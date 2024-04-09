using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Threading.Tasks;

namespace JwtFromCookie{


public class JwtCookieToHeaderMiddleware : IMiddleware
{

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Cookies.TryGetValue("access_token", out var jwtToken))
        {
            context.Request.Headers.Append("Authorization", $"Bearer {jwtToken}");
        }

        await next(context);
    }
}
}