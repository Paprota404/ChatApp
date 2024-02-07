using System.ComponentModel.DataAnnotations;

namespace SignUp.Models;

public class SignUpModel{
    public int id {get;set;}
    [Required]
    [EmailAddress]
    public string email{get;set;}
    [Required]
    public string password{get;set;}
}