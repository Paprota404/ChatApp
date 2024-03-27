using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace IdProvider{

public class UserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        // Assuming the user ID is stored in a claim named "sub"
        var user = connection.User;
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
}
