using DreamLeague.Models;
using DreamLeague.ViewModels;
using System.Collections.Generic;

namespace DreamLeague.Services
{
    public interface IGameWeekService
    {
        GameWeek GetCurrent();

        GameWeek GetLatest();

        List<ManagerCupWeek> ManagerCupWeeks(int? gameWeekId = null);
    }
}
