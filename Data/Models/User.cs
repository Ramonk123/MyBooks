using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MyBooks.Libraries.Models;

public class User : IdentityUser
{
    [MaxLength(48)]
    public string FullName { get; set; }
    public ICollection<Library> Libraries { get; set; }
}