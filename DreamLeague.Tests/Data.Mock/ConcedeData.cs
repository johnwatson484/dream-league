using DreamLeague.Models;
using System.Collections.Generic;

namespace DreamLeague.Tests.Data.Mock
{
    public static class ConcedeData
    {
        public static List<Concede> Data()
        {
            return new List<Concede>
            {
                new Concede
                {
                    ConcedeId = 1,
                    GameWeek = new GameWeek(),
                    Manager = new Manager(),
                    Team = new Team
                    {
                        League = new League()
                    }
                },
                new Concede
                {
                    ConcedeId = 2,
                    GameWeek = new GameWeek(),
                    Manager = new Manager(),
                    Team = new Team
                    {
                        League = new League()
                    }
                },
            };
        }
    }
}

