using DreamLeague.Inputs;
using DreamLeague.Models;
using System.Collections.Generic;
using System.Linq;

namespace DreamLeague.ViewModels
{
    public class ManagerTeamSheet
    {
        public List<ManagerTeam> ManagerTeams { get; set; }

        public ManagerTeamSheet()
        {
            ManagerTeams = new List<ManagerTeam>();
        }

        public ManagerTeamSheet(List<Manager> managers, TeamSheet teamSheet) : this()
        {
            foreach (var manager in managers)
            {
                TeamSheetTeam teamSheetTeam = teamSheet.Teams.Where(x => x.Manager == manager.Alias).FirstOrDefault();             

                ManagerTeam managerTeam = new ManagerTeam(manager, teamSheetTeam);

                ManagerTeams.Add(managerTeam);
            }
        }
    }

    public class ManagerTeam
    {
        public int ManagerId { get; set; }

        public string ManagerName { get; set; }

        public List<TeamGoalKeeperGroup> TeamGoalKeeperGroups { get; set; }

        public List<TeamPlayerGroup> TeamPlayerGroups { get; set; }

        public ManagerTeam()
        {
            TeamPlayerGroups = new List<TeamPlayerGroup>();
            TeamGoalKeeperGroups = new List<TeamGoalKeeperGroup>();
        }

        public ManagerTeam(Manager manager, TeamSheetTeam teamSheetTeam = null) : this()
        {
            ManagerId = manager.ManagerId;
            ManagerName = manager.Name;

            manager.GoalKeepers = manager.GoalKeepers.OrderBy(x => x.Team.Name).ToList();
            manager.Players = manager.Players.OrderBy(x => x.Player.Position).ThenBy(x => x.Player.LastNameFirstName).ToList();

            if (teamSheetTeam != null)
            {
                teamSheetTeam.GoalKeepers = teamSheetTeam.GoalKeepers.OrderBy(x => x.Team).ToList();
                teamSheetTeam.Players = teamSheetTeam.Players.OrderBy(x => x.Position).ThenBy(x => x.Name).ToList();
            }

            for (int i = 0; i < 2; i++)
            {
                TeamGoalKeeper teamGoalKeeperA;
                TeamGoalKeeper teamGoalKeeperB = null;

                if (manager.GoalKeepers != null && i < manager.GoalKeepers.Count)
                {
                    teamGoalKeeperA = new TeamGoalKeeper(manager.GoalKeepers[i].TeamId, manager.GoalKeepers[i].Team.Name, manager.GoalKeepers[i].Substitute);
                }
                else
                {
                    teamGoalKeeperA = new TeamGoalKeeper();
                }

                if (teamSheetTeam != null && i < teamSheetTeam.GoalKeepers.Count)
                {
                    teamGoalKeeperB = new TeamGoalKeeper(0, teamSheetTeam.GoalKeepers[i].Team, teamSheetTeam.GoalKeepers[i].Substitute);
                }

                TeamGoalKeeperGroup teamGoalKeeperGroup = new TeamGoalKeeperGroup(teamGoalKeeperA, teamGoalKeeperB);

                TeamGoalKeeperGroups.Add(teamGoalKeeperGroup);
            }

            for (int i = 0; i < 13; i++)
            {
                TeamPlayer teamPlayerA;
                TeamPlayer teamPlayerB = null;

                if (manager.Players != null && i < manager.Players.Count)
                {
                    teamPlayerA = new TeamPlayer(manager.Players[i].PlayerId, manager.Players[i].Player.Details, manager.Players[i].Substitute);
                }
                else
                {
                    teamPlayerA = new TeamPlayer();
                }

                if (teamSheetTeam != null && i < teamSheetTeam.Players.Count)
                {
                    teamPlayerB = new TeamPlayer(0, teamSheetTeam.Players[i].Name, teamSheetTeam.Players[i].Substitute);
                }

                TeamPlayerGroup teamPlayerGroup = new TeamPlayerGroup(teamPlayerA, teamPlayerB);

                TeamPlayerGroups.Add(teamPlayerGroup);
            }
        }
    }

    public class TeamGoalKeeperGroup
    {
        public TeamGoalKeeper TeamGoalKeeperA { get; set; }

        public TeamGoalKeeper TeamGoalKeeperB { get; set; }

        public TeamGoalKeeperGroup() { }

        public TeamGoalKeeperGroup(TeamGoalKeeper teamGoalKeeperA, TeamGoalKeeper teamGoalKeeperB = null) : this()
        {
            TeamGoalKeeperA = teamGoalKeeperA;
            TeamGoalKeeperB = teamGoalKeeperB;
        }
    }

    public class TeamGoalKeeper
    {
        public int TeamId { get; set; }

        public string Name { get; set; }

        public bool Substitute { get; set; }

        public TeamGoalKeeper() { }

        public TeamGoalKeeper(int teamId, string name, bool substitute)
        {
            TeamId = teamId;
            Name = name;
            Substitute = substitute;
        }
    }

    public class TeamPlayerGroup
    {
        public TeamPlayer TeamPlayerA { get; set; }

        public TeamPlayer TeamPlayerB { get; set; }

        public TeamPlayerGroup() { }

        public TeamPlayerGroup(TeamPlayer teamPlayerA, TeamPlayer teamPlayerB = null) : this()
        {
            TeamPlayerA = teamPlayerA;
            TeamPlayerB = teamPlayerB;
        }
    }

    public class TeamPlayer
    {
        public int PlayerId { get; set; }

        public string Name { get; set; }

        public bool Substitute { get; set; }

        public TeamPlayer() { }

        public TeamPlayer(int playerId, string name, bool substitute)
        {
            PlayerId = playerId;
            Name = name;
            Substitute = substitute;
        }
    }
}