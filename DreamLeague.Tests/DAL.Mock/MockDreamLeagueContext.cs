using DreamLeague.DAL;
using DreamLeague.Models;
using DreamLeague.Tests.Data.Mock;
using Moq;
using System.Data.Entity;

namespace DreamLeague.Tests.DAL.Mock
{
    public class MockDreamLeagueContext
    {
        public Mock<DreamLeagueContext> MockContext { get; set; }

        public virtual Mock<DbSet<Email>> MockEmails { get; set; }
        public virtual Mock<DbSet<GameWeek>> MockGameWeeks { get; set; }
        public virtual Mock<DbSet<League>> MockLeagues { get; set; }
        public virtual Mock<DbSet<Manager>> MockManagers { get; set; }
        public virtual Mock<DbSet<ManagerImage>> MockManagerImages { get; set; }
        public virtual Mock<DbSet<ManagerGoalKeeper>> MockManagerGoalKeepers { get; set; }
        public virtual Mock<DbSet<ManagerPlayer>> MockManagerPlayers { get; set; }
        public virtual Mock<DbSet<Player>> MockPlayers { get; set; }
        public virtual Mock<DbSet<Team>> MockTeams { get; set; }
        public virtual Mock<DbSet<Goal>> MockGoals { get; set; }
        public virtual Mock<DbSet<Concede>> MockConceded { get; set; }
        public virtual Mock<DbSet<Meeting>> MockMeetings { get; set; }
        public virtual Mock<DbSet<History>> MockHistory { get; set; }
        public virtual Mock<DbSet<Cup>> MockCups { get; set; }
        public virtual Mock<DbSet<Fixture>> MockFixtures { get; set; }
        public virtual Mock<DbSet<Group>> MockGroups { get; set; }
        public virtual Mock<DbSet<Audit>> MockAudit { get; set; }

        public MockDreamLeagueContext()
        {
            SetMocks();
        }

        private void SetMocks()
        {
            MockContext = new Mock<DreamLeagueContext>();

            MockEmails = new Mock<DbSet<Email>>().SetupData(EmailData.Data());
            MockContext.Setup(x => x.Emails).Returns(MockEmails.Object);

            MockGameWeeks = new Mock<DbSet<GameWeek>>().SetupData(GameWeekData.Data());
            MockContext.Setup(x => x.GameWeeks).Returns(MockGameWeeks.Object);

            MockLeagues = new Mock<DbSet<League>>().SetupData(LeagueData.Data());
            MockContext.Setup(x => x.Leagues).Returns(MockLeagues.Object);

            MockManagers = new Mock<DbSet<Manager>>().SetupData(ManagerData.Data());
            MockContext.Setup(x => x.Managers).Returns(MockManagers.Object);

            MockManagerImages = new Mock<DbSet<ManagerImage>>().SetupData(ManagerImageData.Data());
            MockContext.Setup(x => x.ManagerImages).Returns(MockManagerImages.Object);

            MockManagerGoalKeepers = new Mock<DbSet<ManagerGoalKeeper>>().SetupData(ManagerGoalKeeperData.Data());
            MockContext.Setup(x => x.ManagerGoalKeepers).Returns(MockManagerGoalKeepers.Object);

            MockManagerPlayers = new Mock<DbSet<ManagerPlayer>>().SetupData(ManagerPlayerData.Data());
            MockContext.Setup(x => x.ManagerPlayers).Returns(MockManagerPlayers.Object);

            MockPlayers = new Mock<DbSet<Player>>().SetupData(PlayerData.Data());
            MockContext.Setup(x => x.Players).Returns(MockPlayers.Object);

            MockTeams = new Mock<DbSet<Team>>().SetupData(TeamData.Data());
            MockContext.Setup(x => x.Teams).Returns(MockTeams.Object);

            MockGoals = new Mock<DbSet<Goal>>().SetupData(GoalData.Data());
            MockContext.Setup(x => x.Goals).Returns(MockGoals.Object);

            MockConceded = new Mock<DbSet<Concede>>().SetupData(ConcedeData.Data());
            MockContext.Setup(x => x.Conceded).Returns(MockConceded.Object);

            MockMeetings = new Mock<DbSet<Meeting>>().SetupData(MeetingData.Data());
            MockContext.Setup(x => x.Meetings).Returns(MockMeetings.Object);

            MockHistory = new Mock<DbSet<History>>().SetupData(HistoryData.Data());
            MockContext.Setup(x => x.History).Returns(MockHistory.Object);

            MockCups = new Mock<DbSet<Cup>>().SetupData(CupData.Data());
            MockContext.Setup(x => x.Cups).Returns(MockCups.Object);

            MockFixtures = new Mock<DbSet<Fixture>>().SetupData(FixtureData.Data());
            MockContext.Setup(x => x.Fixtures).Returns(MockFixtures.Object);

            MockGroups = new Mock<DbSet<Group>>().SetupData(GroupData.Data());
            MockContext.Setup(x => x.Groups).Returns(MockGroups.Object);

            MockAudit = new Mock<DbSet<Audit>>().SetupData(AuditData.Data());
            MockContext.Setup(x => x.Audit).Returns(MockAudit.Object);
        }
    }
}
