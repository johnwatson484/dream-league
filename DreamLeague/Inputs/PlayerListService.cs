using DreamLeague.DAL;
using DreamLeague.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DreamLeague.Inputs
{
    public class PlayerListService : IPlayerListService
    {
        readonly DreamLeagueContext db;
        readonly IPlayerListReader playerListReader;
        readonly string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");
        readonly List<PlayerListPlayer> unmatched = new List<PlayerListPlayer>();

        public PlayerListService(DreamLeagueContext db, IPlayerListReader playerListReader)
        {
            this.db = db;
            this.playerListReader = playerListReader;
        }

        public string Upload(HttpPostedFileBase file)
        {
            string filePath = Path.Combine(path, string.Format("PlayersList_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss")));

            file.SaveAs(filePath);

            DeleteCurrent();

            Load(playerListReader.Read(filePath));

            File.Delete(filePath);

            if (unmatched.Count == 0)
            {
                db.SaveChanges();
                return "Success";
            }
            else
            {
                return Unmatched();
            }
        }

        private void DeleteCurrent()
        {
            var players = db.Players;
            foreach (var player in players)
            {
                db.Players.Remove(player);
            }
            db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('League.Players', RESEED, 0)");
        }

        private void Load(PlayerList playerList)
        {
            foreach (var player in playerList.Players)
            {
                if (player.PositionUpdated != null)
                {
                    int teamId = db.Teams.AsNoTracking().Where(x => x.Alias == player.Team).Select(x => x.TeamId).FirstOrDefault();

                    if (teamId == 0)
                    {
                        unmatched.Add(player);
                    }
                    else
                    {
                        Player newPlayer = new Player(player.FirstNameUpdated, player.LastNameUpdated, player.PositionUpdated.Value, teamId);
                        db.Players.Add(newPlayer);
                    }
                }
            }
        }

        private string Unmatched()
        {
            var unmatchedTeams = unmatched.GroupBy(x => x.Team).ToList();

            StringBuilder sb = new StringBuilder();

            sb.Append("The following teams were not matched: ");

            foreach (var team in unmatchedTeams)
            {
                sb.Append(string.Format(" - {0}", team.Key));
            }

            return sb.ToString();
        }
    }
}