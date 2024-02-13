using Requests.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Requests.Data;

public class FriendRequestService{
     private readonly AppDbContext _dbContext;

     public FriendRequestService(AppDbContext dbContext){
        _dbContext = dbContext;
     }

     public void SendFriendRequest(int senderId, int receiverId){
        
        if(_dbContext.friend_requests.Any(r => r.request_sender_id == senderId && r.request_receiver_id == receiverId)){
            return;
        }

        var friendRequest = new FriendRequestModel{
            request_sender_id = senderId,
            request_receiver_id = receiverId,
            Status = FriendRequestStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.friend_requests.Add(friendRequest);
        _dbContext.SaveChanges();
     }

     public void AcceptFriendRequests(int requestId){
        var friendRequest = _dbContext.friend_requests.Find(requestId);

         if (friendRequest != null)
    {
        //Accepting friend request
        //Adding eachother to friends
        friendRequest.status = FriendRequestStatus.Accepted;
        _dbContext.SaveChanges();
    };
     }

    public List<FriendRequestModel> GetFriendRequests(int userId)
    {
        return _dbContext.friend_requests.Where(fr => fr.request_receiver_id == userId && fr.status == FriendRequestStatus.Pending).ToList();
    }
}