# 🎮 OverKitchen - Match-3 Puzzle Game with Progression System

A fun and addictive Match-3 puzzle game built with Unity, featuring a complete progression system, level management, and player progression tracking.

## ✨ Features

### 🎯 Game Mechanics
- **Match-3 Puzzle System** - Classic match-3 gameplay with smooth animations
- **Bonus System** - Create special pieces (Row Bomb, Column Bomb, Color Bomb)
- **Cascading Matches** - Automatic chain reactions with score multipliers
- **Obstacles** - Dynamic obstacles that add challenge to levels

### 📈 Progression System
- **100+ Levels** - Each level with unique objectives and difficulty
- **Level Goals** - Score-based objectives with difficulty scaling
- **Move Limits** - Strategic gameplay with limited moves per level
- **Time Limits** - Optional time-based challenges (Optional)
- **Star Rating** - 1-3 star rating system based on score thresholds

### 💾 Player Progression
- **Persistent Save System** - Automatically save progress (PlayerPrefs)
- **Currency System** - Coins and premium currency
- **Level Unlocking** - Progressive level unlocking
- **Achievement Tracking** - Track completed achievements
- **Statistics** - Best scores, stars earned, levels completed

### 🎨 User Interface
- **Main Menu** - Entry point with stats display
- **Level Select** - Browse and select from available levels
- **In-Game HUD** - Real-time score, moves, time, and goal tracking
- **Level Complete Screen** - Victory/defeat with star rewards and coins
- **Progress Bar** - Visual feedback on goal progress

## 🛠️ Quick Setup

### Requirements
- Unity 2020.3+
- TextMesh Pro (built-in)

### Installation
1. Import the project into Unity
2. Open `INTEGRATION_GUIDE.md` for detailed setup instructions
3. Add the following scenes to Build Settings:
   - MainMenu
   - LevelSelect
   - SampleScene

### Quick Start
1. Add managers to your scene:
   ```csharp
   var levelManager = new GameObject("LevelManager").AddComponent<LevelManager>();
   var playerProgress = new GameObject("PlayerProgress").AddComponent<PlayerProgress>();
   var scoreCalculator = new GameObject("ScoreCalculator").AddComponent<ScoreCalculator>();
   ```

2. Add UI components to canvas and assign to GameHUD/LevelCompleteUI

3. Run the game!

## 📊 Level System

### Level Data Structure
```csharp
LevelData {
    levelId: int
    levelName: string
    gridSize: int (default 8)
    moveLimit: int (e.g., 20 moves)
    timeLimit: int (e.g., 180 seconds, -1 for unlimited)
    scoreGoal: int (e.g., 25000 points)
    difficulty: int (1-5)
    obstaclePositions: List<Vector2Int>
    starThresholds: [1★, 2★, 3★]
}
```

### Example Levels
- **Level 1**: 25 moves, 20K score goal → Learn the basics
- **Level 3**: 20 moves, 30K goal + 2 obstacles → Increased difficulty
- **Level 5+**: Auto-generated with progressive difficulty

## 🎮 Gameplay Loop

```
Main Menu
    ↓
Level Select
    ↓
Play Level
    ├─ Win (score ≥ goal) → Show stars + coins
    └─ Lose (moves = 0) → Offer retry
    ↓
Save Progress
```

## 📁 Project Structure

```
Assets/
├── Scripts/
│   ├── LevelData.cs          - Level configuration
│   ├── LevelManager.cs       - Level management (singleton)
│   ├── PlayerProgress.cs     - Player data & saving (singleton)
│   ├── ScoreCalculator.cs    - Scoring logic (singleton)
│   ├── GameManager.cs        - Main game loop (modified)
│   ├── PieceSystem.cs        - Piece behavior
│   ├── Obstacle.cs           - Obstacle behavior
│   ├── GameHUD.cs            - In-game UI
│   ├── LevelCompleteUI.cs    - Win/lose screen
│   ├── LevelSelectUI.cs      - Level selection
│   ├── MainMenuUI.cs         - Main menu
│   └── SceneSetupHelper.cs   - Setup utilities
├── Scenes/
│   ├── MainMenu.unity        - (Create this)
│   ├── LevelSelect.unity     - (Create this)
│   └── SampleScene.unity     - Game scene
└── Prefabs/
    ├── Piece variants (Tomato, Cheese, etc.)
    └── Obstacles
```

## 💡 Core Systems

### Scoring System
- **Match 3**: +100 base points
- **Match 4**: +300 base points
- **Match 5+**: +500 base points
- **Cascades**: +10% multiplier per cascade
- **Bonuses**: ×1.5 (row/column bomb), ×3 (color bomb)

```csharp
// Example: 4-match with cascade = (300 * 4 * 1.1) = 1320 points
```

### Save Format
Player progress is saved as JSON:
```json
{
  "coins": 1000,
  "premiumCurrency": 0,
  "lives": 5,
  "totalStars": 12,
  "highestLevelUnlocked": 5,
  "levelProgress": [
    {
      "levelId": 1,
      "completed": true,
      "bestScore": 45000,
      "starsEarned": 3,
      "timesPlayed": 2
    }
  ],
  "unlockedAchievements": []
}
```

## 🚀 Future Enhancements

- [x] Level system with progression
- [x] Player progress saving
- [x] Scoring system
- [ ] Daily Challenges with rewards
- [ ] Shop with boosters (extra moves, score multipliers)
- [ ] Additional bonus types (Lightning, Fireworks, UFO)
- [ ] Leaderboards (local/global)
- [ ] Achievement system with badges
- [ ] Sound and music
- [ ] Theme/world progression
- [ ] Special events

## 🐛 Known Issues

- Obstacles spawning needs manual prefab setup
- No sound effects yet
- Leaderboard not implemented
- No shop UI yet

## 📞 Support

For detailed integration steps, see `INTEGRATION_GUIDE.md`

## 📜 License

Part of the OverKitchen project

---

Happy coding! 🎮✨

**Version**: 1.0
**Last Updated**: 2026-05-16
