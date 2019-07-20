using DreamLeague.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace DreamLeague.DAL
{
    public class DreamLeagueContext : IdentityDbContext<ApplicationUser>
    {
        public DreamLeagueContext()
            : base("DreamLeagueContext", throwIfV1Schema: false)
        {
        }

        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<GameWeek> GameWeeks { get; set; }
        public virtual DbSet<League> Leagues { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<ManagerImage> ManagerImages { get; set; }
        public virtual DbSet<ManagerGoalKeeper> ManagerGoalKeepers { get; set; }
        public virtual DbSet<ManagerPlayer> ManagerPlayers { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Goal> Goals { get; set; }
        public virtual DbSet<Concede> Conceded { get; set; }
        public virtual DbSet<Meeting> Meetings { get; set; }
        public virtual DbSet<History> History { get; set; }
        public virtual DbSet<Cup> Cups { get; set; }
        public virtual DbSet<Fixture> Fixtures { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Audit> Audit { get; set; }

        public static DreamLeagueContext Create()
        {
            return new DreamLeagueContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Manager>()
                .HasOptional(a => a.Image)
                .WithRequired(x => x.Manager)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Manager>()
                .HasMany(c => c.Groups).WithMany(i => i.Managers)
                .Map(t => t.MapLeftKey("ManagerId")
                .MapRightKey("GroupId")
                .ToTable("ManagerGroups", "Cup"));
        }
    }
}