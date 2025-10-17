using FanDuelDepthChart.Core.Interfaces;
using FanDuelDepthChart.Models;
using System.Text;

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

            if (!_chart.TryGetValue(position, out var players))
                return null;

            int index = players.FindIndex(p => p.Number == player.Number);
            if (index < 0)
                return null;

            var removed = players[index];
            players.RemoveAt(index);
            return removed;
        }

        public List<Player> GetBackups(string position, Player player)
        {
            ValidatePositionAndPlayer(position, player);

            if (!_chart.TryGetValue(position, out var players))
                return [];

            int index = players.FindIndex(p => p.Number == player.Number);

            // If player not found or is the last in the list, return empty list
            if (index == -1 || index == players.Count - 1) return [];

            return players.GetRange(index + 1, players.Count - (index + 1));
        }

        public string GetFullDepthChart()
        {
            var sb = new StringBuilder();

            foreach (var kvp in _chart)
            {
                string position = kvp.Key;
                string playersStr = string.Join(", ", kvp.Value.Select(p => p.ToString()));
                sb.AppendLine($"{position} – {playersStr}");
            }

            string result = sb.ToString().TrimEnd();

            Console.WriteLine(result); // prints for console demo

            return result;
        }

        private void ValidatePositionAndPlayer(string position, Player player)
        {
            if (string.IsNullOrWhiteSpace(position))
                throw new ArgumentException("Position must be provided.", nameof(position));

            // only check valid positions if provided
            if (_validPositions != null && _validPositions.Count > 0 && !_validPositions.Contains(position))
                throw new ArgumentException($"Invalid position '{position}' for this sport.", nameof(position));

            if (player is null)
                throw new ArgumentNullException(nameof(player));
        }
    }
}
