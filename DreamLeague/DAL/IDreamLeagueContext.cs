using System.Data.Entity;
using System.Threading.Tasks;
using DreamLeague.Models;

namespace DreamLeague.DAL
{
    public interface IDreamLeagueContext
    {
        DbSet<Audit> Audit { get; set; }
        DbSet<Concede> Conceded { get; set; }
        DbSet<Cup> Cups { get; set; }
        DbSet<Email> Emails { get; set; }
        DbSet<Fixture> Fixtures { get; set; }
        DbSet<GameWeek> GameWeeks { get; set; }
        DbSet<Goal> Goals { get; set; }
        DbSet<Group> Groups { get; set; }
        DbSet<History> History { get; set; }
        DbSet<League> Leagues { get; set; }
        DbSet<ManagerGoalKeeper> ManagerGoalKeepers { get; set; }
        DbSet<ManagerImage> ManagerImages { get; set; }
        DbSet<ManagerPlayer> ManagerPlayers { get; set; }
        DbSet<Manager> Managers { get; set; }
        DbSet<Meeting> Meetings { get; set; }
        DbSet<Player> Players { get; set; }
        DbSet<Team> Teams { get; set; }

        Database Database { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void Dispose();
        void SetModified(object entity);
    }
}