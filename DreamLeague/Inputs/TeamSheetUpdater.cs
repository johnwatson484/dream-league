using DreamLeague.DAL;
using DreamLeague.Models;
using DreamLeague.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DreamLeague.Inputs
{
    public class TeamSheetUpdater : ITeamSheetUpdater
    {
        readonly IDreamLeagueContext db;

        public TeamSheetUpdater(IDreamLeagueContext db)
        {
            this.db = db;
        }

        public void Update(TeamSheet teamSheet)
        {
            foreach (var team in teamSheet.Teams)
            {
                var managerId = db.Managers.Where(x => x.Alias == team.Manager).Select(x => x.ManagerId).FirstOrDefault();

                if (managerId != 0)
                {
                    db.ManagerGoalKeepers.RemoveRange(db.ManagerGoalKeepers.Where(x => x.ManagerId == managerId));
                    db.ManagerPlayers.RemoveRange(db.ManagerPlayers.Where(x => x.ManagerId == managerId));
                    MapTeams(team, managerId);
                    MapPlayers(team, managerId);
                }
            }

            db.SaveChanges();
        }

        private void MapPlayers(TeamSheetTeam team, int managerId)
        {
            var leaguePlayers = db.Players.ToList();

            foreach (var player in team.Players)
            {
                string matchText = player.Name.Replace(" ", string.Empty).ToUpper();

                int bestDistance = -1;
                int bestPlayerId = -1;
                foreach (var leaguePlayer in leaguePlayers.Where(x => x.Position == player.Position))
                {
                    var distance = LevenshteinDistance.Compute(matchText, string.Format("{0}-{1}", leaguePlayer.LastName, leaguePlayer.Team.Alias).Replace(" ", string.Empty).ToUpper());
                    if ((bestDistance == -1 || distance < bestDistance) || (distance == bestDistance && leaguePlayer.LastName.Contains(player.Name)))
                    {
                        bestDistance = distance;
                        bestPlayerId = leaguePlayer.PlayerId;
                    }
                }

                if (bestPlayerId != -1)
                {
                    db.ManagerPlayers.Add(new ManagerPlayer(bestPlayerId, managerId, player.Substitute));
                }
            }
        }

        private void MapTeams(TeamSheetTeam team, int managerId)
        {
            var leagueTeams = db.Teams.ToList();

            foreach (var goalKeeper in team.GoalKeepers)
            {
                string matchText = goalKeeper.Team.Replace(" ", string.Empty).ToUpper();

                int bestDistance = -1;
                int bestTeamId = -1;
                foreach (var leagueTeam in leagueTeams)
                {
                    var distance = LevenshteinDistance.Compute(matchText, leagueTeam.Alias.Replace(" ", string.Empty).ToUpper());
                    if (bestDistance == -1 || distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestTeamId = leagueTeam.TeamId;
                    }
                }

                if (bestTeamId != -1)
                {
                    db.ManagerGoalKeepers.Add(new ManagerGoalKeeper(bestTeamId, managerId, goalKeeper.Substitute));
                }
            }
        }
    }
}