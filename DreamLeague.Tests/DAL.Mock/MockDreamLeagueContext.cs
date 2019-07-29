using DreamLeague.DAL;
using DreamLeague.Models;
using DreamLeague.Tests.Data.Mock;
using Moq;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DreamLeague.Tests.DAL.Mock
{
    public class MockDreamLeagueContext
    {
        public Mock<IDreamLeagueContext> MockContext { get; set; }

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

        public MockDreamLeagueContext(bool setMocks = true)
        {
            if (setMocks)
            {
                SetMocks();
            }
        }

        private void SetMocks()
        {
            SetMockContext();
            SetMockEmails();
            SetMockGameWeeks();
            SetMockLeagues();
            SetMockManagers();
            SetMockManagerImages();
            SetMockManagerGoalKeepers();
            SetMockManagerPlayers();
            SetMockPlayers();
            SetMockTeams();
            SetMockGoals();
            SetMockConceded();
            SetMockMeetings();
            SetMockHistory();
            SetMockCups();
            SetMockFixtures();
            SetMockGroups();
            SetMockAudit();
        }

        private void SetMockAudit()
        {
            MockAudit = new Mock<DbSet<Audit>>().SetupData(AuditData.Data());
            MockAudit.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(ids => AuditData.Data().FirstOrDefault(d => d.AuditId == (int)ids[0]));
            MockAudit.Setup(x => x.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids => Task.FromResult(AuditData.Data().FirstOrDefault(d => d.AuditId == (int)ids[0])));
            MockContext.Setup(x => x.Audit).Returns(MockAudit.Object);
        }

        private void SetMockGroups()
        {
            MockGroups = new Mock<DbSet<Group>>().SetupData(GroupData.Data());
            MockGroups.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(ids => GroupData.Data().FirstOrDefault(d => d.GroupId == (int)ids[0]));
            MockGroups.Setup(x => x.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids => Task.FromResult(GroupData.Data().FirstOrDefault(d => d.GroupId == (int)ids[0])));
            MockContext.Setup(x => x.Groups).Returns(MockGroups.Object);
        }

        private void SetMockFixtures()
        {
            MockFixtures = new Mock<DbSet<Fixture>>().SetupData(FixtureData.Data());
            MockFixtures.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(ids => FixtureData.Data().FirstOrDefault(d => d.FixtureId == (int)ids[0]));
            MockFixtures.Setup(x => x.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids => Task.FromResult(FixtureData.Data().FirstOrDefault(d => d.FixtureId == (int)ids[0])));
            MockContext.Setup(x => x.Fixtures).Returns(MockFixtures.Object);
        }

        private void SetMockCups()
        {
            MockCups = new Mock<DbSet<Cup>>().SetupData(CupData.Data());
            MockCups.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(ids => CupData.Data().FirstOrDefault(d => d.CupId == (int)ids[0]));
            MockCups.Setup(x => x.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids => Task.FromResult(CupData.Data().FirstOrDefault(d => d.CupId == (int)ids[0])));
            MockContext.Setup(x => x.Cups).Returns(MockCups.Object);
        }

        private void SetMockHistory()
        {
            MockHistory = new Mock<DbSet<History>>().SetupData(HistoryData.Data());
            MockHistory.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(ids => HistoryData.Data().FirstOrDefault(d => d.HistoryId == (int)ids[0]));
            MockHistory.Setup(x => x.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids => Task.FromResult(HistoryData.Data().FirstOrDefault(d => d.HistoryId == (int)ids[0])));
            MockContext.Setup(x => x.History).Returns(MockHistory.Object);
        }

        private void SetMockMeetings()
        {
            MockMeetings = new Mock<DbSet<Meeting>>().SetupData(MeetingData.Data());
            MockMeetings.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(ids => MeetingData.Data().FirstOrDefault(d => d.MeetingId == (int)ids[0]));
            MockMeetings.Setup(x => x.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids => Task.FromResult(MeetingData.Data().FirstOrDefault(d => d.MeetingId == (int)ids[0])));
            MockContext.Setup(x => x.Meetings).Returns(MockMeetings.Object);
        }

        private void SetMockConceded()
        {
            MockConceded = new Mock<DbSet<Concede>>().SetupData(ConcedeData.Data());
            MockConceded.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(ids => ConcedeData.Data().FirstOrDefault(d => d.ConcedeId == (int)ids[0]));
            MockConceded.Setup(x => x.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids => Task.FromResult(ConcedeData.Data().FirstOrDefault(d => d.ConcedeId == (int)ids[0])));
            MockContext.Setup(x => x.Conceded).Returns(MockConceded.Object);
        }

        private void SetMockGoals()
        {
            MockGoals = new Mock<DbSet<Goal>>().SetupData(GoalData.Data());
            MockGoals.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(ids => GoalData.Data().FirstOrDefault(d => d.GoalId == (int)ids[0]));
            MockGoals.Setup(x => x.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids => Task.FromResult(GoalData.Data().FirstOrDefault(d => d.GoalId == (int)ids[0])));
            MockContext.Setup(x => x.Goals).Returns(MockGoals.Object);
        }

        private void SetMockTeams()
        {
            MockTeams = new Mock<DbSet<Team>>().SetupData(TeamData.Data());
            MockTeams.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(ids => TeamData.Data().FirstOrDefault(d => d.TeamId == (int)ids[0]));
            MockTeams.Setup(x => x.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids => Task.FromResult(TeamData.Data().FirstOrDefault(d => d.TeamId == (int)ids[0])));
            MockContext.Setup(x => x.Teams).Returns(MockTeams.Object);
        }

        private void SetMockPlayers()
        {
            MockPlayers = new Mock<DbSet<Player>>().SetupData(PlayerData.Data());
            MockPlayers.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(ids => PlayerData.Data().FirstOrDefault(d => d.PlayerId == (int)ids[0]));
            MockPlayers.Setup(x => x.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids => Task.FromResult(PlayerData.Data().FirstOrDefault(d => d.PlayerId == (int)ids[0])));
            MockContext.Setup(x => x.Players).Returns(MockPlayers.Object);
        }

        private void SetMockManagerPlayers()
        {
            MockManagerPlayers = new Mock<DbSet<ManagerPlayer>>().SetupData(ManagerPlayerData.Data());
            MockManagerPlayers.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(ids => ManagerPlayerData.Data().FirstOrDefault(d => d.ManagerPlayerId == (int)ids[0]));
            MockManagerPlayers.Setup(x => x.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids => Task.FromResult(ManagerPlayerData.Data().FirstOrDefault(d => d.ManagerPlayerId == (int)ids[0])));
            MockContext.Setup(x => x.ManagerPlayers).Returns(MockManagerPlayers.Object);
        }

        private void SetMockManagerGoalKeepers()
        {
            MockManagerGoalKeepers = new Mock<DbSet<ManagerGoalKeeper>>().SetupData(ManagerGoalKeeperData.Data());
            MockManagerGoalKeepers.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(ids => ManagerGoalKeeperData.Data().FirstOrDefault(d => d.ManagerGoalKeeperId == (int)ids[0]));
            MockManagerGoalKeepers.Setup(x => x.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids => Task.FromResult(ManagerGoalKeeperData.Data().FirstOrDefault(d => d.ManagerGoalKeeperId == (int)ids[0])));
            MockContext.Setup(x => x.ManagerGoalKeepers).Returns(MockManagerGoalKeepers.Object);
        }

        private void SetMockManagerImages()
        {
            MockManagerImages = new Mock<DbSet<ManagerImage>>().SetupData(ManagerImageData.Data());
            MockManagerImages.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(ids => ManagerImageData.Data().FirstOrDefault(d => d.ManagerId == (int)ids[0]));
            MockManagerImages.Setup(x => x.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids => Task.FromResult(ManagerImageData.Data().FirstOrDefault(d => d.ManagerId == (int)ids[0])));
            MockContext.Setup(x => x.ManagerImages).Returns(MockManagerImages.Object);
        }

        private void SetMockManagers()
        {
            MockManagers = new Mock<DbSet<Manager>>().SetupData(ManagerData.Data());
            MockManagers.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(ids => ManagerData.Data().FirstOrDefault(d => d.ManagerId == (int)ids[0]));
            MockManagers.Setup(x => x.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids => Task.FromResult(ManagerData.Data().FirstOrDefault(d => d.ManagerId == (int)ids[0])));
            MockContext.Setup(x => x.Managers).Returns(MockManagers.Object);
        }

        private void SetMockLeagues()
        {
            MockLeagues = new Mock<DbSet<League>>().SetupData(LeagueData.Data());
            MockLeagues.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(ids => LeagueData.Data().FirstOrDefault(d => d.LeagueId == (int)ids[0]));
            MockLeagues.Setup(x => x.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids => Task.FromResult(LeagueData.Data().FirstOrDefault(d => d.LeagueId == (int)ids[0])));
            MockContext.Setup(x => x.Leagues).Returns(MockLeagues.Object);
        }

        private void SetMockGameWeeks()
        {
            MockGameWeeks = new Mock<DbSet<GameWeek>>().SetupData(GameWeekData.Data());
            MockGameWeeks.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(ids => GameWeekData.Data().FirstOrDefault(d => d.GameWeekId == (int)ids[0]));
            MockGameWeeks.Setup(x => x.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids => Task.FromResult(GameWeekData.Data().FirstOrDefault(d => d.GameWeekId == (int)ids[0])));
            MockContext.Setup(x => x.GameWeeks).Returns(MockGameWeeks.Object);
        }

        private void SetMockEmails()
        {
            MockEmails = new Mock<DbSet<Email>>().SetupData(EmailData.Data());
            MockEmails.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(ids => EmailData.Data().FirstOrDefault(d => d.EmailId == (int)ids[0]));
            MockEmails.Setup(x => x.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids => Task.FromResult(EmailData.Data().FirstOrDefault(d => d.EmailId == (int)ids[0])));
            MockContext.Setup(x => x.Emails).Returns(MockEmails.Object);
        }

        private void SetMockContext()
        {
            MockContext = new Mock<IDreamLeagueContext>();
        }
    }
}
