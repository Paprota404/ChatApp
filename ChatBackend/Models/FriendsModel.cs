using Microsoft.EntityFrameworkCore;

namespace Friends.Models{

public class FriendsModel{
    public int id { get; set; }
    public int user1_id { get; set; }
    public int user2_id { get; set; }
}
}