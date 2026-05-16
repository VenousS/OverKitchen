# PowerShell 5.1 compatible script
Set-StrictMode -Off
$ErrorActionPreference = "Continue"

Push-Location "E:\alex2\OverKitchen.worktrees\agents-game-enhancements-for-burmaldatik"

# Stage all changes
Write-Host "Staging changes..."
& "git.exe" add -A

# Create commit
$commitMsg = @"
feat: Add progression system with levels, scoring, and player progress

Phase 1: Foundation Systems
- Level system with 100 auto-generated levels
- Player progress persistence (JSON/PlayerPrefs)
- Score calculation with multipliers and cascades
- Game HUD, level complete, level select UIs
- Main menu with player stats
- Integrated in GameManager

Features:
- Star rating (1-3 based on score)
- Progressive level unlocking
- Move/time limits
- Bonus multipliers
- Support for obstacles

Documentation added:
- README.md: Feature overview
- INTEGRATION_GUIDE.md: Setup instructions
- QUICKSTART.md: Developer reference

Co-authored-by: Copilot <223556219+Copilot@users.noreply.github.com>
"@

Write-Host "Creating commit..."
& "git.exe" commit -m $commitMsg

# Show result
Write-Host "`nCommit result:"
& "git.exe" log --oneline -1

Pop-Location
