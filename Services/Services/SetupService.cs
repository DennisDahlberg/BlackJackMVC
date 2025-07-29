using DataAccessLayer.Data;

namespace Services.Services;

public class SetupService
{
    private readonly ApplicationDbContext _context;

    public SetupService(ApplicationDbContext context)
    {
        _context = context;
    }

    public bool IsBetValid(decimal betAmount, decimal userMoney)
    {
        return betAmount <= userMoney;
    }
}