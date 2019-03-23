using DreamLeague.Models;
using DreamLeague.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamLeague.Services
{
    public interface IGameWeekService
    {
        GameWeek GetCurrent();

        GameWeek GetLatest();

        List<ManagerCupWeek> ManagerCupWeeks(int? gameWeekId = null);
    }
}
