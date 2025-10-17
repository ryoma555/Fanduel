namespace FanDuelDepthChart.Models
{
    public record Player
    {
        public string Name { get; init; }
        public int Number { get; init; }
        public Player(string name, int number)
        {
            Name = name;
            Number = number;
        }
        public override string ToString()
        {
            return $"(#{Number}, {Name})";
        }
    }
}
