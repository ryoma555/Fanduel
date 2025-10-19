using FanDuelDepthChart.Core.Interfaces;

namespace FanDuelDepthChart.Core.Services
{
    public class Sport : ISport
    {
        public string Name { get; }
        public IReadOnlySet<string> ValidPositions { get; }
        private readonly Dictionary<string, ITeam> _teams = [];

        public Sport(string name, IEnumerable<string> validPositions)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Sport name must be provided.", nameof(name));

            Name = name;
            ValidPositions = new HashSet<string>(validPositions ?? Enumerable.Empty<string>());
        }

        public void AddTeam(ITeam team)
        {
            ArgumentNullException.ThrowIfNull(team);

            if (string.IsNullOrWhiteSpace(team.Name))
                throw new ArgumentException("Team must have a non-empty name.", nameof(team));

            if (_teams.ContainsKey(team.Name))
                throw new InvalidOperationException($"Team '{team.Name}' already exists.");

            _teams[team.Name] = team;
        }

        public ITeam? GetTeam(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name must be provided.", nameof(name));

            return _teams.GetValueOrDefault(name);
        }

        public IReadOnlyCollection<ITeam> GetAllTeams() => _teams.Values;
    }
}
