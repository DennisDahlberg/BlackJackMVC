using Services.ViewModels;

namespace Services.Services;

public class GameService
{
    public bool CheckWin(GameViewModel model)
    {
        if (model.ComputerPoints < 22 && model.ComputerPoints >= model.PlayerPoints)
            return false;
        return true;
    }
}