using DreamLeague.Models;
using System.Collections.Generic;

namespace DreamLeague.Tests.Data.Mock
{
    public static class TeamData
    {
        public static List<Team> Data()
        {
            return new List<Team>
            {
                new Team
                {
                    TeamId = 1,
                    Name = "Newcastle United",
                    League = new League()
                },
                new Team
                {
                    TeamId = 2,
                    Name = "Sunderland",
                    League = new League()
                }
            };
        }
    }
}

