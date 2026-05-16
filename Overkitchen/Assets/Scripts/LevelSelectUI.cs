using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private Transform levelButtonContainer;
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Button backButton;
    
    [SerializeField] private Image levelPreview;
    [SerializeField] private TextMeshProUGUI levelNameText;
    [SerializeField] private TextMeshProUGUI levelInfoText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    
    private void Start()
    {
        backButton?.onClick.AddListener(GoBack);
        GenerateLevelButtons();
    }
    
    private void GenerateLevelButtons()
    {
        int maxLevel = LevelManager.Instance.GetTotalLevels();
        int unlockedLevel = PlayerProgress.Instance.GetHighestUnlockedLevel();
        
        for (int i = 1; i <= maxLevel && i <= 20; i++) // Показываем первые 20 уровней
        {
            GameObject btn = Instantiate(levelButtonPrefab, levelButtonContainer);
            Button button = btn.GetComponent<Button>();
            TextMeshProUGUI buttonText = btn.GetComponentInChildren<TextMeshProUGUI>();
            Image stars = btn.transform.Find("Stars")?.GetComponent<Image>();
            
            int levelId = i;
            LevelData levelData = LevelManager.Instance.GetLevel(levelId);
            LevelProgress progress = PlayerProgress.Instance.GetLevelProgress(levelId);
            
            buttonText.text = $"Уровень {levelId}";
            
            if (i <= unlockedLevel)
            {
                button.interactable = true;
                button.onClick.AddListener(() => SelectLevel(levelId));
                
                if (progress.completed && stars != null)
                {
                    // Показываем звёзды
                    string starsStr = new string('⭐', progress.starsEarned);
                    stars.GetComponent<TextMeshProUGUI>().text = starsStr;
                }
            }
            else
            {
                button.interactable = false;
                buttonText.text += " 🔒";
            }
        }
    }
    
    private void SelectLevel(int levelId)
    {
        LevelData level = LevelManager.Instance.GetLevel(levelId);
        LevelProgress progress = PlayerProgress.Instance.GetLevelProgress(levelId);
        
        levelNameText.text = level.levelName;
        levelInfoText.text = $"Ходы: {level.moveLimit} | Цель: {level.scoreGoal} очков | Сложность: {level.difficulty}";
        bestScoreText.text = progress.completed ? $"Лучший результат: {progress.bestScore}" : "Не пройден";
        
        LevelManager.Instance.SetCurrentLevel(levelId);
    }
    
    private void GoBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
