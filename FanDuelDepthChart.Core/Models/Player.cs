namespace FanDuelDepthChart.Core.Models
{
    public record Player(string Name, int Number)
    {
        public override string ToString() => $"(#{Number}, {Name})";
    }
}
