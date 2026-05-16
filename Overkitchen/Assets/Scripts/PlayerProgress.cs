using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LevelProgress
{
    public int levelId;
    public bool completed;
    public int bestScore;
    public int starsEarned;
    public int timesPlayed;
}

[System.Serializable]
public class PlayerData
{
    public int coins = 0;
    public int premiumCurrency = 0;
    public int lives = 5;
    public int totalStars = 0;
    public int highestLevelUnlocked = 1;
    public List<LevelProgress> levelProgress = new List<LevelProgress>();
    public List<string> unlockedAchievements = new List<string>();
}

public class PlayerProgress : MonoBehaviour
{
    public static PlayerProgress Instance { get; private set; }
    
    private PlayerData data;
    private const string SAVE_KEY = "OverKitchen_PlayerData";
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        LoadGame();
    }
    
    private void LoadGame()
    {
        string json = PlayerPrefs.GetString(SAVE_KEY, "");
        
        if (string.IsNullOrEmpty(json))
        {
            data = new PlayerData();
            data.coins = 1000; // Стартовая валюта
            data.lives = 5;
            data.highestLevelUnlocked = 1;
            SaveGame();
        }
        else
        {
            data = JsonUtility.FromJson<PlayerData>(json);
        }
    }
    
    public void SaveGame()
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }
    
    // ===== COINS & CURRENCY =====
    public int GetCoins() => data.coins;
    public int GetPremiumCurrency() => data.premiumCurrency;
    
    public void AddCoins(int amount)
    {
        data.coins += amount;
        SaveGame();
    }
    
    public bool SpendCoins(int amount)
    {
        if (data.coins >= amount)
        {
            data.coins -= amount;
            SaveGame();
            return true;
        }
        return false;
    }
    
    // ===== LIVES =====
    public int GetLives() => data.lives;
    
    public void AddLife()
    {
        data.lives++;
        SaveGame();
    }
    
    public bool SpendLife()
    {
        if (data.lives > 0)
        {
            data.lives--;
            SaveGame();
            return true;
        }
        return false;
    }
    
    // ===== LEVEL PROGRESS =====
    public LevelProgress GetLevelProgress(int levelId)
    {
        var progress = data.levelProgress.Find(p => p.levelId == levelId);
        if (progress == null)
        {
            progress = new LevelProgress { levelId = levelId, completed = false, bestScore = 0, starsEarned = 0, timesPlayed = 0 };
            data.levelProgress.Add(progress);
        }
        return progress;
    }
    
    public void CompleteLevelWithScore(int levelId, int finalScore)
    {
        LevelProgress progress = GetLevelProgress(levelId);
        
        if (finalScore > progress.bestScore)
            progress.bestScore = finalScore;
        
        // Получить количество звёзд
        LevelData levelData = LevelManager.Instance.GetLevel(levelId);
        int newStars = levelData.GetStarCount(finalScore);
        
        if (newStars > progress.starsEarned)
        {
            data.totalStars += (newStars - progress.starsEarned);
            progress.starsEarned = newStars;
        }
        
        progress.completed = true;
        progress.timesPlayed++;
        
        // Разблокируем следующий уровень
        if (levelId >= data.highestLevelUnlocked)
        {
            data.highestLevelUnlocked = levelId + 1;
        }
        
        SaveGame();
    }
    
    public int GetHighestUnlockedLevel() => data.highestLevelUnlocked;
    public int GetTotalStars() => data.totalStars;
    
    // ===== ACHIEVEMENTS =====
    public bool HasAchievement(string achievementId)
    {
        return data.unlockedAchievements.Contains(achievementId);
    }
    
    public void UnlockAchievement(string achievementId)
    {
        if (!data.unlockedAchievements.Contains(achievementId))
        {
            data.unlockedAchievements.Add(achievementId);
            SaveGame();
        }
    }
    
    public void ResetProgress() // Для дебага
    {
        data = new PlayerData();
        data.coins = 1000;
        data.lives = 5;
        SaveGame();
    }
}
