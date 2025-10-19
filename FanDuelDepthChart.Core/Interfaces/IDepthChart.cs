using FanDuelDepthChart.Core.Models;

namespace FanDuelDepthChart.Core.Interfaces
{
    public interface IDepthChart
    {
        void AddPlayerToDepthChart(string position, Player player, int? positionDepth = null);
        Player? RemovePlayerFromDepthChart(string position, Player player);
        IReadOnlyList<Player> GetBackups(string position, Player player);
        string GetFullDepthChart();
    }
}
