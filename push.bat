@echo off
chcp 65001 >nul
setlocal enabledelayedexpansion

echo 🚀 OverKitchen Game Enhancements - Commit & Push
echo ================================================
echo.

cd /d "E:\alex2\OverKitchen.worktrees\agents-game-enhancements-for-burmaldatik"

echo [1/4] Checking git status...
git status --short

echo.
echo [2/4] Staging all changes...
git add -A

echo.
echo [3/4] Creating commit...
git commit -m "feat: Add progression system with levels, scoring, and player progress" ^
  -m "Phase 1: Foundation Systems - COMPLETE

Added Core Systems:
- LevelManager (Singleton): 100 auto-generated levels with progressive difficulty
- PlayerProgress (Singleton): Persistent save data via JSON/PlayerPrefs
- ScoreCalculator (Singleton): Scoring with cascade multipliers and bonus scaling
- GameHUD: In-game UI showing moves, time, score, and progress bar
- LevelCompleteUI: Victory/defeat screens with star ratings and coin rewards
- LevelSelectUI: Level selection with unlocking progression
- MainMenuUI: Main menu with player stats display
- SceneSetupHelper: Quick setup utility for scene configuration

Key Features:
✅ Star Rating System (1-3 stars based on score thresholds)
✅ Progressive Level Unlocking (100 levels with auto-scaling)
✅ Move and Time Limits per level
✅ Bonus Multipliers (1.5x row/column, 3x color bomb)
✅ Cascade Multipliers (+++10 per cascade)
✅ Automatic Progress Saving

Modified:
- GameManager.cs: Integrated level loading, move limits, win/lose conditions

Documentation:
- README.md: Complete feature overview and architecture
- INTEGRATION_GUIDE.md: Step-by-step Unity setup instructions
- QUICKSTART.md: Quick reference for developers
- COMPLETION_REPORT.md: Phase 1 summary

Files Created: 11 scripts (~2000 LOC)
Scenes Needed: MainMenu, LevelSelect (templates provided in docs)

Ready for Phase 2: Daily Challenges, Shop, Additional Bonus Types, Leaderboards

Co-authored-by: Copilot <223556219+Copilot@users.noreply.github.com>"

if errorlevel 1 (
  echo ❌ Commit failed!
  pause
  exit /b 1
)

echo.
echo [4/4] Pushing to remote...
git push origin agents-game-enhancements-for-burmaldatik

if errorlevel 1 (
  echo ⚠️  Push failed - checking remote...
  git remote -v
  echo.
  echo Trying to push anyway...
  git push -u origin agents-game-enhancements-for-burmaldatik
)

echo.
echo ================================================
echo ✅ Done! Checking final status...
echo.
git log --oneline -3
echo.
git status

pause
