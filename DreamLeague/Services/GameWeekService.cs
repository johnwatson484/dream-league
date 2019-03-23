using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DreamLeague.Models;
using DreamLeague.DAL;
using DreamLeague.ViewModels;

namespace DreamLeague.Services
{
    public class GameWeekService : IGameWeekService
    {
        DreamLeagueContext db;

        public GameWeekService(DreamLeagueContext db)
        {
            this.db = db;
        }

        public GameWeek GetCurrent()
        {
            var currentDate = DateTime.UtcNow;
                        
            var gameWeek = db.GameWeeks.Where(x => x.Start <= currentDate).OrderByDescending(x => x.Start).FirstOrDefault();
            
            return gameWeek;
        }

        public GameWeek GetLatest()
        {
            var currentDate = DateTime.UtcNow;

            var gameWeek = db.GameWeeks.Where(x => x.Start <= currentDate && x.Complete).OrderByDescending(x => x.Start).FirstOrDefault();

            return gameWeek;
        }

        public List<ManagerCupWeek> ManagerCupWeeks(int? gameWeekId = null)
        {
            List<ManagerCupWeek> managerCupWeeks = new List<ManagerCupWeek>();

            var managers = db.Managers.AsNoTracking();
            var fixtures = db.Fixtures.AsNoTracking();

            foreach(var manager in managers)
            {
                foreach(var fixture in fixtures)
                {
                    if (gameWeekId == null)
                    {
                        if (fixture.HomeManagerId == manager.ManagerId || fixture.AwayManagerId == manager.ManagerId)
                        {
                            managerCupWeeks.Add(new ManagerCupWeek(manager.ManagerId, fixture.GameWeekId));
                        }
                    }
                    else if(fixture.GameWeekId == gameWeekId && (fixture.HomeManagerId == manager.ManagerId || fixture.AwayManagerId == manager.ManagerId))
                    {
                        managerCupWeeks.Add(new ManagerCupWeek(manager.ManagerId, fixture.GameWeekId));
                    }
                }
            }

            return managerCupWeeks;
        }
    }
}