using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Messages.Models{
    public class Message{
        public int Id {get;set;}
        
        [ForeignKey("SenderId")]
        public IdentityUser Sender { get; set; }
        
        [ForeignKey("ReceiverId")]
        public IdentityUser Receiver { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}