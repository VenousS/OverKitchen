using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelGoal
{
    public enum GoalType { Score, Elements, Time }
    public GoalType type;
    public int target; // целевое значение
    public string description; // "Собрать 50 помидоров"
}

[System.Serializable]
public class LevelData
{
    public int levelId;
    public string levelName;
    public int gridSize = 8;
    public int moveLimit = 20;
    public int timeLimit = -1; // -1 = без лимита времени
    public int scoreGoal = 25000;
    public int difficulty = 1; // 1-5
    
    public List<LevelGoal> goals = new List<LevelGoal>();
    public List<Vector2Int> obstaclePositions = new List<Vector2Int>();
    
    // 3 star thresholds (например 25k, 35k, 50k)
    public int starThreshold1 = 25000;
    public int starThreshold2 = 35000;
    public int starThreshold3 = 50000;
    
    public LevelData(int id, string name = "Level")
    {
        levelId = id;
        levelName = name;
        
        // Задаём goal по умолчанию
        LevelGoal scoreGoal = new LevelGoal();
        scoreGoal.type = LevelGoal.GoalType.Score;
        scoreGoal.target = this.scoreGoal;
        scoreGoal.description = $"Набери {this.scoreGoal} очков";
        goals.Add(scoreGoal);
    }
    
    public int GetStarCount(int finalScore)
    {
        if (finalScore >= starThreshold3) return 3;
        if (finalScore >= starThreshold2) return 2;
        if (finalScore >= starThreshold1) return 1;
        return 0;
    }
}
