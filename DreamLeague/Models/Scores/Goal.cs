using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DreamLeague.Models.Scores
{
    public class Goal
    {
        public int GoalId { get; set; }

        public int FixtureId { get; set; }

        public string For { get; set; }

        public string Team
        {
            get
            {
                if(!OwnGoal)
                {
                    return For;
                }

                if(Fixture.HomeTeam == For)
                {
                    return Fixture.AwayTeam;
                }

                return Fixture.HomeTeam;
            }
        }

        public string Scorer { get; set; }

        public int Minute { get; set; }

        public bool OwnGoal { get; set; }

        public virtual Fixture Fixture { get; set; }
    }
}