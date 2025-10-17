using FanDuelDepthChart.Constants;
using FanDuelDepthChart.Core.Services;
using FanDuelDepthChart.Models;

namespace FanDuelDepthChart.Tests.Integration
{
    [TestFixture]
    public class DepthChartTests
    {
        private DepthChart _depthChart;
        private Player _tomBrady;
        private Player _johnCena;
        private Player _mikeTyson;

        [SetUp]
        public void Setup()
        {
            var validPositions = new HashSet<string>(NflPositions.All);
            _depthChart = new DepthChart(validPositions);
            _tomBrady = new Player("Tom Brady", 1);
            _johnCena = new Player("John Cena", 2);
            _mikeTyson = new Player("Mike Tyson", 3);
        }

        [Test]
        public void AddPlayerToDepthChart_ShouldAddPlayer_WhenPositionIsNew()
        {
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _tomBrady);

            var backups = _depthChart.GetBackups(NflPositions.QB, _tomBrady);
            Assert.That(backups, Is.Empty);
        }

        [Test]
        public void AddPlayerToDepthChart_ShouldInsertPlayerAtSpecificDepth()
        {
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _tomBrady);
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _mikeTyson);
            // Insert John Cena at position 1 (between Tom Brady and Mike Tyson)
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _johnCena, 1);

            // Expect order: Tom Brady, John Cena, Mike Tyson, Mike got pushed to 3rd position
            var backups = _depthChart.GetBackups(NflPositions.QB, _tomBrady);
            Assert.That(backups.Count, Is.EqualTo(2));
            Assert.That(backups, Is.EqualTo(new List<Player> { _johnCena, _mikeTyson}));
        }

        [Test]
        public void AddPlayerToDepthChart_ShouldInsertSamePlayer_WithDifferentPositions()
        {
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _tomBrady);
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _mikeTyson);

            _depthChart.AddPlayerToDepthChart(NflPositions.LS, _tomBrady);
            _depthChart.AddPlayerToDepthChart(NflPositions.LS, _johnCena);

            var backups = _depthChart.GetBackups(NflPositions.QB, _tomBrady);
            Assert.That(backups.Count, Is.EqualTo(1));
            Assert.That(backups[0], Is.EqualTo(_mikeTyson));

            backups = _depthChart.GetBackups(NflPositions.LS, _tomBrady);
            Assert.That(backups.Count, Is.EqualTo(1));
            Assert.That(backups[0], Is.EqualTo(_johnCena));
        }

        [Test]
        public void AddPlayerToDepthChart_ShouldThrow_WhenPositionIsNull()
        {
            Assert.Throws<ArgumentException>(() =>
                _depthChart.AddPlayerToDepthChart(null!, _tomBrady));
        }

        [Test]
        public void AddPlayerToDepthChart_ShouldAppend_WhenPositionDepthExceedsCount()
        {
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _tomBrady);
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _johnCena, 10); // index > count

            var backups = _depthChart.GetBackups(NflPositions.QB, _tomBrady);
            Assert.That(backups, Is.EqualTo(new List<Player> { _johnCena }));
        }

        [Test]
        public void AddPlayerToDepthChart_ShouldThrow_WhenDuplicatePlayerAdded()
        {
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _tomBrady);
            Assert.Throws<InvalidOperationException>(() =>
                _depthChart.AddPlayerToDepthChart(NflPositions.QB, _tomBrady));
        }

        [Test]
        public void AddPlayerToDepthChart_ShouldThrow_WhenPositionIndexIsNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                _depthChart.AddPlayerToDepthChart(NflPositions.QB, _tomBrady, -4));
        }

        [Test]
        public void AddPlayerToDepthChart_LargeIndex_AppendsToEnd()
        {
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _tomBrady);
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _johnCena);
            // large index should append to end
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _mikeTyson, 100);

            var backups = _depthChart.GetBackups(NflPositions.QB, _tomBrady);
            Assert.That(backups, Is.EqualTo(new List<Player> { _johnCena, _mikeTyson }));
        }

        [Test]
        public void RemovePlayerFromDepthChart_ShouldReturnRemovedPlayer_WhenExists()
        {
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _johnCena);
            var removed = _depthChart.RemovePlayerFromDepthChart(NflPositions.QB, _johnCena);

            Assert.That(removed, Is.EqualTo(_johnCena));
        }

        [Test]
        public void RemovePlayerFromDepthChart_ShouldActuallyRemovePlayer()
        {
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _tomBrady);
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _johnCena);

            var removed = _depthChart.RemovePlayerFromDepthChart(NflPositions.QB, _tomBrady);
            Assert.That(removed, Is.EqualTo(_tomBrady));

            // Tom Brady removed — John Cena should now be first and have no backups
            var backups = _depthChart.GetBackups(NflPositions.QB, _johnCena);
            Assert.That(backups, Is.Empty);
        }

        [Test]
        public void RemovePlayerFromDepthChart_ShouldReturnNull_WhenPlayerNotFound()
        {
            var removed = _depthChart.RemovePlayerFromDepthChart(NflPositions.QB, _tomBrady);
            Assert.That(removed, Is.Null);
        }

        [Test]
        public void GetBackups_ShouldReturnAllPlayersBelowGivenPlayer()
        {
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _tomBrady);
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _johnCena);
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _mikeTyson);

            var backups = _depthChart.GetBackups(NflPositions.QB, _tomBrady);

            Assert.That(backups.Count, Is.EqualTo(2));
            Assert.That(backups, Is.EqualTo(new List<Player> { _johnCena, _mikeTyson }));
        }

        [Test]
        public void GetBackups_ShouldReturnEmpty_WhenPlayerIsLastInDepth()
        {
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _tomBrady);
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _johnCena);

            var backups = _depthChart.GetBackups(NflPositions.QB, _johnCena);
            Assert.That(backups, Is.Empty);
        }

        [Test]
        public void GetBackups_ShouldReturnEmpty_WhenPositionDoesNotExist()
        {
            var backups = _depthChart.GetBackups(NflPositions.QB, _tomBrady);
            Assert.That(backups, Is.Empty);
        }

        [Test]
        public void GetBackups_ShouldReturnEmpty_WhenPlayerDoesNotExist()
        {
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _tomBrady);

            var backups = _depthChart.GetBackups(NflPositions.QB, _johnCena);
            Assert.That(backups, Is.Empty);
        }

        [Test]
        public void GetFullDepthChart_ShouldReturnExpectedString()
        {
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _tomBrady);
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _johnCena);
            _depthChart.AddPlayerToDepthChart(NflPositions.QB, _mikeTyson);

            _depthChart.AddPlayerToDepthChart(NflPositions.OC, _mikeTyson);
            _depthChart.AddPlayerToDepthChart(NflPositions.OC, _tomBrady);
            _depthChart.AddPlayerToDepthChart(NflPositions.OC, _johnCena);

            var result = _depthChart.GetFullDepthChart();

            var expected = """
                QB – (#1, Tom Brady), (#2, John Cena), (#3, Mike Tyson)
                OC – (#3, Mike Tyson), (#1, Tom Brady), (#2, John Cena)
                """;
            Assert.That(result.Trim(), Is.EqualTo(expected.Trim()));
        }

        [Test]
        public void ValidatePosition_ShouldSkipCheck_WhenValidPositionsIsEmpty()
        {
            var depthChart = new DepthChart([]);

            Assert.DoesNotThrow(() =>
                depthChart.AddPlayerToDepthChart("RANDOM_POSITION", _tomBrady));
        }

        [Test]
        public void ValidatePosition_ShouldSkipCheck_WhenValidPositionsIsNull()
        {
            var depthChart = new DepthChart(null!);
            
            Assert.DoesNotThrow(() =>
                depthChart.AddPlayerToDepthChart("RANDOM_POSITION", _tomBrady));
        }

        [Test]
        public void AddPlayerToDepthChart_ShouldThrow_WhenInvalidArguments()
        {
            Assert.Throws<ArgumentException>(() =>
                _depthChart.AddPlayerToDepthChart("", _tomBrady));

            Assert.Throws<ArgumentNullException>(() =>
                _depthChart.AddPlayerToDepthChart(NflPositions.QB, null!));

            Assert.Throws<ArgumentException>(() =>
                _depthChart.AddPlayerToDepthChart("RANDOM_POSITION", _tomBrady));
        }

        [Test]
        public void RemovePlayerToDepthChart_ShouldThrow_WhenInvalidArguments()
        {
            Assert.Throws<ArgumentException>(() =>
                _depthChart.RemovePlayerFromDepthChart("", _tomBrady));

            Assert.Throws<ArgumentNullException>(() =>
                _depthChart.RemovePlayerFromDepthChart(NflPositions.QB, null!));

            Assert.Throws<ArgumentException>(() =>
                _depthChart.RemovePlayerFromDepthChart("RANDOM_POSITION", _tomBrady));
        }

        [Test]
        public void GetBackups_ShouldThrow_WhenInvalidArguments()
        {
            Assert.Throws<ArgumentException>(() =>
                _depthChart.GetBackups("", _tomBrady));

            Assert.Throws<ArgumentNullException>(() =>
                _depthChart.GetBackups(NflPositions.QB, null!));

            Assert.Throws<ArgumentException>(() =>
                _depthChart.GetBackups("RANDOM_POSITION", _tomBrady));
        }
    }
}
