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
        Task SendFriendRequest(string senderId, string receiverUsername);
        Task AcceptFriendRequests(int requestId, string receiverId);
        List<FriendRequestModel> GetFriendRequests(string userId);
    }

public class FriendRequestService : IFriendRequestService{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;       
     public FriendRequestService(AppDbContext dbContext,UserManager<IdentityUser> userManager){
        _dbContext = dbContext;
        _userManager = userManager;
     }

     public async Task SendFriendRequest(string senderId, string receiverUsername){
        
        IdentityUser receiver = await _userManager.FindByNameAsync(receiverUsername);

        if (receiver == null)
        {
            throw new Exception("User was not found");
        }

        if (senderId == receiver.Id)
        {
            throw new Exception("Can't send friend request to yourself");
        }

        if (_dbContext.friend_requests.Any(r => r.request_sender_id == senderId && r.request_receiver_id == receiver.Id))
        {
            var errorMessage = "Friend request already sent.";
            throw new Exception(errorMessage);
        }
        IdentityUser sender = await  _userManager.FindByIdAsync(senderId);

        var friendRequest = new FriendRequestModel
        {
            request_sender_id = senderId,
            request_receiver_id = receiver.Id,
            status = FriendRequestStatus.Pending,
            created_at = DateTime.UtcNow,
            sender_username = sender.UserName
        };

        _dbContext.friend_requests.Add(friendRequest);
        _dbContext.SaveChanges();

    }

     public async Task AcceptFriendRequests(int requestId, string receiverId){
        var friendRequest = await _dbContext.friend_requests.FindAsync(requestId);

        if(friendRequest.request_receiver_id!=receiverId){
            throw new Exception("Friend request is not intended for this user");
        }

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

    public List<FriendRequestModel> GetFriendRequests(string userId)
    {
        return _dbContext.friend_requests.Where(fr => fr.request_receiver_id == userId && fr.status == FriendRequestStatus.Pending).ToList();
    }
}
}