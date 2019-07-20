using DreamLeague.DAL;
using DreamLeague.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DreamLeague.Services
{
    public class CupWeekSummaryService : IGameWeekSummaryService
    {
        readonly DreamLeagueContext db;
        readonly IGameWeekSerializer<CupWeekSummary> gameWeekSerializer;

        public CupWeekSummaryService(DreamLeagueContext db, IGameWeekSerializer<CupWeekSummary> gameWeekSerializer)
        {
            this.db = db;
            this.gameWeekSerializer = gameWeekSerializer;
        }

        public void Create(int gameWeekId)
        {
            var gameWeek = db.GameWeeks.Find(gameWeekId);
            var fixtures = db.Fixtures.AsNoTracking().Include(x => x.Cup).Where(x => x.GameWeekId == gameWeekId);

            if (fixtures.Count() == 0)
            {
                return;
            }

            var cups = fixtures.Select(x => x.Cup).Distinct();

            var scores = GetScores(gameWeekId);

            List<CupScore> cupScores = new List<CupScore>();

            foreach (var fixture in fixtures)
            {
                var homeScore = scores.Where(x => x.ManagerId == fixture.HomeManagerId).FirstOrDefault();
                var awayScore = scores.Where(x => x.ManagerId == fixture.AwayManagerId).FirstOrDefault();

                cupScores.Add(new CupScore(fixture.CupId, fixture.FixtureId, fixture.Round, homeScore, awayScore));
            }

            foreach (var cup in cups)
            {
                List<Table> groups = new List<Table>();

                if (cup.HasGroupStage && db.Fixtures.AsNoTracking().Where(x => x.Round == 1 && x.CupId == cup.CupId && x.GameWeek.Number <= gameWeek.Number).Count() > 0)
                {
                    foreach (var group in cup.Groups)
                    {
                        groups.Add(GetTable(gameWeekId, group.GroupId));
                    }
                }

                CupWeekSummary cupWeekSummary = new CupWeekSummary(gameWeek, cup.Name, cup.CupId, cupScores.Where(x => x.CupId == cup.CupId).ToList(), groups);

                gameWeekSerializer.Serialize(cupWeekSummary, gameWeekId, string.Format("CupWeek_{0}", cup.CupId));
            }
        }

        public List<Score> GetScores(int gameWeekId)
        {
            List<Score> scores = new List<Score>();

            var managers = db.Managers.AsNoTracking().OrderBy(x => x.Name);

            foreach (var manager in managers)
            {
                var goals = manager.GameWeekGoals(gameWeekId, true);
                var conceded = manager.GameWeekConceded(gameWeekId, true);
                bool substituteGoalKeeper = conceded.Select(x => x.Substitute).FirstOrDefault();

                List<Scorer> scorers = new List<Scorer>();

                foreach (var goal in goals.GroupBy(x => x.PlayerId).Select(g => new { PlayerId = g.Key, Goals = g.Count() }))
                {
                    var player = goals.Where(x => x.PlayerId == goal.PlayerId).FirstOrDefault();

                    Scorer scorer = new Scorer(player.Player.LastNameInitial, goal.Goals);
                    scorers.Add(scorer);
                }

                Score score = new Score(manager.ManagerId, manager.Name, goals.Count(), scorers, conceded.Count(), substituteGoalKeeper);
                scores.Add(score);
            }

            return scores;
        }

        public Table GetTable(int gameWeekId, int? groupId)
        {
            var gameWeek = db.GameWeeks.Find(gameWeekId);
            var group = db.Groups.AsNoTracking().Include(x => x.Managers).Where(x => x.GroupId == groupId).FirstOrDefault();

            Table table = new Table(group.Name, group.GroupId);

            foreach (var manager in group.Managers)
            {
                int[] gameWeekPoints = GetGameWeekPoints(manager.ManagerId, gameWeekId, group.CupId);

                int won = 0;
                int drawn = 0;
                int lost = 0;
                int points = gameWeekPoints.Sum();

                for (int i = 0; i < gameWeekPoints.Length; i++)
                {
                    switch (gameWeekPoints[i])
                    {
                        case 3:
                            won++;
                            break;
                        case 1:
                            drawn++;
                            break;
                        case 0:
                            lost++;
                            break;
                        default:
                            break;
                    }
                }

                int gf = GetGameWeekGoals(manager.ManagerId, gameWeekId, group.CupId);
                int ga = GetGameWeekConceded(manager.ManagerId, gameWeekId, group.CupId);

                TableRow tableRow = new TableRow(manager.ManagerId, manager.Name, gameWeek.Number, won, drawn, lost, gf, ga, points);

                table.TableRows.Add(tableRow);
            }

            table.Order();

            return table;
        }

        public int[] GetGameWeekPoints(int managerId, int gameWeekId, int cupId)
        {
            var currentGameWeek = db.GameWeeks.Find(gameWeekId);
            var fixtures = db.Fixtures.AsNoTracking().Where(x => x.CupId == cupId && x.Round == 1 && x.GameWeek.Number <= currentGameWeek.Number && (x.HomeManagerId == managerId || x.AwayManagerId == managerId));
            var manager = db.Managers.Find(managerId);

            int[] gameWeekPoints = new int[fixtures.Count()];
            int i = 0;

            foreach (var fixture in fixtures)
            {
                var scores = GetScores(fixture.GameWeekId);

                var homeScore = scores.Where(x => x.ManagerId == fixture.HomeManagerId).FirstOrDefault();
                var awayScore = scores.Where(x => x.ManagerId == fixture.AwayManagerId).FirstOrDefault();

                var cupScore = new CupScore(fixture.CupId, fixture.FixtureId, fixture.Round, homeScore, awayScore);

                gameWeekPoints[i] = cupScore.GetManagerPoints(managerId);
                i++;
            }

            return gameWeekPoints;
        }

        public int GetGameWeekGoals(int managerId, int gameWeekId, int cupId)
        {
            var currentGameWeek = db.GameWeeks.Find(gameWeekId);
            var fixtures = db.Fixtures.AsNoTracking().Where(x => x.CupId == cupId && x.Round == 1 && x.GameWeek.Number <= currentGameWeek.Number && (x.HomeManagerId == managerId || x.AwayManagerId == managerId));
            var manager = db.Managers.Find(managerId);

            int goals = 0;

            foreach (var fixture in fixtures)
            {
                var scores = GetScores(fixture.GameWeekId);

                var homeScore = scores.Where(x => x.ManagerId == fixture.HomeManagerId).FirstOrDefault();
                var awayScore = scores.Where(x => x.ManagerId == fixture.AwayManagerId).FirstOrDefault();

                var cupScore = new CupScore(fixture.CupId, fixture.FixtureId, fixture.Round, homeScore, awayScore);

                goals += cupScore.GetManagerGoals(managerId);
            }

            return goals;
        }

        public int GetGameWeekConceded(int managerId, int gameWeekId, int cupId)
        {
            var currentGameWeek = db.GameWeeks.Find(gameWeekId);
            var fixtures = db.Fixtures.AsNoTracking().Where(x => x.CupId == cupId && x.Round == 1 && x.GameWeek.Number <= currentGameWeek.Number && (x.HomeManagerId == managerId || x.AwayManagerId == managerId));
            var manager = db.Managers.Find(managerId);

            int conceded = 0;

            foreach (var fixture in fixtures)
            {
                var scores = GetScores(fixture.GameWeekId);

                var homeScore = scores.Where(x => x.ManagerId == fixture.HomeManagerId).FirstOrDefault();
                var awayScore = scores.Where(x => x.ManagerId == fixture.AwayManagerId).FirstOrDefault();

                var cupScore = new CupScore(fixture.CupId, fixture.FixtureId, fixture.Round, homeScore, awayScore);

                conceded += cupScore.GetManagerConceded(managerId);
            }

            return conceded;
        }

        public void Refresh()
        {
            gameWeekSerializer.DeleteAll("CupWeek");

            var completedGameWeeks = db.GameWeeks.AsNoTracking().Where(x => x.Complete);
            var fixtureGameWeeks = db.Fixtures.AsNoTracking().Where(x => completedGameWeeks.Any(p => p.GameWeekId == x.GameWeekId)).Select(x => x.GameWeekId).Distinct();

            foreach (var gameWeekId in fixtureGameWeeks)
            {
                Create(gameWeekId);
            }
        }
    }
}