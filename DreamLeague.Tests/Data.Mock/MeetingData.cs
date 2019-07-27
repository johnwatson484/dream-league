using DreamLeague.Models;
using System;
using System.Collections.Generic;

namespace DreamLeague.Tests.Data.Mock
{
    public static class MeetingData
    {
        public static List<Meeting> Data()
        {
            return new List<Meeting>
            {
                new Meeting
                {
                    MeetingId = 1,
                    Date = new DateTime(2019, 8, 1)
                },
                new Meeting
                {
                    MeetingId = 2,
                    Date = new DateTime(2019, 9, 1)
                }
            };
        }
    }
}

