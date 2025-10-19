using FanDuelDepthChart.Core.Constants;
using FanDuelDepthChart.Core.Interfaces;
using FanDuelDepthChart.Core.Services;
using Moq;

namespace FanDuelDepthChart.Tests.Unit
{
    [TestFixture]
    public class SportTests
    {
        private Sport _sport;
        private Mock<ITeam> _mockTeam;

        [SetUp]
        public void Setup()
        {
            _sport = new Sport(SportTypes.NFL, NflPositions.All);
            _mockTeam = new Mock<ITeam>();
            _mockTeam.Setup(t => t.Name).Returns("Team 1");
        }

        [Test]
        public void AddTeam_NewTeam_AddsSuccessfully()
        {
            _sport.AddTeam(_mockTeam.Object);
            var retrieved = _sport.GetTeam("Team 1");
            Assert.That(retrieved, Is.EqualTo(_mockTeam.Object));
        }

        [Test]
        public void AddTeam_DuplicateTeam_ThrowsException()
        {
            _sport.AddTeam(_mockTeam.Object);
            Assert.Throws<InvalidOperationException>(() => _sport.AddTeam(_mockTeam.Object));
        }

        [Test]
        public void AddTeam_SameNameDifferentObject_ThrowsException()
        {
            _sport.AddTeam(_mockTeam.Object);
            var anotherMock = new Mock<ITeam>();
            anotherMock.Setup(t => t.Name).Returns("Team 1");

            Assert.Throws<InvalidOperationException>(() => _sport.AddTeam(anotherMock.Object));
        }

        [Test]
        public void AddTeam_NullTeam_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _sport.AddTeam(null!));
        }

        [Test]
        public void AddTeam_EmptyTeamName_ThrowsArgumentException()
        {
            var badTeam = new Mock<ITeam>();
            badTeam.Setup(t => t.Name).Returns(string.Empty);
            Assert.Throws<ArgumentException>(() => _sport.AddTeam(badTeam.Object));
        }

        [Test]
        public void GetTeam_NullOrEmptyName_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sport.GetTeam(null!));
            Assert.Throws<ArgumentException>(() => _sport.GetTeam(string.Empty));
            Assert.Throws<ArgumentException>(() => _sport.GetTeam("   "));
        }

        [Test]
        public void GetAllTeams_ReturnsAllTeams()
        {
            var mockTeam2 = new Mock<ITeam>();
            mockTeam2.Setup(t => t.Name).Returns("Team 2");

            _sport.AddTeam(_mockTeam.Object);
            _sport.AddTeam(mockTeam2.Object);

            var teams = _sport.GetAllTeams().ToList();
            Assert.That(teams.Count, Is.EqualTo(2));
            Assert.That(teams, Is.EqualTo([_mockTeam.Object, mockTeam2.Object]));
        }

        [Test]
        public void GetAllTeams_Empty_ReturnsEmptyCollection()
        {
            var teams = _sport.GetAllTeams().ToList();
            Assert.That(teams, Is.Empty);
        }

        [Test]
        public void GetTeam_NonExisting_ReturnsNull()
        {
            var retrieved = _sport.GetTeam("NonExistent");
            Assert.That(retrieved, Is.Null);
        }

        [Test]
        public void GetTeam_IsCaseSensitive_ByDefault()
        {
            _sport.AddTeam(_mockTeam.Object);
            // default dictionary is case-sensitive: "team 1" should not match "Team 1"
            var lower = _sport.GetTeam("team 1");
            Assert.That(lower, Is.Null);
        }

        [Test]
        public void Sport_AllowsCreation_WhenValidPositionsNullOrEmpty()
        {
            var sportNull = new Sport("TestSport", null!);
            var sportEmpty = new Sport("TestSportEmpty", []);

            Assert.DoesNotThrow(() => sportNull.AddTeam(_mockTeam.Object));
            Assert.DoesNotThrow(() => sportEmpty.AddTeam(_mockTeam.Object));
        }
    }
}
