using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Requests.Models;
using Requests.Services;

namespace Requests.Controller{
    [Route("api/[controller]")]
    [Authorize]
    public class RequestsController : Controller
    {
    [Authorize] // Add this attribute if authentication is required
    [Route("api/[controller]")]
    [ApiController]
    public class FriendRequestController : ControllerBase
    {
        private readonly IFriendRequestService _friendRequestService;

        public FriendRequestController(IFriendRequestService friendRequestService)
        {
            _friendRequestService = friendRequestService;
        }

        [HttpPost("send")]
        public IActionResult SendFriendRequest([FromBody] FriendRequestRequestModel requestModel)
        {
            try
            {
                // Assuming you have a model to represent incoming friend request data
                // (FriendRequestRequestModel) and validation logic
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid friend request data");
                }

                // Get the currently authenticated user's ID (assuming it's stored in the claims)
                int senderId = GetAuthenticatedUserId();

                _friendRequestService.SendFriendRequest(senderId, requestModel.ReceiverId);

                return Ok("Friend request sent successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "An error occurred while processing the request");
            }
        }

        [HttpPost("accept/{requestId}")]
        public IActionResult AcceptFriendRequest(int requestId)
        {
            try
            {
                // Get the currently authenticated user's ID (assuming it's stored in the claims)
                int receiverId = GetAuthenticatedUserId();

                _friendRequestService.AcceptFriendRequest(receiverId, requestId);

                return Ok("Friend request accepted successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "An error occurred while processing the request");
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
                // Log the exception or handle it appropriately
                return StatusCode(500, "An error occurred while processing the request");
            }
        }

        // Helper method to get the currently authenticated user's ID (replace with your authentication logic)
        private int GetAuthenticatedUserId()
        {
            // Example: Get the user ID from claims
            // Replace this with your actual authentication logic
            // ClaimsPrincipal.Claims might contain the user ID claim
            // Example: User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return 1; // Placeholder value, replace with your actual logic
        }
    }
}
}