using FanDuelDepthChart.Core.Interfaces;

namespace FanDuelDepthChart.Core.Services
{
    public class SportManager : ISportManager
    {
        private readonly Dictionary<string, ISport> _sports = [];

        public void AddSport(ISport sport)
        {
            if (sport is null)
                throw new ArgumentNullException(nameof(sport));
            if (_sports.ContainsKey(sport.Name))
                throw new InvalidOperationException($"Sport '{sport.Name}' already exists.");

            _sports[sport.Name] = sport;
        }

        public ISport? GetSport(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name must be provided.", nameof(name));

            return _sports.GetValueOrDefault(name);
        }

        public IEnumerable<ISport> GetAllSports() => _sports.Values;
    }
}
