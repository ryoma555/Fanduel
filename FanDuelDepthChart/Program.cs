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

// Register Sport/s
services.AddSingleton<ISport>(provider =>
{
    var sportManager = provider.GetRequiredService<ISportManager>();

    var nfl = new Sport(SportTypes.NFL, NflPositions.All);
    sportManager.AddSport(nfl);

    return nfl;
});

// Register Team/s
services.AddSingleton<ITeam>(provider =>
{
    var allSports = provider.GetServices<ISport>();
    var nfl = allSports.First(s => s.Name == SportTypes.NFL);

    var depthChart = new DepthChart(nfl.ValidPositions);
    var team = new Team("Team Awesome", depthChart);
    nfl.AddTeam(team);

    return team;
});

// Build the provider
var provider = services.BuildServiceProvider();

// Resolve services
var sportManager = provider.GetRequiredService<ISportManager>();
var allSports = provider.GetServices<ISport>();
var nfl = allSports.Single(s => s.Name == SportTypes.NFL);

// Resolve Teams
var allTeams = provider.GetServices<ITeam>().ToList();
var teamAwesome = allTeams.Single(t => t.Name == "Team Awesome");

// Create players
var tom = new Player("Tom Brady", 12);
var john = new Player("John Cena", 11);
var mike = new Player("Mike Tyson", 13);
var travis = new Player("Travis Kelce", 87);
var taylor = new Player("Taylor Swift", 22);

// Add players to multiple positions
teamAwesome.DepthChart.AddPlayerToDepthChart(NflPositions.QB, tom);
teamAwesome.DepthChart.AddPlayerToDepthChart(NflPositions.QB, john);

teamAwesome.DepthChart.AddPlayerToDepthChart(NflPositions.TE, travis);
teamAwesome.DepthChart.AddPlayerToDepthChart(NflPositions.TE, taylor);

teamAwesome.DepthChart.AddPlayerToDepthChart(NflPositions.RB, taylor);

teamAwesome.DepthChart.AddPlayerToDepthChart(NflPositions.LS, mike);
teamAwesome.DepthChart.AddPlayerToDepthChart(NflPositions.LS, travis);

// Display backups for a position
Console.WriteLine($"\nQB Backups for {tom.Name}:");
foreach (var backup in teamAwesome.DepthChart.GetBackups(NflPositions.QB, tom))
{
    Console.WriteLine(backup);
}

Console.WriteLine($"\nTE Backups for {travis.Name}:");
foreach (var backup in teamAwesome.DepthChart.GetBackups(NflPositions.TE, travis))
{
    Console.WriteLine(backup);
}

Console.WriteLine($"\nRB Backups for {taylor.Name}:");
foreach (var backup in teamAwesome.DepthChart.GetBackups(NflPositions.RB, taylor))
{
    Console.WriteLine(backup);
}

Console.WriteLine($"\nLS Backups for {mike.Name}:");
foreach (var backup in teamAwesome.DepthChart.GetBackups(NflPositions.LS, mike))
{
    Console.WriteLine(backup);
}

// Display full depth chart
Console.WriteLine("\nFull Depth Chart:");
Console.WriteLine(teamAwesome.DepthChart.GetFullDepthChart());