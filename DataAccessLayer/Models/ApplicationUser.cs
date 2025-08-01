using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Models;

public class ApplicationUser : IdentityUser
{
    public decimal Balance { get; set; } = 1000m;
    public List<Game> Games { get; set; } = [];
}