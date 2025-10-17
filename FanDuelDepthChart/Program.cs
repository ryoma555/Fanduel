using FanDuelDepthChart.Constants;
using FanDuelDepthChart.Core.Constants;
using FanDuelDepthChart.Core.Services;
using FanDuelDepthChart.Models;

Console.WriteLine("FanDuel DepthChart Demo");

// Create NFL sport
var nfl = new Sport(SportTypes.NFL, NflPositions.All);

// Create a team with a DepthChart
var team = new Team("Team Awesome", new DepthChart(nfl.ValidPositions));
nfl.AddTeam(team);

// Create players
var tom = new Player("Tom Brady", 12);
var john = new Player("John Cena", 11);
var mike = new Player("Mike Tyson", 13);
var travis = new Player("Travis Kelce", 87);
var taylor = new Player("Taylor Swift", 22);

// Add players to multiple positions
team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, tom);
team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, john);

team.DepthChart.AddPlayerToDepthChart(NflPositions.TE, travis);
team.DepthChart.AddPlayerToDepthChart(NflPositions.TE, taylor);

team.DepthChart.AddPlayerToDepthChart(NflPositions.RB, taylor);

team.DepthChart.AddPlayerToDepthChart(NflPositions.LS, mike);
team.DepthChart.AddPlayerToDepthChart(NflPositions.LS, travis);

// Display backups for a position
Console.WriteLine($"QB Backups for {tom.Name}:");
foreach (var backup in team.DepthChart.GetBackups(NflPositions.QB, tom))
{
    Console.WriteLine(backup);
}

Console.WriteLine($"TE Backups for {travis.Name}:");
foreach (var backup in team.DepthChart.GetBackups(NflPositions.TE, travis))
{
    Console.WriteLine(backup);
}

Console.WriteLine($"RB Backups for {taylor.Name}:");
foreach (var backup in team.DepthChart.GetBackups(NflPositions.RB, taylor))
{
    Console.WriteLine(backup);
}

Console.WriteLine($"LS Backups for {mike.Name}:");
foreach (var backup in team.DepthChart.GetBackups(NflPositions.LS, mike))
{
    Console.WriteLine(backup);
}

// Display full depth chart
Console.WriteLine("Full Depth Chart:");
Console.WriteLine(team.DepthChart.GetFullDepthChart());