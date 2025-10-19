namespace FanDuelDepthChart.Core.Constants
{
    public static class NflPositions
    {
        // Offense
        public const string QB = "QB";
        public const string RB = "RB";
        public const string WR = "WR";
        public const string TE = "TE";
        public const string OT = "OT";
        public const string OG = "OG";
        public const string OC = "OC";

        // Defense
        public const string DE = "DE";
        public const string DT = "DT";
        public const string OLB = "OLB";
        public const string ILB = "ILB";
        public const string CB = "CB";
        public const string S = "S";

        // Special Teams
        public const string PK = "PK";
        public const string P = "P";
        public const string LS = "LS";
        public const string KR = "KR";
        public const string PR = "PR";

        public static readonly string[] All =
        {
            QB, RB, WR, TE, OT, OG, OC, DE, DT, OLB, ILB, CB, S, PK, P, LS, KR, PR
        };
    }
}
