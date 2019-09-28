using DreamLeague.DAL;
using DreamLeague.Models.Scores;
using DreamLeague.Utilities;
using DreamLeague.Models;
using DreamLeague.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace DreamLeague.Services
{
    public class ResultsService
    {
        readonly IDreamLeagueContext db;
        string url = "https://lynxmagnus.com/footballscores/api/fixtures?startDate=";
        int tolerance = 5;

        public ResultsService(IDreamLeagueContext db)
        {
            this.db = db;
        }

        public void Reconcile(ResultsSheet resultsSheet)
        {
            var gameWeek = db.GameWeeks.Find(resultsSheet.GameWeekId);

            url = string.Format("{0}{1}&endDate={2}", url, gameWeek.Start.ToString("s"), gameWeek.End.ToString("s"));

            string json;
            List<Models.Scores.Fixture> fixtures = new List<Models.Scores.Fixture>();

            using (WebClient client = new WebClient())
            {
                try
                {
                    json = client.DownloadString(url);
                }
                catch (Exception)
                {
                    json = null;
                }
            }

            if (json != null)
            {
                fixtures = JsonConvert.DeserializeObject<List<Models.Scores.Fixture>>(json);
                var date = DateTime.Now;
                
                foreach(var fixture in fixtures)
                {
                    foreach(var goal in fixture.Goals)
                    {
                        string matchText = string.Format("{0}-{1}", goal.Scorer, goal.Team).Replace(" ", string.Empty).ToUpper();

                        int bestDistance = -1;
                        int bestPlayerId = -1;
                        foreach(var player in resultsSheet.Players)
                        {
                            var distance = LevenshteinDistance.Compute(matchText, string.Format("{0}-{1}", player.Player.Player.LastName, player.Player.Player.Team.Alias).Replace(" ", string.Empty).ToUpper());
                            if (bestDistance == -1 || distance < bestDistance)
                            {
                                bestDistance = distance;
                                bestPlayerId = player.Player.ManagerPlayerId;
                            }
                        }

                        if (bestPlayerId != -1 && bestDistance < tolerance)
                        {
                            AddGoal(resultsSheet, date, resultsSheet.Players.Where(x => x.Player.ManagerPlayerId == bestPlayerId).First());
                        }
                    }
                }
            }
        }

        public void AddGoal(ResultsSheet resultsSheet, DateTime date, ResultsSheet.ResultsSheetPlayer player)
        {
            Models.Goal goal = new Models.Goal(player.Player.PlayerId, resultsSheet.GameWeekId, player.Player.ManagerId);
            goal.Created = date;
            goal.CreatedBy = "Reconciliation";
            db.Goals.Add(goal);
            var playerT = db.Players.Where(x => x.PlayerId == goal.PlayerId).FirstOrDefault();
            //auditService.Log("Goal", "Goal Added", "Reconciliation", string.Format("Goal scored for {0} ({1})", playerT?.FullName, playerT?.ManagerPlayers.FirstOrDefault()?.Manager?.Name ?? "Unattached"), resultsSheet.GameWeekId);
        }
    }
}