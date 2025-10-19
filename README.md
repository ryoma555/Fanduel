# 🏈 FanDuel Depth Chart

This solution provides a flexible and scalable framework for managing depth charts across multiple sports and teams. New sports, positions, and teams can be added dynamically without modifying existing code. Each team maintains its own depth chart, supporting multiple positions per player. Comprehensive unit and integration tests cover all edge cases, ensuring correctness and preventing regressions. The code is modular, well-organized, and easy to maintain, with clear separation of concerns and dependency injection for maximum flexibility.

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
FanDuel
|-- FanDuelDepthChart
|   |-- Program.cs
|
|-- FanDuelDepthChart.Core
|   |-- Constants
|   |   |-- NflPositions.cs
|   |   |-- SportTypes.cs
|   |
|   |-- Interfaces
|   |   |-- IDepthChart.cs
|   |   |-- ISport.cs
|   |   |-- ISportManager.cs
|   |   |-- ITeam.cs
|   |
|   |-- Models
|   |   |-- Player.cs
|   |
|   |-- Services
|       |-- DepthChart.cs
|       |-- Sport.cs
|       |-- SportManager.cs
|       |-- Team.cs
|
|-- FanDuelDepthChart.Tests
|   |-- Integration
|   |   |-- DepthChartIntegrationTests.cs
|   |
|   |-- Unit
|       |-- DepthChartTests.cs
|       |-- SportManagerTests.cs
|       |-- SportTests.cs
|       |-- TeamTests.cs
|
|-- README.md
|-- .git
|-- .gitignore
```

---

## ⚙️ Technologies & Versions

- **Language**: C# .Net 9
- **Testing Framework**: NUnit 4.4.0
- **NUnit Analyzer**: NUnit.Analyzers 4.10.0
- **NUnit3 Test Adapter**: 5.2.0
- **Mocking Library**: Moq 4.20.72

---

## 🧠 Core Concepts

### Depth Chart
Handles ordered player management per position.

Key methods:
- `AddPlayerToDepthChart(string position, Player player, int? positionDepth = null)`
- `RemovePlayerFromDepthChart(string position, Player player)`
- `GetBackups(string position, Player player)`
- `GetFullDepthChart()`

Supports:
- Duplicate player prevention
- Multi-position players
- Automatic insertion at specified depth or appending at end
- Empty position handling in full depth chart

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

## 🚀 Running the Program

```bash
dotnet run --project FanDuelDepthChart
```

---

## 🧪 Running the Tests

```bash
dotnet test
```

---

## 🧰 Example Usage

```csharp
var nfl = new Sport(SportTypes.NFL, NflPositions.All);
var team = new Team("Team 1", new DepthChart(nfl.ValidPositions));
nfl.AddTeam(team);

var tom = new Player("Tom Brady", 12);
var john = new Player("John Cena", 11);

team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, tom);
team.DepthChart.AddPlayerToDepthChart(NflPositions.QB, john);

var backups = team.DepthChart.GetBackups(NflPositions.QB, tom);
Console.WriteLine(team.DepthChart.GetFullDepthChart());
```

---

### 🔮 Future Enhancements
- Add support for more sports like MLB, NHL, and NBA.
- Add all teams dynamically using configuration files or a database.
- Build a web API for interactive depth chart management.
- This will require thread-safety enhancements (e.g., locking or concurrent collections) to handle simultaneous updates to the same team or position safely.
- Persist depth charts to a storage layer (e.g., SQL, NoSQL, or in-memory cache) for long-term tracking and analytics.