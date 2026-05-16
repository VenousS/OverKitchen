using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI movesText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI goalText;
    [SerializeField] private Image progressBar; // Визуальный прогресс к цели
    
    [SerializeField] private Color goalReachedColor = Color.green;
    [SerializeField] private Color normalColor = Color.white;
    
    private LevelData currentLevel;
    
    private void Start()
    {
        currentLevel = LevelManager.Instance.GetCurrentLevel();
        
        if (currentLevel != null && goalText != null)
            goalText.text = currentLevel.GetPrimaryGoal()?.description ?? "Цель: ---";
    }
    
    public void UpdateHUD(int moves, int timeLeft, int score, int lives, string goalProgressText = null)
    {
        if (currentLevel == null)
            currentLevel = LevelManager.Instance.GetCurrentLevel();

        if (scoreText != null)
        {
            scoreText.text = $"Очки: {score}";
            
            // Меняем цвет, если достигли цели
            if (currentLevel != null && score >= currentLevel.scoreGoal)
                scoreText.color = goalReachedColor;
            else
                scoreText.color = normalColor;
        }
        
        if (movesText != null)
            movesText.text = $"Ходы: {moves}";
        
        if (timerText != null)
        {
            if (timeLeft > 0)
            {
                int minutes = timeLeft / 60;
                int seconds = timeLeft % 60;
                timerText.text = $"Время: {minutes}:{seconds:D2}";
            }
            else
                timerText.text = "Время: ∞";
        }

        if (livesText != null)
        {
            livesText.text = $"Жизни: {lives}";
        }

        if (goalText != null)
        {
            goalText.text = !string.IsNullOrEmpty(goalProgressText) ? goalProgressText : currentLevel?.GetPrimaryGoal()?.description ?? "Цель: ---";
        }
        
        // Обновляем прогресс-бар
        if (progressBar != null && currentLevel != null)
        {
            float progress = currentLevel.scoreGoal > 0 ? Mathf.Clamp01((float)score / currentLevel.scoreGoal) : 0f;
            progressBar.fillAmount = progress;
        }
    }
}
