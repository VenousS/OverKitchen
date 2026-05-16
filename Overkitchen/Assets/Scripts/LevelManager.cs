using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    
    private Dictionary<int, LevelData> levels = new Dictionary<int, LevelData>();
    private int currentLevelId = 1;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        InitializeLevels();
    }
    
    private void InitializeLevels()
    {
        // Уровень 1: Tutorial - лёгкий
        LevelData level1 = new LevelData(1, "Начало");
        level1.moveLimit = 25;
        level1.scoreGoal = 20000;
        level1.starThreshold1 = 20000;
        level1.starThreshold2 = 30000;
        level1.starThreshold3 = 45000;
        level1.difficulty = 1;
        levels[1] = level1;
        
        // Уровень 2
        LevelData level2 = new LevelData(2, "Свежесть");
        level2.moveLimit = 22;
        level2.scoreGoal = 25000;
        level2.starThreshold1 = 25000;
        level2.starThreshold2 = 37500;
        level2.starThreshold3 = 50000;
        level2.difficulty = 2;
        levels[2] = level2;
        
        // Уровень 3: С препятствиями
        LevelData level3 = new LevelData(3, "Препятствия");
        level3.moveLimit = 20;
        level3.scoreGoal = 30000;
        level3.starThreshold1 = 30000;
        level3.starThreshold2 = 42500;
        level3.starThreshold3 = 55000;
        level3.difficulty = 3;
        level3.obstaclePositions.Add(new Vector2Int(3, 3));
        level3.obstaclePositions.Add(new Vector2Int(4, 4));
        levels[3] = level3;
        
        // Уровень 4
        LevelData level4 = new LevelData(4, "Сложность");
        level4.moveLimit = 18;
        level4.scoreGoal = 35000;
        level4.starThreshold1 = 35000;
        level4.starThreshold2 = 50000;
        level4.starThreshold3 = 65000;
        level4.difficulty = 4;
        levels[4] = level4;
        
        // Уровень 5+: Генерируем автоматически
        for (int i = 5; i <= 100; i++)
        {
            LevelData level = new LevelData(i, $"Уровень {i}");
            level.moveLimit = Mathf.Max(15, 25 - (i / 5));
            level.scoreGoal = 20000 + (i * 2500);
            level.difficulty = Mathf.Min(5, 1 + (i / 10));
            level.starThreshold1 = level.scoreGoal;
            level.starThreshold2 = (int)(level.scoreGoal * 1.4f);
            level.starThreshold3 = (int)(level.scoreGoal * 1.8f);
            levels[i] = level;
        }
    }
    
    public LevelData GetLevel(int levelId)
    {
        if (levels.ContainsKey(levelId))
            return levels[levelId];
        
        Debug.LogWarning($"Level {levelId} not found, returning default");
        return new LevelData(levelId);
    }
    
    public void SetCurrentLevel(int levelId)
    {
        if (levels.ContainsKey(levelId))
        {
            currentLevelId = levelId;
        }
    }
    
    public LevelData GetCurrentLevel()
    {
        return GetLevel(currentLevelId);
    }
    
    public int GetCurrentLevelId()
    {
        return currentLevelId;
    }
    
    public int GetTotalLevels()
    {
        return 100; // Можно расширять
    }
}
