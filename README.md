# FanDuel Depth Chart

A modular C# solution to manage and visualize team depth charts for different sports (e.g., NFL).  
The system allows adding, removing, and retrieving players per position, supports multiple sports and teams, and includes both unit and integration tests.

---

## 🧩 Project Overview

This project models a simplified version of a sports **depth chart manager**.

### Key Features
- **Sport Management**: Supports multiple sports (e.g., NFL, NBA).
- **Team Management**: Each sport can have one or more teams.
- **Depth Chart**: Each team maintains an ordered depth chart of players per position.
- **Flexible Validation**: Position validation is configurable (can be skipped if no valid positions are defined).
- **Test Coverage**: Unit tests for all core services and integration tests for end-to-end validation.

---

## 🏗 Folder Structure

```
Fanduel
├── FanduelDepthChart
│   └── Program.cs
│
├── FanduelDepthChart.Core
│   ├── Constants
│   │   ├── NflPositions.cs
│   │   └── SportTypes.cs
│   │
│   ├── Interfaces
│   │   ├── IDepthChart.cs
│   │   ├── ISport.cs
│   │   ├── ISportManager.cs
│   │   └── ITeam.cs
│   │
│   ├── Models
│   │   └── Player.cs
│   │
│   └── Services
│       ├── DepthChart.cs
│       ├── Sport.cs
│       ├── SportManager.cs
│       └── Team.cs
│
├── FanduelDepthChart.Tests
│   ├── Integration
│   │   └── DepthChartIntegrationTests.cs
│   │
│   └── Unit
│       ├── DepthChartTests.cs
│       ├── SportManagerTests.cs
│       ├── SportTests.cs
│       └── TeamTests.cs
│
├── README.md
└── .git
```

---

## ⚙️ Technologies & Versions

- **Language**: C# (.NET 8 or later)
- **Testing Framework**: NUnit 4.4.0
- **NUnit Analyzer**: NUnit.Analyzers 4.10.0
- **NUnit3 Test Adapter**: 5.2.0
- **Mocking Library**: Moq 4.20.72

---

## 🧠 Core Concepts

### Player Model
A lightweight immutable record representing a player.
```csharp
public record Player(string Name, int Number)
{
    public override string ToString() => $"#{Number}, {Name}";
}
```

### Depth Chart
Handles ordered player management per position.

Key methods:
- `AddPlayerToDepthChart(string position, Player player, int? positionDepth = null)`
- `RemovePlayerFromDepthChart(string position, Player player)`
- `GetBackups(string position, Player player)`
- `GetFullDepthChart()`

### Sport and Team
- **Sport**: Manages valid positions and multiple teams.
- **Team**: Owns a depth chart for managing its players.

### Sport Manager
Central registry for all sports:
```csharp
var manager = new SportManager();
manager.AddSport(new Sport(SportTypes.NFL, NflPositions.All));
```

---

## 🧪 Running Tests

### Run all tests:
```bash
dotnet test
```

### Run a specific test category:
```bash
dotnet test --filter TestCategory=Integration
```

---

## 🧰 Example Usage

```csharp
var nfl = new Sport("NFL", NflPositions.All);
var team = new Team("Tampa Bay Buccaneers", new DepthChart(nfl.ValidPositions));
nfl.AddTeam(team);

var tom = new Player("Tom Brady", 12);
var john = new Player("John Cena", 11);

team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, tom);
team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, john);

var backups = team.DepthChart.GetBackups(NflPositions.QB, tom);
Console.WriteLine(team.DepthChart.GetFullDepthChart());
```

---
