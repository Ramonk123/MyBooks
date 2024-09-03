using System.ComponentModel.DataAnnotations;

namespace MyBooks.Models.Account;

public class LoginVM
{
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}