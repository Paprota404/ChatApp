using Requests.Models;
using Friends.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Chat.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Requests.Services{

 public interface IFriendRequestService
    {
        void SendFriendRequest(int senderId, string receiverUsername);
        void AcceptFriendRequests(int requestId, int receiverId);
        List<FriendRequestModel> GetFriendRequests(int userId);
    }

public class FriendRequestService : IFriendRequestService{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;       
     public FriendRequestService(AppDbContext dbContext,UserManager<IdentityUser> userManager){
        _dbContext = dbContext;
        _userManager = userManager;
     }

     public void SendFriendRequest(int senderId, string receiverUsername){

        int receiverId = int.Parse(_userManager.FindByNameAsync(receiverUsername).Result.Id);

        

        if(receiverId == null){
            throw new Exception("User was not found");
        }

        if(senderId==receiverId){
            throw new Exception("Can't send friend request to yourself");
        }
        
        if(_dbContext.friend_requests.Any(r => r.request_sender_id == senderId && r.request_receiver_id == receiverId)){
            var errorMessage = "Friend request already sent.";
            throw new Exception(errorMessage);
        }

        var friendRequest = new FriendRequestModel{
            request_sender_id = senderId,
            request_receiver_id = receiverId,
            status = FriendRequestStatus.Pending,
            created_at = DateTime.UtcNow
        };

        _dbContext.friend_requests.Add(friendRequest);
        _dbContext.SaveChanges();
    }

     public void AcceptFriendRequests(int requestId, int receiverId){
        var friendRequest = _dbContext.friend_requests.Find(requestId);

        if(friendRequest.request_receiver_id!=receiverId){
            return;
        }

        if (friendRequest != null)
        {
        //Accepting friend request
        //Adding eachother to friends

        friendRequest.status = FriendRequestStatus.Accepted;

        var newFriendship = new FriendsModel{
            user1_id = friendRequest.request_sender_id,
            user2_id = friendRequest.request_receiver_id ?? 0
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
}