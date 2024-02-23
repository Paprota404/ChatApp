using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Friends.Models{

public class FriendsModel{
    public int id { get; set; }
    public string user1_id { get; set; }
    public string user2_id { get; set; }
}
}