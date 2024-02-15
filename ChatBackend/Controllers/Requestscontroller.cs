using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Requests.Models;
using Requests.Services;

namespace Requests.Controllers{
   
    [Authorize] // Add this attribute if authentication is required
    [Route("api/[controller]")]
    public class FriendRequestController : Controller
    {
        private readonly IFriendRequestService _friendRequestService;

        public FriendRequestController(IFriendRequestService friendRequestService)
        {
            _friendRequestService = friendRequestService;
        }

        [HttpPost("send")]
        public IActionResult SendFriendRequest([FromBody] FriendRequestModel requestModel)
        {
            try
            {   
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid friend request data");
                }

                // Get the currently authenticated user's ID (assuming it's stored in the claims)
               
                
                int senderId = GetAuthenticatedUserId();

                _friendRequestService.SendFriendRequest(senderId, requestModel.request_receiver_id);

                return Ok("Friend request sent successfully");
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "An error occurred while processing the request");
            }
        }

        [HttpPost("accept/{requestId}")]
        public IActionResult AcceptFriendRequest(int requestId)
        {
            try
            {
                int receiverId = GetAuthenticatedUserId();

                _friendRequestService.AcceptFriendRequests(receiverId, requestId);

                return Ok("Friend request accepted successfully");
            }
            catch (Exception ex)
            {
                
                 return  StatusCode(500, "An error occurred while processing the request");
            }
        }

        [HttpGet("pending")]
        public IActionResult GetPendingFriendRequests()
        {
            try
            {
                // Get the currently authenticated user's ID (assuming it's stored in the claims)
                int userId = GetAuthenticatedUserId();

                List<FriendRequestModel> pendingRequests = _friendRequestService.GetFriendRequests(userId);

                return Ok(pendingRequests);
            }
            catch (Exception ex)
            {
                
                return (500, "An error occurred while processing the request");
            }
        }

        // Helper method to get the currently authenticated user's ID 
       private int GetAuthenticatedUserId(){
                    var httpContext = new HttpContextAccessor();
                    var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst   (ClaimTypes.NameIdentifier);

                    if(userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId)){
                        return userId;
                    }

                   return 0;
                }
    }
}
