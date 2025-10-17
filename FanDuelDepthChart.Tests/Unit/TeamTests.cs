using FanDuelDepthChart.Constants;
using FanDuelDepthChart.Core.Interfaces;
using FanDuelDepthChart.Core.Services;
using FanDuelDepthChart.Models;
using Moq;

namespace FanDuelDepthChart.Tests.Unit
{
    [TestFixture]
    public class TeamTests
    {
        private Mock<IDepthChart> _mockDepthChart = null!;
        private Team _team = null!;

        [SetUp]
        public void Setup()
        {
            _mockDepthChart = new Mock<IDepthChart>();
            _team = new Team("Team 1", _mockDepthChart.Object);
        }

        [Test]
        public void TeamName_IsSetCorrectly()
        {
            Assert.That(_team.Name, Is.EqualTo("Team 1"));
        }

        [Test]
        public void DepthChart_PropertyPreservesInstance()
        {
            Assert.That(_team.DepthChart, Is.SameAs(_mockDepthChart.Object));
        }

        [Test]
        public void DepthChart_CallsAddPlayer()
        {
            var player = new Player("Tom Brady", 1);
            _team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, player, 0);

            _mockDepthChart.Verify(d => d.AddPlayerToDepthChart(NflPositions.QB, player, 0), Times.Once);
        }

        [Test]
        public void DepthChart_CallsRemovePlayer()
        {
            var player = new Player("John Cena", 2);
            _team.DepthChart.RemovePlayerFromDepthChart(NflPositions.QB, player);

            _mockDepthChart.Verify(d => d.RemovePlayerFromDepthChart(NflPositions.QB, player), Times.Once);
        }

        [Test]
        public void DepthChart_CallsGetBackups()
        {
            var player = new Player("Mike Tyson", 3);
            _team.DepthChart.GetBackups(NflPositions.QB, player);
            _mockDepthChart.Verify(d => d.GetBackups(NflPositions.QB, player), Times.Once);
        }

        [Test]
        public void DepthChart_CallsGetFullDepthChart()
        {
            _team.DepthChart.GetFullDepthChart();
            _mockDepthChart.Verify(d => d.GetFullDepthChart(), Times.Once);
        }

        [Test]
        public void Constructor_NullTeamName_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Team(null!, _mockDepthChart.Object));
        }

        [Test]
        public void Constructor_EmptyOrWhitespaceTeamName_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Team(string.Empty, _mockDepthChart.Object));
            Assert.Throws<ArgumentException>(() => new Team("   ", _mockDepthChart.Object));
        }

        [Test]
        public void Constructor_NullDepthChart_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Team("Team 1", null!));
        }
    }
}
