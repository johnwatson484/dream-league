using DreamLeague.DAL;
using DreamLeague.Models;
using System;
using System.Linq;

namespace DreamLeague.Services
{
    public class MeetingService : IMeetingService
    {
        DreamLeagueContext db;

        public MeetingService(DreamLeagueContext db)
        {
            this.db = db;
        }

        public Meeting Next()
        {
            var current = DateTime.Now;

            return db.Meetings.OrderBy(x => x.Date).Where(x => x.Date >= current).FirstOrDefault();
        }
    }
}