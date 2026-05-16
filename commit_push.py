#!/usr/bin/env python3
import subprocess
import os
import sys

os.chdir(r"E:\alex2\OverKitchen.worktrees\agents-game-enhancements-for-burmaldatik")

print("🚀 OverKitchen Game Enhancements - Commit & Push")
print("=" * 60)
print()

try:
    print("[1/4] Checking git status...")
    result = subprocess.run(["git", "status", "--short"], capture_output=True, text=True)
    print(result.stdout)
    
    print("\n[2/4] Staging all changes...")
    subprocess.run(["git", "add", "-A"], check=True)
    
    print("\n[3/4] Creating commit...")
    
    commit_msg = """feat: Add progression system with levels, scoring, and player progress

Phase 1: Foundation Systems - COMPLETE

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
✅ Cascade Multipliers (+10%% per cascade)
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

Co-authored-by: Copilot <223556219+Copilot@users.noreply.github.com>"""
    
    subprocess.run(["git", "commit", "-m", commit_msg], check=True)
    
    print("\n[4/4] Pushing to remote...")
    
    try:
        subprocess.run(["git", "push", "origin", "agents-game-enhancements-for-burmaldatik"], check=True)
    except subprocess.CalledProcessError:
        print("⚠️  Push failed - trying with upstream...")
        subprocess.run(["git", "push", "-u", "origin", "agents-game-enhancements-for-burmaldatik"], check=True)
    
    print("\n" + "=" * 60)
    print("✅ SUCCESS! Commit and push completed!")
    print("=" * 60)
    print("\nLatest commits:")
    subprocess.run(["git", "log", "--oneline", "-3"])
    
    print("\nGit status:")
    subprocess.run(["git", "status"])
    
except subprocess.CalledProcessError as e:
    print(f"\n❌ Error: {e}")
    sys.exit(1)
except Exception as e:
    print(f"\n❌ Unexpected error: {e}")
    sys.exit(1)
