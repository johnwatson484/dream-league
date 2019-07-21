using DreamLeague.Models;
using System.Collections.Generic;

namespace DreamLeague.Tests.Data.Mock
{
    public static class LeagueData
    {
        public static List<League> Data()
        {
            return new List<League>
            {
                new League
                {
                    LeagueId = 1,
                    Name = "Championship",
                    Rank = 1,
                    Teams = new List<Team>()
                },
                new League
                {
                    LeagueId = 2,
                    Name = "League 1",
                    Rank = 2,
                    Teams = new List<Team>()
                },
                new League
                {
                    LeagueId = 3,
                    Name = "League 2",
                    Rank = 3,
                    Teams = new List<Team>()
                },
            };
        }
    }
}

