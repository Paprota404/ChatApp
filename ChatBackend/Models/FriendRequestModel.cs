using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;




namespace Requests.Models{

public class FriendRequestModel{
    public int id {get; set;}

    public int request_sender_id {get;set;}
    [NotMapped]
    public string receiver_username {get;set;}

    public int? request_receiver_id {get;set;}

    public FriendRequestStatus status {get;set;} = FriendRequestStatus.Pending;

    public DateTime created_at {get; set;} = DateTime.UtcNow;
}

public enum FriendRequestStatus{
    Pending,Accepted,Rejected
}

}