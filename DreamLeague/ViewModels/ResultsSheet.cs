using DreamLeague.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DreamLeague.ViewModels
{
    public class ResultsSheet
    {
        public int GameWeekId { get; set; }

        public List<ResultsSheetGoalKeeper> GoalKeepers { get; set; }

        public List<ResultsSheetPlayer> Players { get; set; }

        public List<ManagerCupWeek> ManagerCupWeeks { get; set; }

        public ResultsSheet()
        {
            GoalKeepers = new List<ResultsSheetGoalKeeper>();
            Players = new List<ResultsSheetPlayer>();
            ManagerCupWeeks = new List<ManagerCupWeek>();
        }

        public ResultsSheet(List<ManagerGoalKeeper> goalKeepers, List<ManagerPlayer> players, List<ManagerCupWeek> managerCupWeeks):this()
        {
            ManagerCupWeeks = managerCupWeeks;

            foreach(var goalKeeper in goalKeepers)
            {
                if (!goalKeeper.Substitute)
                {
                    ResultsSheetGoalKeeper resultsSheetGoalKeeper = new ResultsSheetGoalKeeper(goalKeeper);
                    GoalKeepers.Add(resultsSheetGoalKeeper);
                }
                else
                {
                    ResultsSheetGoalKeeper resultsSheetGoalKeeper = GoalKeepers.Where(x => x.GoalKeeper.ManagerId == goalKeeper.ManagerId).FirstOrDefault();

                    if(resultsSheetGoalKeeper!= null)
                    {
                        resultsSheetGoalKeeper.Substitute = goalKeeper;
                    }
                }
            }

            foreach (var player in players)
            {
                ResultsSheetPlayer resultsSheetPlayer = new ResultsSheetPlayer(player);
                Players.Add(resultsSheetPlayer);
            }
        }

        public class ResultsSheetGoalKeeper
        {
            public ManagerGoalKeeper GoalKeeper { get; set; }

            public ManagerGoalKeeper Substitute { get; set; }

            public int Conceded { get; set; }

            public int CupConceded { get; set; }

            public bool SubstitutePlayed { get; set; }

            public ResultsSheetGoalKeeper() { }

            public ResultsSheetGoalKeeper(ManagerGoalKeeper goalKeeper)
            {
                GoalKeeper = goalKeeper;
            }
        }

        public class ResultsSheetPlayer
        {
            public ManagerPlayer Player { get; set; }

            public int Goals { get; set; }

            public int CupGoals { get; set; }

            public ResultsSheetPlayer() { }

            public ResultsSheetPlayer(ManagerPlayer player)
            {
                Player = player;
            }
        }
    }
}