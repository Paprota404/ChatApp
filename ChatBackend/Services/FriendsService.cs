using Chat.Database;
using Friends.Models;
using Microsoft.EntityFrameworkCore;

namespace Friends.Services{

    public interface IFriendsService{
        List<FriendsModel> GetFriends(string userId)
    }

    public class FriendsService : IFriendsService{
        private readonly AppDbContext _dbContext;

        public FriendsService(AppDbContext dbContext){
            _dbContext = dbContext;
        }

        public List<FriendsModel> GetFriends(string userId){
            var friends = _dbContext.friends.Where(fr => fr.user1_id == userId || user2_id ==userId).ToList();

            var friendDetails = new List<FriendsModel>();

            foreach (var friend in friends){
                var opositeUser = (friend.user1_id == userId) ? friend.user1_id : friend.user2_id;

                var userDetails
            }
    }
}