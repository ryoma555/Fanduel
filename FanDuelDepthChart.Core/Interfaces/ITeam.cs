namespace FanDuelDepthChart.Core.Interfaces
{
    public interface ITeam
    {
        string Name { get; }
        IDepthChart DepthChart { get; }
    }
}
