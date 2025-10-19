using FanDuelDepthChart.Core.Interfaces;
using FanDuelDepthChart.Core.Models;

namespace FanDuelDepthChart.Core.Services
{
    public class DepthChart : IDepthChart
    {
        private readonly Dictionary<string, List<Player>> _chart = [];
        private readonly HashSet<string> _validPositions;

        public DepthChart(HashSet<string> validPositions)
        {
            _validPositions = validPositions ?? [];
        }

        public void AddPlayerToDepthChart(string position, Player player, int? positionDepth = null)
        {
            ValidatePositionAndPlayer(position, player);

            if (positionDepth < 0)
                throw new ArgumentOutOfRangeException(nameof(positionDepth), "Position depth cannot be negative.");

            // Initialize position if it doesn't exist
            if (!_chart.TryGetValue(position, out var players))
            {
                players = [];
                _chart[position] = players;
            }

            // Prevent adding duplicate players
            if (players.Any(p => p.Number == player.Number))
                throw new InvalidOperationException($"Player #{player.Number} already exists at position '{position}'.");

            // Ensure positionDepth is within bounds
            if (!positionDepth.HasValue || positionDepth >= players.Count)
            {
                players.Add(player);
            }
            else
            {
                players.Insert(positionDepth.Value, player);
            }
        }

        public Player? RemovePlayerFromDepthChart(string position, Player player)
        {
            ValidatePositionAndPlayer(position, player);

            // Return null if position does not exist
            if (!_chart.TryGetValue(position, out var players))
                return null;

            int index = players.FindIndex(p => p == player);
            // Return null if player not found
            if (index < 0)
                return null;

            var removed = players[index];
            players.RemoveAt(index);
            return removed;
        }

        public IReadOnlyList<Player> GetBackups(string position, Player player)
        {
            ValidatePositionAndPlayer(position, player);

            // Return empty list if position does not exist
            if (!_chart.TryGetValue(position, out var players))
                return [];

            int index = players.FindIndex(p => p == player);

            // If player not found or is the last in the list, return empty list
            if (index == -1 || index == players.Count - 1) return [];

            return players[(index + 1)..];
        }

        public string GetFullDepthChart()
        {
            return string.Join(Environment.NewLine,
                _chart
                    .Where(kvp => kvp.Value.Count > 0)
                    .Select(kvp => $"{kvp.Key} – {string.Join(", ", kvp.Value)}"));
        }

        private void ValidatePositionAndPlayer(string position, Player player)
        {
            if (string.IsNullOrWhiteSpace(position))
                throw new ArgumentException("Position must be provided.", nameof(position));

            // only check valid positions if provided
            if (_validPositions != null && _validPositions.Count > 0 && !_validPositions.Contains(position))
                throw new ArgumentException($"Invalid position '{position}' for this sport.", nameof(position));

            ArgumentNullException.ThrowIfNull(player);
        }
    }
}
