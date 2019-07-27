using DreamLeague.Models;
using System.Collections.Generic;

namespace DreamLeague.Tests.Data.Mock
{
    public static class FixtureData
    {
        public static List<Fixture> Data()
        {
            return new List<Fixture>
            {
                new Fixture
                {
                    FixtureId = 1,
                    CupId = 1
                },
                new Fixture
                {
                    FixtureId = 2,
                    CupId = 2
                }
            };
        }
    }
}

