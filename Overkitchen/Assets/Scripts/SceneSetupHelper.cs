using System.Reflection;
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

        if (FindObjectOfType<AudioManager>() == null)
        {
            GameObject audioGO = new GameObject("AudioManager");
            audioGO.AddComponent<AudioManager>();
            DontDestroyOnLoad(audioGO);
        }

        Debug.Log("Managers created!");
    }

    [ContextMenu("Setup Main Menu UI")]
    public void SetupMainMenuUI()
    {
        Canvas canvas = GetOrCreateCanvas();
        GameObject root = CreateUIContainer(canvas.transform, "MainMenuUI");

        TextMeshProUGUI title = CreateText(root.transform, "TitleText", "OverKitchen", 60, TextAlignmentOptions.Center);
        title.rectTransform.anchoredPosition = new Vector2(0, 220);

        Button playButton = CreateButton(root.transform, "PlayButton", "Играть");
        Button settingsButton = CreateButton(root.transform, "SettingsButton", "Настройки");
        Button achievementsButton = CreateButton(root.transform, "AchievementsButton", "Достижения");
        Button shopButton = CreateButton(root.transform, "ShopButton", "Магазин");

        TextMeshProUGUI coinsText = CreateText(root.transform, "CoinsText", "💰 1000", 24, TextAlignmentOptions.Center);
        TextMeshProUGUI starsText = CreateText(root.transform, "StarsText", "⭐ 0", 24, TextAlignmentOptions.Center);
        TextMeshProUGUI levelText = CreateText(root.transform, "LevelText", "Уровень: 1", 24, TextAlignmentOptions.Center);

        MainMenuUI menuUI = root.AddComponent<MainMenuUI>();
        SetPrivateField(menuUI, "playButton", playButton);
        SetPrivateField(menuUI, "settingsButton", settingsButton);
        SetPrivateField(menuUI, "achievementsButton", achievementsButton);
        SetPrivateField(menuUI, "shopButton", shopButton);
        SetPrivateField(menuUI, "coinsText", coinsText);
        SetPrivateField(menuUI, "starsText", starsText);
        SetPrivateField(menuUI, "levelText", levelText);

        Debug.Log("Main Menu UI setup complete!");
    }

    [ContextMenu("Setup Level Select UI")]
    public void SetupLevelSelectUI()
    {
        Canvas canvas = GetOrCreateCanvas();
        GameObject root = CreateUIContainer(canvas.transform, "LevelSelectUI");

        TextMeshProUGUI title = CreateText(root.transform, "TitleText", "Выбор уровня", 50, TextAlignmentOptions.Center);
        title.rectTransform.anchoredPosition = new Vector2(0, 220);

        GameObject buttonContainer = CreateUIContainer(root.transform, "LevelButtonContainer");
        VerticalLayoutGroup layout = buttonContainer.AddComponent<VerticalLayoutGroup>();
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.spacing = 8;
        layout.padding = new RectOffset(10, 10, 10, 10);
        ContentSizeFitter fitter = buttonContainer.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        RectTransform containerRect = buttonContainer.GetComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0.1f, 0.3f);
        containerRect.anchorMax = new Vector2(0.45f, 0.85f);
        containerRect.offsetMin = Vector2.zero;
        containerRect.offsetMax = Vector2.zero;

        GameObject levelButtonPrefab = CreateLevelButtonPrefab(root.transform, "LevelButtonPrefab");
        levelButtonPrefab.SetActive(false);

        Button backButton = CreateButton(root.transform, "BackButton", "Назад");
        backButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-220, -220);
        backButton.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 50);

        GameObject previewPanel = CreateUIContainer(root.transform, "LevelPreviewPanel");
        RectTransform previewRect = previewPanel.GetComponent<RectTransform>();
        previewRect.anchorMin = new Vector2(0.55f, 0.3f);
        previewRect.anchorMax = new Vector2(0.9f, 0.85f);
        previewRect.offsetMin = Vector2.zero;
        previewRect.offsetMax = Vector2.zero;

        Image previewImage = previewPanel.AddComponent<Image>();
        previewImage.color = new Color(0.1f, 0.1f, 0.1f, 0.5f);

        TextMeshProUGUI levelNameText = CreateText(previewPanel.transform, "LevelNameText", "Уровень 1", 34, TextAlignmentOptions.Center);
        levelNameText.rectTransform.anchoredPosition = new Vector2(0, 120);
        TextMeshProUGUI levelInfoText = CreateText(previewPanel.transform, "LevelInfoText", "Ходы: 20 | Цель: 25000", 22, TextAlignmentOptions.Center);
        levelInfoText.rectTransform.anchoredPosition = new Vector2(0, 60);
        TextMeshProUGUI bestScoreText = CreateText(previewPanel.transform, "BestScoreText", "Лучший результат: ---", 22, TextAlignmentOptions.Center);
        bestScoreText.rectTransform.anchoredPosition = new Vector2(0, 20);

        LevelSelectUI selectUI = root.AddComponent<LevelSelectUI>();
        SetPrivateField(selectUI, "levelButtonContainer", buttonContainer.transform);
        SetPrivateField(selectUI, "levelButtonPrefab", levelButtonPrefab);
        SetPrivateField(selectUI, "backButton", backButton);
        SetPrivateField(selectUI, "levelPreview", previewImage);
        SetPrivateField(selectUI, "levelNameText", levelNameText);
        SetPrivateField(selectUI, "levelInfoText", levelInfoText);
        SetPrivateField(selectUI, "bestScoreText", bestScoreText);

        Debug.Log("Level Select UI setup complete!");
    }

    private Canvas GetOrCreateCanvas()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        return canvas;
    }

    private GameObject CreateUIContainer(Transform parent, string name)
    {
        GameObject obj = new GameObject(name);
        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        return obj;
    }

    private TextMeshProUGUI CreateText(Transform parent, string name, string text, int fontSize = 28, TextAlignmentOptions alignment = TextAlignmentOptions.Left)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        TextMeshProUGUI uiText = obj.AddComponent<TextMeshProUGUI>();
        uiText.text = text;
        uiText.fontSize = fontSize;
        uiText.alignment = alignment;
        uiText.color = Color.white;
        uiText.enableWordWrapping = false;
        RectTransform rect = uiText.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(500, 60);
        return uiText;
    }

    private Button CreateButton(Transform parent, string name, string label)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent, false);
        Image bg = buttonObj.AddComponent<Image>();
        bg.color = new Color(0.18f, 0.18f, 0.18f, 0.95f);
        Button button = buttonObj.AddComponent<Button>();
        RectTransform rect = buttonObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(260, 60);

        GameObject labelObj = new GameObject("Text");
        labelObj.transform.SetParent(buttonObj.transform, false);
        TextMeshProUGUI text = labelObj.AddComponent<TextMeshProUGUI>();
        text.text = label;
        text.fontSize = 28;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
        RectTransform textRect = labelObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        return button;
    }

    private GameObject CreateLevelButtonPrefab(Transform parent, string name)
    {
        GameObject prefab = new GameObject(name);
        RectTransform rect = prefab.AddComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.sizeDelta = new Vector2(260, 60);

        Image bg = prefab.AddComponent<Image>();
        bg.color = new Color(0.14f, 0.14f, 0.14f, 0.95f);
        Button button = prefab.AddComponent<Button>();

        GameObject labelObj = new GameObject("Label");
        labelObj.transform.SetParent(prefab.transform, false);
        TextMeshProUGUI labelText = labelObj.AddComponent<TextMeshProUGUI>();
        labelText.text = "Уровень";
        labelText.fontSize = 24;
        labelText.alignment = TextAlignmentOptions.Center;
        labelText.color = Color.white;
        RectTransform labelRect = labelObj.GetComponent<RectTransform>();
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        GameObject starsObj = new GameObject("Stars");
        starsObj.transform.SetParent(prefab.transform, false);
        TextMeshProUGUI starsText = starsObj.AddComponent<TextMeshProUGUI>();
        starsText.text = "";
        starsText.fontSize = 18;
        starsText.alignment = TextAlignmentOptions.BottomRight;
        starsText.color = Color.yellow;
        RectTransform starsRect = starsObj.GetComponent<RectTransform>();
        starsRect.anchorMin = new Vector2(0.5f, 0f);
        starsRect.anchorMax = new Vector2(1f, 0.5f);
        starsRect.offsetMin = new Vector2(-80, 6);
        starsRect.offsetMax = new Vector2(-10, 30);

        return prefab;
    }

    private void SetPrivateField<T>(Component target, string fieldName, T value)
    {
        FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (field != null)
            field.SetValue(target, value);
    }
}
