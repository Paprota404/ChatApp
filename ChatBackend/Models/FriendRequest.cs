
namespace Requests.Models;

public class RequestModel{
    public int id {get; set;}

    public int sender {get;set;}

    public int receiver {get;set;}

    public FriendRequestStatus Status {get;set;} = FriendRequestStatus.Pending;

    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
}

public enum FriendRequestStatus{
    Pending,Accepted,Rejected
}