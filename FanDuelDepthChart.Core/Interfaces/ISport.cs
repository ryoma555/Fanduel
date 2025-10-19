namespace FanDuelDepthChart.Core.Interfaces
{
    public interface ISport
    {
        string Name { get; }
        IReadOnlySet<string> ValidPositions { get; }
        void AddTeam(ITeam team);
        ITeam? GetTeam(string name);
        IReadOnlyCollection<ITeam> GetAllTeams();
    }
}
