using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Models;

public class ApplicationUser : IdentityUser
{
    public decimal Balance { get; set; }
    public List<Game> Games { get; set; } = [];
}