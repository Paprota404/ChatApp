using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Requests.Models;
using Requests.Services;


namespace Requests.Controllers{
   
    
    [Route("api/[controller]")]
    public class FriendRequestController : Controller
    {
        private readonly IFriendRequestService _friendRequestService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public FriendRequestController(IFriendRequestService friendRequestService, IHttpContextAccessor httpContextAccessor)
        {
            _friendRequestService = friendRequestService;
            _httpContextAccessor = httpContextAccessor;
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
                //User enters username
                //Find userId by username
                

                _friendRequestService.SendFriendRequest(senderId, requestModel.receiver_username);

                return Ok("Friend request sent successfully");
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("accept/{requestId}")]
        public IActionResult AcceptFriendRequest(int requestId)
        {
            try
            {
                int receiverId = GetAuthenticatedUserId();

                _friendRequestService.AcceptFriendRequests(requestId);

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
                 return StatusCode(500, new { Number = 500, Error = "An error occurred while processing the request" });
            }
        }

        // Helper method to get the currently authenticated user's ID 
       private int GetAuthenticatedUserId(){
                    var httpContext = _httpContextAccessor.HttpContext;
                    var userIdClaim = httpContext?.User.FindFirst(ClaimTypes.Name);

                    if(userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId)){
                        return userId;
                    }

                   return 0;
                }
    }
}
