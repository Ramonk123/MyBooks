using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Data.Models;

public class User : IdentityUser
{
    [MaxLength(48)]
    public string FullName { get; set; }
    public ICollection<Library> Libraries { get; set; }
}