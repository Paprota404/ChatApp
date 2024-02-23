using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Requests.Models;
using Requests.Services;
using Microsoft.Extensions.Logging;


namespace Requests.Controllers{
   
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendRequestController : ControllerBase
    {
        private readonly IFriendRequestService _friendRequestService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<FriendRequestController> _logger;

        public FriendRequestController(IFriendRequestService friendRequestService, IHttpContextAccessor httpContextAccessor, ILogger<FriendRequestController> logger)
        {
            _friendRequestService = friendRequestService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpPost("send")]
        public IActionResult SendFriendRequest([FromBody] string receiver_username)
        {
            try
            {   
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid friend request data");
                }
   
                string senderId = GetAuthenticatedUserId();
                //User enters username
                //Find userId by username
                _logger.LogInformation($"Authenticated user ID: {senderId}");

                _friendRequestService.SendFriendRequest(senderId, receiver_username);

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
                string receiverId = GetAuthenticatedUserId();
        

                _friendRequestService.AcceptFriendRequests(requestId,receiverId);

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
                string userId = GetAuthenticatedUserId();

                List<FriendRequestModel> pendingRequests = _friendRequestService.GetFriendRequests(userId);

                return Ok(pendingRequests);
            }
            catch (Exception ex)
            {
                 return StatusCode(500, new { Number = 500, Error = "An error occurred while processing the request" });
            }
        }

        // Helper method to get the currently authenticated user's ID 
        private string GetAuthenticatedUserId(){
            var httpContext = _httpContextAccessor.HttpContext;
            var userIdClaim = httpContext?.User.FindFirst(ClaimTypes.NameIdentifier);


            return userIdClaim?.Value;
        }
    }
}
