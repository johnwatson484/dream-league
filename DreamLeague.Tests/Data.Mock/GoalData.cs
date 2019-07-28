using DreamLeague.Models;
using System.Collections.Generic;

namespace DreamLeague.Tests.Data.Mock
{
    public static class GoalData
    {
        public static List<Goal> Data()
        {
            return new List<Goal>
            {
                new Goal
                {
                    GoalId = 1,
                    GameWeek = new GameWeek(),
                    Manager = new Manager(),
                    Player = new Player
                    {
                        Team = new Team
                        {
                            League = new League()
                        }
                    }
                },
                new Goal
                {
                    GoalId = 2,
                    GameWeek = new GameWeek(),
                    Manager = new Manager(),
                    Player = new Player
                    {
                        Team = new Team
                        {
                            League = new League()
                        }
                    }
                }
            };
        }
    }
}

