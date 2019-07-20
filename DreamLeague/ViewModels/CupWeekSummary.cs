using DreamLeague.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DreamLeague.ViewModels
{
    [Serializable]
    public class CupWeekSummary
    {
        public GameWeek GameWeek { get; set; }

        public int CupId { get; set; }

        public string Cup { get; set; }

        public List<CupScore> Scores { get; set; }

        public List<Table> Groups { get; set; }

        public bool GroupCurrent
        {
            get
            {
                return Scores.Select(x => x.Round).FirstOrDefault() == 1 ? true : false;
            }
        }

        public CupWeekSummary()
        {

            GameWeek = new GameWeek();

            Scores = new List<CupScore>();

            Groups = new List<Table>();
        }

        public CupWeekSummary(GameWeek gameWeek, string cup, int cupId, List<CupScore> scores, List<Table> groups = null)
        {
            GameWeek = gameWeek;
            Cup = cup;
            CupId = cupId;
            Scores = scores;
            Groups = groups;
        }
    }

    public class CupScore
    {
        public int FixtureId { get; set; }

        public int CupId { get; set; }

        public int Round { get; set; }

        public Score HomeScore { get; set; }

        public Score AwayScore { get; set; }

        public string Winner
        {
            get
            {
                if (HomeScore.Margin > AwayScore.Margin)
                {
                    return "Home";
                }
                if (AwayScore.Margin > HomeScore.Margin)
                {
                    return "Away";
                }
                return "Draw";
            }
        }

        public CupScore()
        {
            HomeScore = new Score();
            AwayScore = new Score();
        }

        public CupScore(int cupId, int fixtureId, int round, Score homeScore, Score awayScore)
        {
            CupId = cupId;
            FixtureId = fixtureId;
            Round = round;
            HomeScore = homeScore;
            AwayScore = awayScore;
        }

        public int GetManagerPoints(int managerId)
        {
            Score managerScore;
            Score opponentScore;

            if (HomeScore.ManagerId == managerId)
            {
                managerScore = HomeScore;
                opponentScore = AwayScore;
            }
            else if (AwayScore.ManagerId == managerId)
            {
                managerScore = AwayScore;
                opponentScore = HomeScore;
            }
            else
            {
                return 0;
            }

            if (managerScore.Margin > opponentScore.Margin)
            {
                return 3;
            }
            if (managerScore.Margin < opponentScore.Margin)
            {
                return 0;
            }
            return 1;
        }

        public int GetManagerGoals(int managerId)
        {
            Score managerScore;
            Score opponentScore;

            if (HomeScore.ManagerId == managerId)
            {
                managerScore = HomeScore;
                opponentScore = AwayScore;
            }
            else if (AwayScore.ManagerId == managerId)
            {
                managerScore = AwayScore;
                opponentScore = HomeScore;
            }
            else
            {
                return 0;
            }

            return managerScore.Margin;
        }

        public int GetManagerConceded(int managerId)
        {
            Score managerScore;
            Score opponentScore;

            if (HomeScore.ManagerId == managerId)
            {
                managerScore = HomeScore;
                opponentScore = AwayScore;
            }
            else if (AwayScore.ManagerId == managerId)
            {
                managerScore = AwayScore;
                opponentScore = HomeScore;
            }
            else
            {
                return 0;
            }

            return opponentScore.Margin;
        }
    }
}