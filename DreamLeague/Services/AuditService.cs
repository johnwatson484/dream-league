using DreamLeague.DAL;
using DreamLeague.Models;
using System;

namespace DreamLeague.Services
{
    public class AuditService : IAuditService
    {
        readonly DreamLeagueContext db;

        public AuditService(DreamLeagueContext db)
        {
            this.db = db;
        }

        public void Log(string area, string action, string user, string description, int? gameWeekId = null)
        {
            db.Audit.Add(new Audit
            {
                Date = DateTime.Now,
                Area = area,
                Action = action,
                User = user,
                GameWeekId = gameWeekId,
                Description = description
            });
        }
    }
}