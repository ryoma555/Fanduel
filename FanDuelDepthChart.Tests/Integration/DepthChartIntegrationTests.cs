using FanDuelDepthChart.Core.Constants;
using FanDuelDepthChart.Core.Models;
using FanDuelDepthChart.Core.Services;

namespace FanDuelDepthChart.Tests.Integration
{
    [TestFixture]
    public class DepthChartIntegrationTests
    {
        private const string TeamName = "Team 1";

        private SportManager _sportManager;
        private Sport _sport;
        private Team _team;
        private Player _tom;
        private Player _john;
        private Player _mike;

        [SetUp]
        public void SetUp()
        {
            var validPositions = new HashSet<string> { 
                NflPositions.QB, 
                NflPositions.LS,
                NflPositions.RB,
            };
            var depthChart = new DepthChart(validPositions);
            _team = new Team(TeamName, depthChart);

            _sport = new Sport(SportTypes.NFL, validPositions);
            _sport.AddTeam(_team);

            _sportManager = new SportManager();
            _sportManager.AddSport(_sport);

            _tom = new Player("Tom Brady", 12);
            _john = new Player("John Cena", 11);
            _mike = new Player("Mike Tyson", 13);
        }

        [Test]
        public void AddPlayers_ShouldRespectDepthOrder()
        {
            _team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, _tom);
            _team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, _john);
            _team.DepthChart.AddPlayerToDepthChart(NflPositions.LS, _mike);

            var chartOutput = _team.DepthChart.GetFullDepthChart();

            Assert.That(chartOutput, Is.EqualTo("""
                QB – (#12, Tom Brady), (#11, John Cena)
                LS – (#13, Mike Tyson)
                """));
        }

        [Test]
        public void GetBackups_ShouldReturnCorrectPlayers()
        {
            _team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, _tom);
            _team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, _john);

            var backups = _team.DepthChart.GetBackups(NflPositions.QB, _tom);

            Assert.That(backups, Has.Count.EqualTo(1));
            Assert.That(backups[0], Is.EqualTo(_john));
        }

        [Test]
        public void RemovePlayer_ShouldUpdateDepthChartCorrectly()
        {
            _team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, _tom);
            _team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, _john);

            _team.DepthChart.RemovePlayerFromDepthChart(NflPositions.QB, _tom);
            var chartOutput = _team.DepthChart.GetFullDepthChart();

            Assert.That(chartOutput, Is.EqualTo("QB – (#11, John Cena)"));
        }

        [Test]
        public void GetFullDepthChart_ShouldReturnFormattedString()
        {
            _team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, _tom);
            _team.DepthChart.AddPlayerToDepthChart(NflPositions.LS, _mike);

            var output = _team.DepthChart.GetFullDepthChart();

            Assert.That(output, Is.EqualTo("""
                QB – (#12, Tom Brady)
                LS – (#13, Mike Tyson)
                """));
        }

        [Test]
        public void MultiPositionPlayer_ShouldBeIndependentAcrossPositions()
        {
            _team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, _tom);
            _team.DepthChart.AddPlayerToDepthChart(NflPositions.LS, _tom);

            _team.DepthChart.RemovePlayerFromDepthChart(NflPositions.QB, _tom);

            var chartOutput = _team.DepthChart.GetFullDepthChart();
            Assert.That(chartOutput, Is.EqualTo("LS – (#12, Tom Brady)"));
        }

        [Test]
        public void SportManager_ShouldRetrieveSportAndTeam()
        {
            var sport = _sportManager.GetSport(SportTypes.NFL);
            Assert.That(sport, Is.EqualTo(_sport));

            var team = sport!.GetTeam(TeamName);
            Assert.That(team, Is.EqualTo(_team));
        }

        [Test]
        public void EmptyDepthChart_ShouldReturnEmptyString()
        {
            var emptyDepthChart = new DepthChart(new HashSet<string> { NflPositions.QB });
            var output = emptyDepthChart.GetFullDepthChart();

            Assert.That(output, Is.EqualTo(string.Empty));
        }

        [Test]
        public void AddPlayerAtTopAndEnd_ShouldRespectDepth()
        {
            _team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, _john);
            _team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, _tom, 0);  // insert at top
            _team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, _mike, 100); // append at end

            var backups = _team.DepthChart.GetBackups(NflPositions.QB, _tom);
            Assert.That(backups, Is.EqualTo([_john, _mike]));

            var output = _team.DepthChart.GetFullDepthChart();
            Assert.That(output, Is.EqualTo("QB – (#12, Tom Brady), (#11, John Cena), (#13, Mike Tyson)"));
        }

        [Test]
        public void AddingPlayerWithInvalidPosition_ShouldThrow_WhenValidPositionsDefined()
        {
            Assert.Throws<ArgumentException>(() =>
                _team.DepthChart.AddPlayerToDepthChart("INVALID_POS", _tom));
        }

        [Test]
        public void AddingPlayer_ShouldSucceed_WhenValidPositionsIsEmptyOrNull()
        {
            var chartEmpty = new DepthChart(new HashSet<string> { });
            Assert.DoesNotThrow(() => chartEmpty.AddPlayerToDepthChart("ANY_POS", _tom));

            var chartNull = new DepthChart(null!);
            Assert.DoesNotThrow(() => chartNull.AddPlayerToDepthChart("ANY_POS", _tom));
        }

        [Test]
        public void GetBackups_ShouldReturnAllPlayersBelowTarget()
        {
            _team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, _tom);
            _team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, _john);
            _team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, _mike);

            var backups = _team.DepthChart.GetBackups(NflPositions.QB, _tom);
            Assert.That(backups, Is.EqualTo([_john, _mike]));

            backups = _team.DepthChart.GetBackups(NflPositions.QB, _john);
            Assert.That(backups, Is.EqualTo([_mike]));

            backups = _team.DepthChart.GetBackups(NflPositions.QB, _mike);
            Assert.That(backups, Is.Empty);
        }

        [Test]
        public void RemoveFromNonexistentPosition_ShouldReturnNull()
        {
            var removed = _team.DepthChart.RemovePlayerFromDepthChart(NflPositions.RB, _tom);
            Assert.That(removed, Is.Null);
        }

        [Test]
        public void GetBackups_WhenPlayerIsLast_ShouldReturnEmptyList()
        {
            _team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, _tom);
            var backups = _team.DepthChart.GetBackups(NflPositions.QB, _tom);
            Assert.That(backups, Is.Empty);
        }
    }
}