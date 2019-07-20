using DreamLeague.DAL;
using DreamLeague.Models;
using DreamLeague.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DreamLeague.Services
{
    public class CupService : ICupService
    {
        readonly DreamLeagueContext db;
        readonly IGameWeekSerializer<CupWeekSummary> cupWeekSerializer;

        public CupService(DreamLeagueContext db, IGameWeekSerializer<CupWeekSummary> cupWeekSerializer)
        {
            this.db = db;
            this.cupWeekSerializer = cupWeekSerializer;
        }

        public CupViewModel GetData(int cupId)
        {
            Cup cup = db.Cups.AsNoTracking().Include(x => x.Groups.Select(g => g.Managers)).Include(x => x.Fixtures).Where(x => x.CupId == cupId).FirstOrDefault();

            var gameWeekId = cup.Fixtures.Where(x => x.GameWeek.Complete).OrderByDescending(x => x.GameWeek.Number).Select(x => x.GameWeekId).FirstOrDefault();

            CupWeekSummary cupWeekSummary = new CupWeekSummary();

            if (gameWeekId != 0)
            {
                cupWeekSummary = cupWeekSerializer.DeSerialize(gameWeekId, string.Format("CupWeek_{0}", cup.CupId));
            }

            List<GroupTable> groupTables = new List<GroupTable>();
            List<FixtureResult> fixtures = new List<FixtureResult>();

            foreach (var group in cup.Groups.OrderBy(x => x.Name))
            {
                Table table = cupWeekSummary.Groups.Where(x => x.GroupId == group.GroupId).FirstOrDefault();

                if (table == null)
                {
                    table = new Table(group.Name, group.GroupId, group.Managers);
                }

                groupTables.Add(new GroupTable(group, table));
            }

            var completedGameWeeks = db.GameWeeks.AsNoTracking().Where(x => x.Complete);
            var fixtureGameWeeks = db.Fixtures.AsNoTracking().Where(x => x.CupId == cup.CupId && completedGameWeeks.Any(p => p.GameWeekId == x.GameWeekId)).Select(x => x.GameWeekId).Distinct();

            List<CupScore> cupScores = new List<CupScore>();

            foreach (var fixtureGameWeek in fixtureGameWeeks)
            {
                cupWeekSummary = cupWeekSerializer.DeSerialize(fixtureGameWeek, string.Format("CupWeek_{0}", cup.CupId));

                if (cupWeekSummary != null)
                {
                    foreach (var cupScore in cupWeekSummary.Scores)
                    {
                        cupScores.Add(cupScore);
                    }
                }
            }

            foreach (var fixture in cup.Fixtures.OrderByDescending(x => x.GameWeek.Number))
            {
                CupScore cupScore = cupScores.Where(x => x.FixtureId == fixture.FixtureId).FirstOrDefault();

                if (cupScore != null)
                {
                    fixtures.Add(new FixtureResult(fixture, cupScore.HomeScore.Margin, cupScore.AwayScore.Margin));
                }
                else
                {
                    fixtures.Add(new FixtureResult(fixture));
                }
            }

            return new CupViewModel(cup, groupTables, fixtures);
        }
    }
}