
namespace IdProvider{

public class UserIdProvider : IUserIdProvider
{
    public string GetUserId(ClaimsPrincipal principal)
    {
        // Assuming the user ID is stored in a claim named "sub"
        return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
}
