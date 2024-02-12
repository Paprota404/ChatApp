using Requests.Models;

public interface IFriendRequestService{
    void SendFriendRequest(int senderId, int receiverId);
    void AcceptFriendRequest(int requestId);
    List<FriendRequestModel> GetFriendRequests(int userId);
}

public class FriendRequestService : IFriendRequestService{
     private readonly AppDbContext _dbContext;

     public FriendRequestService(AppDbContext dbContext){
        _dbContext = dbContext;
     }

     public void SendFriendRequest(int senderId, int receiverId){
        var friendRequest = new FriendRequestModel{
            Sender = senderId;
            Receiver = receiverId,
            Status = FriendRequestStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.friend_requests.Add(friendRequest);
        _dbContext.SaveChanges();
     }

     public void AcceptFriendRequests(int requestId){
        var friendRequest = _dbContext.friend_requests.Find(id);

         if (friendRequest != null)
    {
        friendRequest.status = FriendRequestStatus.Accepted;
        _dbContext.SaveChanges();
    }
     }

      public List<FriendRequestModel> GetFriendRequests(int userId)
    {
        return _dbContext.friend_requests.Where(fr => fr.request_receiver_id == userId && fr.status == FriendRequestStatus.Pending).ToList();
    }
}