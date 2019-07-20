using DreamLeague.DAL;
using DreamLeague.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace DreamLeague.Services
{
    public class GameWeekSummaryService : IGameWeekSummaryService
    {
        DreamLeagueContext db;
        IGameWeekSerializer<GameWeekSummary> gameWeekSerializer;

        public GameWeekSummaryService(DreamLeagueContext db, IGameWeekSerializer<GameWeekSummary> gameWeekSerializer)
        {
            this.db = db;
            this.gameWeekSerializer = gameWeekSerializer;
        }

        public void Create(int gameWeekId)
        {
            var gameWeek = db.GameWeeks.Find(gameWeekId);
            var scores = GetScores(gameWeekId);
            var table = GetTable(gameWeekId);

            GameWeekSummary gameWeekSummary = new GameWeekSummary(gameWeek, scores, table);

            gameWeekSerializer.Serialize(gameWeekSummary, gameWeekId, "GameWeek");
        }

        public List<Score> GetScores(int gameWeekId)
        {
            List<Score> scores = new List<Score>();

            var managers = db.Managers.AsNoTracking().OrderBy(x => x.Name);

            foreach (var manager in managers)
            {
                var goals = manager.GameWeekGoals(gameWeekId);
                var conceded = manager.GameWeekConceded(gameWeekId);
                bool substituteGoalKeeper = conceded.Select(x => x.Substitute).FirstOrDefault();

                List<Scorer> scorers = new List<Scorer>();

                foreach (var goal in goals.GroupBy(x => x.PlayerId).Select(g => new { PlayerId = g.Key, Goals = g.Count() }))
                {
                    var player = db.Players.Find(goal.PlayerId);

                    Scorer scorer = new Scorer(player.LastNameInitial, goal.Goals);
                    scorers.Add(scorer);
                }

                Score score = new Score(manager.ManagerId, manager.Name, goals.Count(), scorers.OrderBy(x => x.Name).ToList(), conceded.Count(), substituteGoalKeeper);
                scores.Add(score);
            }

            return scores;
        }

        public Table GetTable(int gameWeekId, int? groupId = null)
        {
            Table table = new Table();

            var gameWeek = db.GameWeeks.Find(gameWeekId);
            var managers = db.Managers.AsNoTracking();

            foreach (var manager in managers)
            {
                int[] gameWeekPoints = GetGameWeekPoints(manager.ManagerId, gameWeekId);

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

                int gf = db.Goals.AsNoTracking().Where(x => x.GameWeek.Number <= gameWeek.Number && x.ManagerId == manager.ManagerId && x.Cup == false).Count();
                int ga = db.Conceded.AsNoTracking().Where(x => x.GameWeek.Number <= gameWeek.Number && x.ManagerId == manager.ManagerId && x.Cup == false).Count();

                TableRow tableRow = new TableRow(manager.ManagerId, manager.Name, gameWeek.Number, won, drawn, lost, gf, ga, points);

                table.TableRows.Add(tableRow);
            }

            table.Order();

            return table;
        }

        public int[] GetGameWeekPoints(int managerId, int gameWeekId)
        {
            var currentGameWeek = db.GameWeeks.Find(gameWeekId);
            var gameWeeks = db.GameWeeks.AsNoTracking().Where(x => x.Number <= currentGameWeek.Number);
            var manager = db.Managers.Find(managerId);

            int[] gameWeekPoints = new int[currentGameWeek.Number];
            int i = 0;

            foreach (var gameWeek in gameWeeks)
            {
                gameWeekPoints[i] = manager.GameWeekPoints(gameWeek.GameWeekId);
                i++;
            }

            return gameWeekPoints;
        }

        public void Refresh()
        {
            gameWeekSerializer.DeleteAll("GameWeek");

            var gameWeeks = db.GameWeeks.AsNoTracking().Where(x => x.Complete);

            foreach (var gameWeek in gameWeeks)
            {
                Create(gameWeek.GameWeekId);
            }
        }
    }
}