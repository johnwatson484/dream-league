using DreamLeague.Models;
using System.Collections.Generic;

namespace DreamLeague.Tests.Data.Mock
{
    public static class CupData
    {
        public static List<Cup> Data()
        {
            return new List<Cup>
            {
                new Cup
                {
                    CupId = 1,
                    Name = "Cup"
                },
                new Cup
                {
                    CupId = 2,
                    Name = "League Cup"
                }
            };
        }
    }
}

