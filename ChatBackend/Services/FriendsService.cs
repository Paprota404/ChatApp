using Chat.Database;
using Friends.Models;
using Microsoft.EntityFrameworkCore;
using UserDetails.Models;
using Microsoft.AspNetCore.Identity;

namespace Friends.Services{

    public interface IFriendsService{
         Task<List<UserDetailsModel>> GetFriends(string userId);
    }

    public class FriendsService : IFriendsService{
        private readonly AppDbContext _dbContext;

        private readonly UserManager<IdentityUser> _userManager;     

        public FriendsService(AppDbContext dbContext,UserManager<IdentityUser> userManager){
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<List<UserDetailsModel>> GetFriends(string userId){
            var friends = _dbContext.friends.Where(fr => fr.user1_id == userId || fr.user2_id ==userId).ToList();

            var friendsDetails = new List<UserDetailsModel>();

            foreach (var friend in friends){
                var oppositeUserId = (friend.user1_id == userId) ? friend.user1_id : friend.user2_id;

                var userDetails = await _userManager.FindByIdAsync(oppositeUserId);

                if(userDetails != null){
                    friendsDetails.Add(new UserDetailsModel{
                        UserId = userDetails.Id,
                        Username = userDetails.UserName
                    });
                }
            }

            return friendsDetails;
    }
}
}