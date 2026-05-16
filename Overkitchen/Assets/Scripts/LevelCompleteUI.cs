using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LevelCompleteUI : MonoBehaviour
{
    [SerializeField] private GameObject panelWin;
    [SerializeField] private GameObject panelLose;
    
    [SerializeField] private TextMeshProUGUI scoreTextWin;
    [SerializeField] private TextMeshProUGUI starsTextWin;
    [SerializeField] private Image[] starImages; // 3 звёздочки
    [SerializeField] private TextMeshProUGUI coinsRewardText;
    
    [SerializeField] private TextMeshProUGUI scoreTextLose;
    [SerializeField] private TextMeshProUGUI goalTextLose;
    
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;
    
    [SerializeField] private Color starOnColor = Color.yellow;
    [SerializeField] private Color starOffColor = Color.gray;
    
    private void Start()
    {
        if (panelWin != null) panelWin.SetActive(false);
        if (panelLose != null) panelLose.SetActive(false);
        
        nextLevelButton?.onClick.AddListener(NextLevel);
        retryButton?.onClick.AddListener(RetryLevel);
        mainMenuButton?.onClick.AddListener(MainMenu);
    }
    
    public void ShowWin(int finalScore, int stars)
    {
        StartCoroutine(ShowWinCoroutine(finalScore, stars));
    }
    
    private IEnumerator ShowWinCoroutine(int finalScore, int stars)
    {
        yield return new WaitForSeconds(0.5f);
        
        if (panelWin != null)
        {
            panelWin.SetActive(true);
            
            if (scoreTextWin != null)
                scoreTextWin.text = $"Очки: {finalScore}";
            
            if (starsTextWin != null)
                starsTextWin.text = $"Звёзды: {stars}/3";
            
            // Показываем звёзды анимацией
            for (int i = 0; i < starImages.Length; i++)
            {
                if (i < stars)
                    starImages[i].color = starOnColor;
                else
                    starImages[i].color = starOffColor;
                
                yield return new WaitForSeconds(0.3f);
            }
            
            // Рассчитываем награду (1 звезда = 100 монет)
            int coinsEarned = stars * 100 + (finalScore / 1000) * 10;
            PlayerProgress.Instance.AddCoins(coinsEarned);
            
            if (coinsRewardText != null)
                coinsRewardText.text = $"+{coinsEarned} монет";
        }
    }
    
    public void ShowLose(int currentScore, int goalScore)
    {
        StartCoroutine(ShowLoseCoroutine(currentScore, goalScore));
    }
    
    private IEnumerator ShowLoseCoroutine(int currentScore, int goalScore)
    {
        yield return new WaitForSeconds(0.5f);
        
        if (panelLose != null)
        {
            panelLose.SetActive(true);
            
            if (scoreTextLose != null)
                scoreTextLose.text = $"Очки: {currentScore}";
            
            if (goalTextLose != null)
                goalTextLose.text = $"Нужно: {goalScore}";
        }
    }
    
    private void NextLevel()
    {
        int nextLevel = LevelManager.Instance.GetCurrentLevelId() + 1;
        LevelManager.Instance.SetCurrentLevel(nextLevel);
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
    
    private void RetryLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
    
    private void MainMenu()
    {
        // TODO: Загрузить меню уровней
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
