using DreamLeague.DAL;
using DreamLeague.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DreamLeague.Services
{
    public class AuditService : IAuditService
    {
        DreamLeagueContext db;

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