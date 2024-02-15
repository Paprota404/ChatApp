using Microsoft.EntityFrameworkCore;



namespace Requests.Models{

public class FriendRequestModel{
    public int id {get; set;}

    public int request_sender_id {get;set;}

    public int request_receiver_id {get;set;}

    public FriendRequestStatus status {get;set;} = FriendRequestStatus.Pending;

    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
}

public enum FriendRequestStatus{
    Pending,Accepted,Rejected
}



    public class AppDbContext : DbContext{
        public DbSet<FriendRequestModel> friend_requests {get;set;}

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }

}