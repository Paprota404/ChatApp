using System.ComponentModel.DataAnnotations;

namespace SignUp.Models;

public class SignUpModel{
    public int id {get;set;}
    [Required]
    
    public string username{get;set;}
    [Required]
    public string password{get;set;}
}