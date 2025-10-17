using FanDuelDepthChart.Core.Interfaces;

namespace FanDuelDepthChart.Core.Services
{
    public class Team : ITeam
    {
        public string Name { get; }
        public IDepthChart DepthChart { get; }

        public Team(string teamName, IDepthChart depthChart)
        {
            if (string.IsNullOrWhiteSpace(teamName))
                throw new ArgumentException("Team name must be provided.", nameof(teamName));

            DepthChart = depthChart ?? throw new ArgumentNullException(nameof(depthChart));
            Name = teamName;
        }
    }
}
