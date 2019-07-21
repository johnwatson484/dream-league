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
                    Name = "Bristol City",
                    League = new League()
                },
                new Team
                {
                    Name = "Bristol Rovers",
                    League = new League()
                }
            };
        }
    }
}

