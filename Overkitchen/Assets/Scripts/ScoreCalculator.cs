using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    public static ScoreCalculator Instance { get; private set; }
    
    // Базовые очки за разные комбинации
    [SerializeField] private int basePointsPer3Match = 100;
    [SerializeField] private int basePointsPer4Match = 300;
    [SerializeField] private int basePointsPer5Match = 500;
    
    // Множители за бонусы
    [SerializeField] private float bombRowMultiplier = 1.5f;
    [SerializeField] private float bombColumnMultiplier = 1.5f;
    [SerializeField] private float bombColorMultiplier = 3f;
    
    private int currentScore = 0;
    private int cascadeMultiplier = 1; // Каскадные матчи дают больше очков
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    public void ResetScore()
    {
        currentScore = 0;
        cascadeMultiplier = 1;
    }
    
    public int GetCurrentScore() => currentScore;
    
    public void CalculateScore(int matchCount, bool hasBonus = false, GameManager.BonusType bonusType = GameManager.BonusType.None)
    {
        int baseScore = 0;
        
        if (matchCount >= 5)
            baseScore = basePointsPer5Match;
        else if (matchCount == 4)
            baseScore = basePointsPer4Match;
        else if (matchCount >= 3)
            baseScore = basePointsPer3Match;
        
        float multiplier = cascadeMultiplier;
        
        if (hasBonus)
        {
            switch (bonusType)
            {
                case GameManager.BonusType.BombRow:
                    multiplier *= bombRowMultiplier;
                    break;
                case GameManager.BonusType.BombColumn:
                    multiplier *= bombColumnMultiplier;
                    break;
                case GameManager.BonusType.BombColor:
                    multiplier *= bombColorMultiplier;
                    break;
            }
        }
        
        int finalScore = (int)(baseScore * matchCount * multiplier);
        currentScore += finalScore;
        
        cascadeMultiplier += 0.1f; // Каждый каскад = ещё +10% к множителю
    }
    
    public void ResetCascadeMultiplier()
    {
        cascadeMultiplier = 1;
    }
}
