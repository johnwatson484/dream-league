using DreamLeague.DAL;
using DreamLeague.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace DreamLeague.Services
{
    public class StatisticsService : IStatisticsService
    {
        DreamLeagueContext db;

        public StatisticsService(DreamLeagueContext db)
        {
            this.db = db;
        }

        public Form GetForm(int managerId, int weeks = 6)
        {
            var manager = db.Managers.Find(managerId);

            var gameWeeks = db.GameWeeks.AsNoTracking().Where(x => x.Complete).OrderByDescending(x => x.Number).Take(weeks);

            StringBuilder sb = new StringBuilder();

            foreach (var gameWeek in gameWeeks.OrderBy(x => x.Number))
            {
                sb.Append(manager.GameWeekResult(gameWeek.GameWeekId));
            }

            Form form = new Form(managerId, manager.Name, sb.ToString());

            return form;
        }

        public decimal GetAverageGoals(int managerId)
        {
            decimal averageGoals = 0;

            var weeks = db.GameWeeks.AsNoTracking().Where(x => x.Complete).Count();

            var goals = db.Goals.AsNoTracking().Where(x => x.ManagerId == managerId).Count();

            if (weeks > 0)
            {
                averageGoals = Math.Round((decimal)goals / weeks, 2);
            }

            return averageGoals;
        }

        public decimal GetAverageConceded(int managerId)
        {
            decimal averageConceded = 0;

            var weeks = db.GameWeeks.AsNoTracking().Where(x => x.Complete).Count();

            var conceded = db.Conceded.AsNoTracking().Where(x => x.ManagerId == managerId).Count();

            if (weeks > 0)
            {
                averageConceded = Math.Round((decimal)conceded / weeks, 2);
            }

            return averageConceded;
        }
    }
}