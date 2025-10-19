using FanDuelDepthChart.Core.Constants;
using FanDuelDepthChart.Core.Interfaces;
using FanDuelDepthChart.Core.Models;
using FanDuelDepthChart.Core.Services;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("FanDuel DepthChart Demo");

// Setup DI
var services = new ServiceCollection();

// Register SportManager as singleton
services.AddSingleton<ISportManager, SportManager>();

var provider = services.BuildServiceProvider();

var sportManager = provider.GetRequiredService<ISportManager>();
var nfl = new Sport(SportTypes.NFL, NflPositions.All);
sportManager.AddSport(nfl);

var teamAwesome = new Team("Team Awesome", new DepthChart(nfl.ValidPositions));
nfl.AddTeam(teamAwesome);

// Create players
var tom = new Player("Tom Brady", 12);
var john = new Player("John Cena", 11);
var mike = new Player("Mike Tyson", 13);
var travis = new Player("Travis Kelce", 87);
var taylor = new Player("Taylor Swift", 22);
var dominic = new Player("Dominic Toretto", 99);
var jabba = new Player("Jabba The Hutt", 66);

// Add players to multiple positions
teamAwesome.DepthChart.AddPlayerToDepthChart(NflPositions.QB, tom);
teamAwesome.DepthChart.AddPlayerToDepthChart(NflPositions.QB, john);
teamAwesome.DepthChart.AddPlayerToDepthChart(NflPositions.QB, dominic);

teamAwesome.DepthChart.AddPlayerToDepthChart(NflPositions.TE, travis);
teamAwesome.DepthChart.AddPlayerToDepthChart(NflPositions.TE, jabba);
teamAwesome.DepthChart.AddPlayerToDepthChart(NflPositions.TE, taylor);

teamAwesome.DepthChart.AddPlayerToDepthChart(NflPositions.RB, taylor);

teamAwesome.DepthChart.AddPlayerToDepthChart(NflPositions.LS, mike);
teamAwesome.DepthChart.AddPlayerToDepthChart(NflPositions.LS, travis);

// Display backups for a position
printBackups(NflPositions.QB, tom);

printBackups(NflPositions.TE, travis);

var removed = teamAwesome.DepthChart.RemovePlayerFromDepthChart(NflPositions.TE, jabba);
Console.WriteLine($"\nRemoved from TE: {removed}");

printBackups(NflPositions.TE, travis);

printBackups(NflPositions.RB, taylor);

printBackups(NflPositions.LS, mike);

// Display full depth chart
Console.WriteLine("\nFull Depth Chart:");
Console.WriteLine(teamAwesome.DepthChart.GetFullDepthChart());

void printBackups(string position, Player player)
{
    var backups = teamAwesome.DepthChart.GetBackups(position, player);
    Console.WriteLine($"\n{position} Backups for {player.Name}:");
    if (!backups.Any()) Console.WriteLine("None");
    else foreach (var b in backups) Console.WriteLine(b);
}