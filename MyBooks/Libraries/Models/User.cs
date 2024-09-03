using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MyBooks.Libraries.Models;

public class User : IdentityUser
{
    public Guid PublicId { get; set; }
    
    [MaxLength(48)]
    public string FullName { get; set; }


}