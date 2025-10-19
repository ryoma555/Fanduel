namespace FanDuelDepthChart.Core.Interfaces
{
    public interface ISport
    {
        string Name { get; }
        HashSet<string> ValidPositions { get; }
        void AddTeam(ITeam team);
        ITeam? GetTeam(string name);
        IReadOnlyCollection<ITeam> GetAllTeams();
    }
}
