using DreamLeague.DAL;
using DreamLeague.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DreamLeague.Services
{
    public class SearchService : ISearchService
    {
        DreamLeagueContext db;

        public SearchService(DreamLeagueContext db)
        {
            this.db = db;
        }

        private List<Search> BaseSearches()
        {
            List<Search> baseSearches = new List<Search>();

            baseSearches.Add(new Search("Players", "Player", "Index"));
            baseSearches.Add(new Search("Teams", "Team", "Index"));
            baseSearches.Add(new Search("Managers", "Manager", "Index"));
            baseSearches.Add(new Search("Leagues", "League", "Index"));
            baseSearches.Add(new Search("Meetings", "Meeting", "Index"));
            baseSearches.Add(new Search("Game Weeks", "GameWeek", "Index"));
            baseSearches.Add(new Search("Results", "Result", "Index"));
            baseSearches.Add(new Search("Team Sheet", "TeamSheet", "Index"));
            baseSearches.Add(new Search("History", "History", "Index"));
            baseSearches.Add(new Search("Downloads", "Download", "Index"));
            baseSearches.Add(new Search("Rules", "Home", "About"));
            baseSearches.Add(new Search("Cups", "Cup", "Index"));

            return baseSearches;
        }

        public List<Search> Search(string prefix)
        {
            List<Search> result = new List<Search>();

            var players = db.Players.AsNoTracking().Where(x => x.LastName.StartsWith(prefix) || x.FirstName.StartsWith(prefix));
            foreach(var player in players)
            {
                Search search = new Search(player.FullName, "Player", "Details", player.PlayerId.ToString());
                result.Add(search);
            }

            var teams = db.Teams.AsNoTracking().Where(x => x.Name.StartsWith(prefix));
            foreach (var team in teams)
            {
                Search search = new Search(team.Name, "Team", "Details", team.TeamId.ToString());
                result.Add(search);
            }

            var managers = db.Managers.AsNoTracking().Where(x => x.Name.Contains(prefix));
            foreach (var manager in managers)
            {
                Search search = new Search(manager.Name, "Manager", "Details", manager.ManagerId.ToString());
                result.Add(search);
            }

            var leagues = db.Leagues.AsNoTracking().Where(x => x.Name.StartsWith(prefix));
            foreach (var league in leagues)
            {
                Search search = new Search(league.Name, "League", "Details", league.LeagueId.ToString());
                result.Add(search);
            }

            var baseSearches = BaseSearches().Where(x => x.Label.ToUpper().StartsWith(prefix.ToUpper()));
            foreach (var baseSearch in baseSearches)
            {
                result.Add(baseSearch);
            }

            return result;
        }
    }
}