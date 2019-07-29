using DreamLeague.Models;
using System.Collections.Generic;

namespace DreamLeague.Tests.Data.Mock
{
    public static class GameWeekData
    {
        public static List<GameWeek> Data()
        {
            return new List<GameWeek>
            {
                new GameWeek
                {
                    GameWeekId = 1,
                    Number = 1,
                    Complete = true
                },
                new GameWeek
                {
                    GameWeekId = 2,
                    Number = 2,
                    Complete = true
                }
            };
        }
    }
}

