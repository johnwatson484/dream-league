using System.Collections.Generic;

namespace DreamLeague.ViewModels
{
    public class Score
    {
        public int ManagerId { get; set; }

        public string Manager { get; set; }

        public int Goals { get; set; }

        public int Conceded { get; set; }

        public bool SubstituteGoalKeeper { get; set; }

        public List<Scorer> Scorers { get; set; }

        public int Margin
        {
            get
            {
                return Goals - Conceded;
            }
        }

        public string Result
        {
            get
            {
                if (Goals > Conceded)
                {
                    return "Won";
                }
                else if (Conceded > Goals)
                {
                    return "Lost";
                }
                else
                {
                    return "Drawn";
                }

            }
        }

        public Score()
        {
            Scorers = new List<Scorer>();
        }

        public Score(int managerId, string manager, int goals, List<Scorer> scorers, int conceded, bool substituteGoalKeeper)
        {
            ManagerId = managerId;
            Manager = manager;
            Goals = goals;
            Conceded = conceded;
            SubstituteGoalKeeper = substituteGoalKeeper;
            Scorers = scorers;
        }
    }

    public class Scorer
    {
        public string Name { get; set; }

        public int Goals { get; set; }

        public Scorer() { }

        public Scorer(string name, int goals)
        {
            Name = name;
            Goals = goals;
        }
    }
}