namespace FanDuelDepthChart.Core.Interfaces
{
    public interface ISportManager
    {
        void AddSport(ISport sport);
        ISport? GetSport(string name);
        IReadOnlyCollection<ISport> GetAllSports();
    }
}
