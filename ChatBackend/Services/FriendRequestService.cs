using Requests.Models;
using Friends.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Chat.Database;

namespace Requests.Services;

 public interface IFriendRequestService
    {
        void SendFriendRequest(int senderId, int receiverId);
        void AcceptFriendRequests(int requestId);
        List<FriendRequestModel> GetFriendRequests(int userId);
    }

public class FriendRequestService : IFriendRequestService{
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
            status = FriendRequestStatus.Pending,
            createdAt = DateTime.UtcNow
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

        var newFriendship = new FriendsModel{
            user1_id = friendRequest.request_sender_id,
            user2_id = friendRequest.request_receiver_id
        };
        _dbContext.friends.Add(newFriendship);
        _dbContext.SaveChanges();
    };
     }

    public List<FriendRequestModel> GetFriendRequests(int userId)
    {
        return _dbContext.friend_requests.Where(fr => fr.request_receiver_id == userId && fr.status == FriendRequestStatus.Pending).ToList();
    }
}