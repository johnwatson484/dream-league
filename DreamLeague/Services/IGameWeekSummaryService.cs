using System.Collections.Generic;
using DreamLeague.ViewModels;

namespace DreamLeague.Services
{
    public interface IGameWeekSummaryService
    {
        void Create(int gameWeekId);

        void Refresh();
        
        List<Score> GetScores(int gameWeekId);

        Table GetTable(int gameWeekId, int? groupId = null);        
    }
}