using DreamLeague.Models;
using System.Collections.Generic;

namespace DreamLeague.Tests.Data.Mock
{
    public static class HistoryData
    {
        public static List<History> Data()
        {
            return new List<History>
            {
                new History
                {
                    HistoryId = 1,
                    Year = 2017
                },
                new History
                {
                    HistoryId = 2,
                    Year = 2018
                }
            };
        }
    }
}

