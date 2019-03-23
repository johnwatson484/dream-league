using DreamLeague.ViewModels;

namespace DreamLeague.Services
{
    public interface IStatisticsService
    {
        decimal GetAverageConceded(int managerId);
        decimal GetAverageGoals(int managerId);
        Form GetForm(int managerId, int weeks = 6);
    }
}