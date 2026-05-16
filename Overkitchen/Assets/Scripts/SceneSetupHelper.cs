using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Этот скрипт помогает быстро настроить сцену с нужными UI элементами
/// Можно вызвать из меню или использовать как шаблон для ручной настройки
/// </summary>
public class SceneSetupHelper : MonoBehaviour
{
    [ContextMenu("Setup Game HUD (для SampleScene)")]
    public void SetupGameHUD()
    {
        // Ищем или создаём Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // Создаём HUD Panel
        GameObject hudPanel = new GameObject("HUD");
        RectTransform hudRect = hudPanel.AddComponent<RectTransform>();
        hudRect.SetParent(canvas.transform, false);
        hudRect.anchorMin = Vector2.zero;
        hudRect.anchorMax = Vector2.one;
        hudRect.offsetMin = Vector2.zero;
        hudRect.offsetMax = Vector2.zero;

        // Score Text
        GameObject scoreObj = new GameObject("ScoreText");
        scoreObj.transform.SetParent(hudPanel.transform);
        TextMeshProUGUI scoreText = scoreObj.AddComponent<TextMeshProUGUI>();
        scoreText.text = "Очки: 0";
        RectTransform scoreRect = scoreObj.GetComponent<RectTransform>();
        scoreRect.anchoredPosition = new Vector2(50, -50);
        scoreRect.sizeDelta = new Vector2(200, 50);

        // Moves Text
        GameObject movesObj = new GameObject("MovesText");
        movesObj.transform.SetParent(hudPanel.transform);
        TextMeshProUGUI movesText = movesObj.AddComponent<TextMeshProUGUI>();
        movesText.text = "Ходы: 20";
        RectTransform movesRect = movesObj.GetComponent<RectTransform>();
        movesRect.anchoredPosition = new Vector2(-100, -50);
        movesRect.sizeDelta = new Vector2(200, 50);

        // Timer Text
        GameObject timerObj = new GameObject("TimerText");
        timerObj.transform.SetParent(hudPanel.transform);
        TextMeshProUGUI timerText = timerObj.AddComponent<TextMeshProUGUI>();
        timerText.text = "Время: ∞";
        RectTransform timerRect = timerObj.GetComponent<RectTransform>();
        timerRect.anchoredPosition = new Vector2(50, 50);
        timerRect.sizeDelta = new Vector2(200, 50);

        // Goal Text
        GameObject goalObj = new GameObject("GoalText");
        goalObj.transform.SetParent(hudPanel.transform);
        TextMeshProUGUI goalText = goalObj.AddComponent<TextMeshProUGUI>();
        goalText.text = "Цель: 25000";
        RectTransform goalRect = goalObj.GetComponent<RectTransform>();
        goalRect.anchoredPosition = new Vector2(-100, 50);
        goalRect.sizeDelta = new Vector2(200, 50);

        // Progress Bar
        GameObject progressObj = new GameObject("ProgressBar");
        progressObj.transform.SetParent(hudPanel.transform);
        Image progressBar = progressObj.AddComponent<Image>();
        progressBar.color = Color.green;
        RectTransform progressRect = progressObj.GetComponent<RectTransform>();
        progressRect.anchoredPosition = new Vector2(0, 0);
        progressRect.sizeDelta = new Vector2(300, 20);

        // Добавляем GameHUD компонент
        GameHUD gameHUD = hudPanel.AddComponent<GameHUD>();
        gameHUD.GetType().GetField("scoreText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(gameHUD, scoreText);
        gameHUD.GetType().GetField("movesText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(gameHUD, movesText);
        gameHUD.GetType().GetField("timerText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(gameHUD, timerText);
        gameHUD.GetType().GetField("goalText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(gameHUD, goalText);
        gameHUD.GetType().GetField("progressBar", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(gameHUD, progressBar);

        Debug.Log("Game HUD setup complete!");
    }

    [ContextMenu("Create Managers")]
    public void CreateManagers()
    {
        // Проверяем, существуют ли уже
        if (FindObjectOfType<LevelManager>() == null)
        {
            GameObject lm = new GameObject("LevelManager");
            lm.AddComponent<LevelManager>();
            DontDestroyOnLoad(lm);
        }

        if (FindObjectOfType<PlayerProgress>() == null)
        {
            GameObject pp = new GameObject("PlayerProgress");
            pp.AddComponent<PlayerProgress>();
            DontDestroyOnLoad(pp);
        }

        if (FindObjectOfType<ScoreCalculator>() == null)
        {
            GameObject sc = new GameObject("ScoreCalculator");
            sc.AddComponent<ScoreCalculator>();
            DontDestroyOnLoad(sc);
        }

        Debug.Log("Managers created!");
    }
}
