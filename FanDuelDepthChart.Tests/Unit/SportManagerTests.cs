using FanDuelDepthChart.Core.Constants;
using FanDuelDepthChart.Core.Services;

namespace FanDuelDepthChart.Tests.Unit
{
    [TestFixture]
    public class SportManagerTests
    {
        private SportManager _manager;
        private Sport _nfl;

        [SetUp]
        public void Setup()
        {
            _manager = new SportManager();
            _nfl = new Sport(SportTypes.NFL, NflPositions.All);
        }

        [Test]
        public void AddSport_NewSport_AddsSuccessfully()
        {
            _manager.AddSport(_nfl);
            var retrieved = _manager.GetSport(SportTypes.NFL);
            Assert.That(_nfl, Is.EqualTo(retrieved));
        }

        [Test]
        public void AddSport_DuplicateSport_ThrowsException()
        {
            _manager.AddSport(_nfl);
            Assert.Throws<InvalidOperationException>(() => _manager.AddSport(_nfl));
        }

        [Test]
        public void AddSport_SameNameDifferentObject_ThrowsException()
        {
            var anotherNfl = new Sport(SportTypes.NFL, []);
            _manager.AddSport(_nfl);
            Assert.Throws<InvalidOperationException>(() => _manager.AddSport(anotherNfl));
        }

        [Test]
        public void AddSport_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _manager.AddSport(null!));
        }

        [Test]
        public void GetSport_ReturnTheCorrectSport()
        {
            var nba = new Sport(SportTypes.NBA, []);
            _manager.AddSport(_nfl);
            _manager.AddSport(nba);
            var retrieved = _manager.GetSport(SportTypes.NFL);
            Assert.That(retrieved, Is.EqualTo(_nfl));
        }

        [Test]
        public void GetSport_NullOrEmptyName_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _manager.GetSport(null!));
            Assert.Throws<ArgumentException>(() => _manager.GetSport(string.Empty));
            Assert.Throws<ArgumentException>(() => _manager.GetSport("   "));
        }

        [Test]
        public void GetSport_NonExisting_ReturnsNull()
        {
            var result = _manager.GetSport("Invalid");
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetAllSports_ReturnsAllSports()
        {
            var nba = new Sport(SportTypes.NBA, []);
            _manager.AddSport(_nfl);
            _manager.AddSport(nba);

            var allSports = _manager.GetAllSports().ToList();
            Assert.That(allSports, Is.EquivalentTo(new List<Sport> { _nfl, nba }));
        }

        [Test]
        public void GetAllSports_WhenEmpty_ReturnsEmptyCollection()
        {
            var allSports = _manager.GetAllSports();
            Assert.That(allSports, Is.Empty);
        }
    }
}
