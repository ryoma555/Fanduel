namespace FanDuelDepthChart.Core.Models
{
    public record Player
    {
        public string Name { get; init; }
        public int Number { get; init; }

        public Player(string name, int number)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Player name cannot be null or empty.", nameof(name));

            if (number < 0)
                throw new ArgumentOutOfRangeException(nameof(number), "Player number cannot be negative.");
            
            Name = name;
            Number = number;
        }
        public override string ToString() => $"(#{Number}, {Name})";
    }
}
