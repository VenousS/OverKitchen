using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button achievementsButton;
    [SerializeField] private Button shopButton;
    
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI starsText;
    [SerializeField] private TextMeshProUGUI levelText;
    
    private void Start()
    {
        // Убеждаемся, что сессия инициализирована
        if (LevelManager.Instance == null)
        {
            GameObject gm = new GameObject("LevelManager");
            gm.AddComponent<LevelManager>();
            DontDestroyOnLoad(gm);
        }
        if (PlayerProgress.Instance == null)
        {
            GameObject gp = new GameObject("PlayerProgress");
            gp.AddComponent<PlayerProgress>();
            DontDestroyOnLoad(gp);
        }
        if (ScoreCalculator.Instance == null)
        {
            GameObject sc = new GameObject("ScoreCalculator");
            sc.AddComponent<ScoreCalculator>();
            DontDestroyOnLoad(sc);
        }
        
        playButton?.onClick.AddListener(PlayGame);
        settingsButton?.onClick.AddListener(OpenSettings);
        achievementsButton?.onClick.AddListener(OpenAchievements);
        shopButton?.onClick.AddListener(OpenShop);
        
        UpdateUI();
    }
    
    private void Update()
    {
        // Обновляем UI каждый кадр (для динамических изменений)
        if (coinsText != null)
            coinsText.text = $"💰 {PlayerProgress.Instance.GetCoins()}";
        
        if (starsText != null)
            starsText.text = $"⭐ {PlayerProgress.Instance.GetTotalStars()}";
        
        if (levelText != null)
            levelText.text = $"Уровень: {PlayerProgress.Instance.GetHighestUnlockedLevel()}";
    }
    
    private void UpdateUI()
    {
        Update();
    }
    
    private void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelect");
    }
    
    private void OpenSettings()
    {
        Debug.Log("Settings opened - TODO");
    }
    
    private void OpenAchievements()
    {
        Debug.Log("Achievements opened - TODO");
    }
    
    private void OpenShop()
    {
        Debug.Log("Shop opened - TODO");
    }
}
