using DreamLeague.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DreamLeague.Inputs
{
    public class TeamSheet
    {
        public List<TeamSheetTeam> Teams { get; set; }

        public TeamSheet()
        {
            Teams = new List<TeamSheetTeam>();
        }
    }

    public class TeamSheetTeam
    {
        public string Manager { get; set; }

        public List<TeamSheetPlayer> Players { get; set; }

        public List<TeamSheetGoalKeeper> GoalKeepers { get; set; }

        public TeamSheetTeam()
        {
            GoalKeepers = new List<TeamSheetGoalKeeper>();
            Players = new List<TeamSheetPlayer>();
        }

        public TeamSheetTeam(string manager):this()
        {
            Manager = manager;
        }
    }

    public class TeamSheetGoalKeeper
    {
        public string Team { get; set; }

        public bool Substitute { get; set; }

        public TeamSheetGoalKeeper() { }

        public TeamSheetGoalKeeper(string team, bool substitute = false):this()
        {
            Team = team;
            Substitute = substitute;
        }
    }

    public class TeamSheetPlayer
    {
        public string Name { get; set; }

        public Position Position { get; set; }

        public bool Substitute { get; set; }

        public TeamSheetPlayer() { }

        public TeamSheetPlayer(string name, Position position, bool substitute = false):this()
        {
            Name = name;
            Position = position;
            Substitute = substitute;
        }
    }
}