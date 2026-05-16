// ======================= GameManager.cs =======================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum BonusType { None, BombColor, BombRow, BombColumn }

    [Header("Grid")]
    [SerializeField] private int gridSize = 8;
    [SerializeField] private GameObject[] piecePrefabs;
    [SerializeField] private GameObject obstaclePrefab;

    private Cell[,] grid;
    private Obstacle[,] obstacles;

    private bool isSwapping = false;
    
    // ===== LEVEL & PROGRESSION =====
    private LevelData currentLevel;
    private int movesRemaining;
    private float timeRemaining = -1f; // -1 = без лимита
    private bool levelInProgress = true;
    private bool levelWon = false;
    private System.Collections.Generic.Dictionary<LevelGoal, int> goalProgress;

    [SerializeField] private float swapDuration = 0.18f;
    [SerializeField] private float pieceFallDuration = 0.18f;
    [SerializeField] private float spawnDelay = 0.02f;

    [Header("Camera")]
    [SerializeField] private float padding = 1f;

    // ================== CELL STRUCT ==================
    // Храним фишку + компонент без постоянных GetComponent
    private class Cell
    {
        public GameObject go;
        public PieceSystem ps;

        public Cell(GameObject go, PieceSystem ps)
        {
            this.go = go;
            this.ps = ps;
        }
    }

    // ================== START ==================
    void Start()
    {
        StartCoroutine(StartGame());
    }
    
    private void Update()
    {
        if (!levelInProgress) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                LevelFailed("Время истекло!");
            }
            else
            {
                UpdateHUD();
            }
        }
    }

    private IEnumerator StartGame()
    {
        // Загружаем данные уровня
        currentLevel = LevelManager.Instance.GetCurrentLevel();
        gridSize = currentLevel.gridSize;
        movesRemaining = currentLevel.moveLimit;
        timeRemaining = currentLevel.timeLimit;
        
        InitializeGoalProgress();
        
        // Инициализируем скоринг
        ScoreCalculator.Instance.ResetScore();
        
        CenterCamera();
        InitializeGrids();
        GenerateInitialBoard();
        
        // Спавним препятствия
        foreach (var pos in currentLevel.obstaclePositions)
        {
            if (obstaclePrefab != null)
                CreateObstacle(pos.x, pos.y, obstaclePrefab, currentLevel.obstacleHp);
        }

        // Ждём, пока все фишки упадут
        yield return new WaitForSeconds(0.25f);

        InitializeGoalProgress();

        // Удаляем стартовые матчи + каскады, пока поле не станет чистым
        while (true)
        {
            var matches = FindMatches();
            if (matches.Count == 0) break;

            yield return StartCoroutine(DestroyMatches(matches));
            yield return new WaitForSeconds(0.1f);
        }
        
        // Сообщаем UI, что игра началась
        if (FindObjectOfType<GameHUD>() != null)
        {
            int lives = PlayerProgress.Instance != null ? PlayerProgress.Instance.GetLives() : 0;
            FindObjectOfType<GameHUD>().UpdateHUD(movesRemaining, timeRemaining, ScoreCalculator.Instance.GetCurrentScore(), lives);
        }
    }

    // ================== GRID SETUP ==================

    private void InitializeGrids()
    {
        grid = new Cell[gridSize, gridSize];
        obstacles = new Obstacle[gridSize, gridSize];
    }

    private void GenerateInitialBoard()
    {
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                CreatePieceAvoidingMatch(x, y);
            }
        }
    }

    // Создаём фишку, избегая матчей по горизонталі и вертикалі
    private void CreatePieceAvoidingMatch(int x, int y)
    {
        int type = Random.Range(0, piecePrefabs.Length);

        // пробуем сменять тип, если он создаёт стартовый матч
        for (int attempts = 0; attempts < 8 && WouldFormMatch(x, y, type); attempts++)
        {
            type = Random.Range(0, piecePrefabs.Length);
        }

        GameObject prefab = piecePrefabs[type];
        GameObject obj = Instantiate(prefab, new Vector3(x, gridSize + 2, 0), Quaternion.identity);

        PieceSystem ps = obj.GetComponent<PieceSystem>();
        ps.Init(this, x, y, type, BonusType.None);

        grid[x, y] = new Cell(obj, ps);

        StartCoroutine(MovePieceToPosition(obj.transform,
            new Vector3(x, y, 0), pieceFallDuration));
    }

    // Проверка: создаст ли тип начальный матч
    private bool WouldFormMatch(int x, int y, int t)
    {
        // горизонталь
        if (x >= 2)
        {
            if (grid[x - 1, y] != null &&
                grid[x - 2, y] != null &&
                grid[x - 1, y].ps.GetPieceType() == t &&
                grid[x - 2, y].ps.GetPieceType() == t)
                return true;
        }

        // вертикаль
        if (y >= 2)
        {
            if (grid[x, y - 1] != null &&
                grid[x, y - 2] != null &&
                grid[x, y - 1].ps.GetPieceType() == t &&
                grid[x, y - 2].ps.GetPieceType() == t)
                return true;
        }

        return false;
    }

    // ================== CAMERA ==================
    private void CenterCamera()
    {
        Camera cam = Camera.main;
        float cx = (gridSize - 1) * 0.5f;
        float cy = (gridSize - 1) * 0.5f;

        cam.transform.position = new Vector3(cx, cy, -10);

        float aspect = (float)Screen.width / Screen.height;
        float v = (gridSize + padding) * 0.5f;
        float h = (gridSize + padding) * 0.5f / aspect;

        cam.orthographicSize = Mathf.Max(v, h);
    }

    private void InitializeGoalProgress()
    {
        goalProgress = new System.Collections.Generic.Dictionary<LevelGoal, int>();

        if (currentLevel == null) return;

        foreach (var goal in currentLevel.goals)
        {
            if (goal != null)
                goalProgress[goal] = 0;
        }
    }

    private void RegisterGoalProgress(LevelGoal goal, int value)
    {
        if (goal == null || !goalProgress.ContainsKey(goal)) return;
        goalProgress[goal] = Mathf.Clamp(goalProgress[goal] + value, 0, goal.target);
    }

    private int GetGoalProgress(LevelGoal goal)
    {
        if (goal == null) return 0;
        if (goal.type == LevelGoal.GoalType.Score)
            return ScoreCalculator.Instance.GetCurrentScore();
        if (goalProgress != null && goalProgress.ContainsKey(goal))
            return goalProgress[goal];
        return 0;
    }

    private bool IsGoalComplete(LevelGoal goal)
    {
        if (goal == null) return false;
        return GetGoalProgress(goal) >= goal.target;
    }

    private bool IsAllGoalsComplete()
    {
        if (currentLevel == null || currentLevel.goals == null || currentLevel.goals.Count == 0)
            return false;

        foreach (var goal in currentLevel.goals)
        {
            if (!IsGoalComplete(goal))
                return false;
        }

        return true;
    }

    private string GetGoalProgressText(LevelGoal goal)
    {
        if (goal == null) return "";

        int progress = GetGoalProgress(goal);
        return $"{goal.description} ({progress}/{goal.target})";
    }

    // ================== SWAP REQUEST FROM PieceSystem ==================
    public void MovePiece(PieceSystem piece, float swipeAngle, float swipeDistance)
    {
        if (isSwapping) return;
        if (swipeDistance < 0.18f) return;
        if (!levelInProgress || levelWon) return;
        
        // Проверяем лимит ходов
        if (movesRemaining <= 0)
        {
            LevelFailed("Ходы закончились!");
            return;
        }

        int x = piece.x;
        int y = piece.y;
        int tx = x;
        int ty = y;

        if (swipeAngle > -45 && swipeAngle <= 45) tx++;
        else if (swipeAngle > 45 && swipeAngle <= 135) ty++;
        else if (swipeAngle > 135 || swipeAngle <= -135) tx--;
        else ty--;

        if (tx < 0 || tx >= gridSize || ty < 0 || ty >= gridSize) return;

        // нельзя двигаться через препятствия
        if (obstacles[tx, ty] != null) return;

        AudioManager.Instance?.PlaySwap();
        StartCoroutine(SwapPieces(x, y, tx, ty));
    }
    // ================== SWAP COROUTINE ==================
    private IEnumerator SwapPieces(int x1, int y1, int x2, int y2, bool revert = false)
    {
        isSwapping = true;

        Cell c1 = grid[x1, y1];
        Cell c2 = grid[x2, y2];

        if (c1 == null || c2 == null)
        {
            isSwapping = false;
            yield break;
        }

        // swap в массиве
        grid[x1, y1] = c2;
        grid[x2, y2] = c1;

        // обновляем координаты
        c1.ps.UpdateCoord(x2, y2);
        c2.ps.UpdateCoord(x1, y1);

        // анимация
        Vector3 p1 = c1.go.transform.position;
        Vector3 p2 = c2.go.transform.position;

        float elapsed = 0f;
        while (elapsed < swapDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / swapDuration;
            c1.go.transform.position = Vector3.Lerp(p1, p2, t);
            c2.go.transform.position = Vector3.Lerp(p2, p1, t);
            yield return null;
        }

        c1.go.transform.position = p2;
        c2.go.transform.position = p1;

        yield return new WaitForSeconds(0.04f);

        // проверяем матчи
        List<Cell> matches = FindMatches();

        if (matches.Count > 0)
        {
            // Уменьшаем ходы только если был успешный матч
            if (!revert)
            {
                movesRemaining--;
                UpdateHUD();
                
                // Проверяем, не проиграли ли
                if (movesRemaining <= 0 && ScoreCalculator.Instance.GetCurrentScore() < currentLevel.scoreGoal)
                {
                    levelInProgress = false;
                    // Завершение будет после каскадов
                }
            }
            
            // создаём бонусы, если нужно
            CreateBonusesAfterSwap(x1, y1, x2, y2, matches);

            foreach (var m in matches)
                m.ps.PlayMatchEffect();

            yield return StartCoroutine(DestroyMatches(matches));
        }
        else if (!revert)
        {
            // откатываем swap
            yield return StartCoroutine(SwapPieces(x2, y2, x1, y1, true));
        }

        isSwapping = false;
    }

    // ================== BONUS CREATION ==================
    private void CreateBonusesAfterSwap(int x1, int y1, int x2, int y2, List<Cell> matches)
    {
        TryCreateBonusOnCell(x1, y1, matches);
        TryCreateBonusOnCell(x2, y2, matches);
    }

    private void OnBonusCreated(Cell cell)
    {
        if (cell == null || cell.ps == null) return;
        if (cell.ps.HasBonus())
        {
            AudioManager.Instance?.PlayBonus();
        }
    }

    private void TryCreateBonusOnCell(int x, int y, List<Cell> matches)
    {
        if (grid[x, y] == null) return;

        int type = grid[x, y].ps.GetPieceType();

        int left = 0, right = 0, up = 0, down = 0;

        // горизонталь
        for (int i = x - 1; i >= 0; i--)
        {
            if (grid[i, y] != null && grid[i, y].ps.GetPieceType() == type) left++;
            else break;
        }
        for (int i = x + 1; i < gridSize; i++)
        {
            if (grid[i, y] != null && grid[i, y].ps.GetPieceType() == type) right++;
            else break;
        }

        // вертикаль
        for (int j = y - 1; j >= 0; j--)
        {
            if (grid[x, j] != null && grid[x, j].ps.GetPieceType() == type) down++;
            else break;
        }
        for (int j = y + 1; j < gridSize; j++)
        {
            if (grid[x, j] != null && grid[x, j].ps.GetPieceType() == type) up++;
            else break;
        }

        int horizCount = left + 1 + right;
        int vertCount = down + 1 + up;

        // 5+ по горизонтали или вертикали → цветовая бомба
        if (horizCount >= 5 || vertCount >= 5)
        {
            grid[x, y].ps.SetBonus(BonusType.BombColor);
            OnBonusCreated(grid[x, y]);
            return;
        }

        // 4 по горизонтали → бомба-строка
        if (horizCount == 4)
        {
            grid[x, y].ps.SetBonus(BonusType.BombRow);
            OnBonusCreated(grid[x, y]);
            return;
        }

        // 4 по вертикали → бомба-колонка
        if (vertCount == 4)
        {
            grid[x, y].ps.SetBonus(BonusType.BombColumn);
            OnBonusCreated(grid[x, y]);
            return;
        }
    }

    // ================== FIND MATCHES ==================
    private List<Cell> FindMatches()
    {
        HashSet<Cell> result = new HashSet<Cell>();

        // horizontal
        for (int y = 0; y < gridSize; y++)
        {
            int run = 1;
            for (int x = 1; x < gridSize; x++)
            {
                bool match = (grid[x, y] != null && grid[x - 1, y] != null &&
                              grid[x, y].ps.GetPieceType() == grid[x - 1, y].ps.GetPieceType());

                if (match) run++;
                else
                {
                    if (run >= 3)
                    {
                        for (int k = 0; k < run; k++)
                            result.Add(grid[x - 1 - k, y]);
                    }
                    run = 1;
                }
            }

            if (run >= 3)
            {
                for (int k = 0; k < run; k++)
                    result.Add(grid[gridSize - 1 - k, y]);
            }
        }

        // vertical
        for (int x = 0; x < gridSize; x++)
        {
            int run = 1;
            for (int y = 1; y < gridSize; y++)
            {
                bool match = (grid[x, y] != null && grid[x, y - 1] != null &&
                              grid[x, y].ps.GetPieceType() == grid[x, y - 1].ps.GetPieceType());

                if (match) run++;
                else
                {
                    if (run >= 3)
                    {
                        for (int k = 0; k < run; k++)
                            result.Add(grid[x, y - 1 - k]);
                    }
                    run = 1;
                }
            }

            if (run >= 3)
            {
                for (int k = 0; k < run; k++)
                    result.Add(grid[x, gridSize - 1 - k]);
            }
        }

        return new List<Cell>(result);
    }
    // ================== DESTROY MATCHES (+ OBSTACLES) ==================
    private IEnumerator DestroyMatches(List<Cell> initialList)
    {
        if (initialList == null || initialList.Count == 0)
            yield break;

        HashSet<Cell> toDestroy = new HashSet<Cell>(initialList);
        Queue<Cell> queue = new Queue<Cell>(initialList);

        // --- APPLY BONUS EFFECTS (EXPAND DESTROY LIST) ---
        while (queue.Count > 0)
        {
            Cell c = queue.Dequeue();
            if (c == null || c.ps == null) continue;

            if (c.ps.HasBonus())
            {
                BonusType b = c.ps.GetBonus();

                switch (b)
                {
                    case BonusType.BombColor:
                        int color = c.ps.GetPieceType();
                        for (int xx = 0; xx < gridSize; xx++)
                            for (int yy = 0; yy < gridSize; yy++)
                            {
                                Cell target = grid[xx, yy];
                                if (target != null && target.ps.GetPieceType() == color && !toDestroy.Contains(target))
                                {
                                    toDestroy.Add(target);
                                    queue.Enqueue(target);
                                }
                            }
                        break;

                    case BonusType.BombRow:
                        for (int xx = 0; xx < gridSize; xx++)
                        {
                            Cell target = grid[xx, c.ps.y];
                            if (target != null && !toDestroy.Contains(target))
                            {
                                toDestroy.Add(target);
                                queue.Enqueue(target);
                            }
                        }
                        break;

                    case BonusType.BombColumn:
                        for (int yy = 0; yy < gridSize; yy++)
                        {
                            Cell target = grid[c.ps.x, yy];
                            if (target != null && !toDestroy.Contains(target))
                            {
                                toDestroy.Add(target);
                                queue.Enqueue(target);
                            }
                        }
                        break;
                }
            }
        }

        // --- TRACK GOAL PROGRESS ---
        foreach (var c in toDestroy)
        {
            if (c == null || c.ps == null) continue;
            foreach (var goal in currentLevel.goals)
            {
                if (goal == null) continue;
                if (goal.type == LevelGoal.GoalType.CollectPieceType && c.ps.GetPieceType() == goal.targetPieceType)
                {
                    RegisterGoalProgress(goal, 1);
                }
            }
        }

        if (IsAllGoalsComplete() && !levelWon)
        {
            levelWon = true;
            levelInProgress = false;
        }

        // --- CALCULATE SCORE ---
        ScoreCalculator.Instance.CalculateScore(toDestroy.Count, false);
        AudioManager.Instance?.PlayMatch();
        UpdateHUD();
        
        // Проверяем, не выиграли ли уровень
        if (ScoreCalculator.Instance.GetCurrentScore() >= currentLevel.scoreGoal && !levelWon)
        {
            levelWon = true;
            levelInProgress = false;
        }

        // --- DAMAGE OBSTACLES NEAR MATCHES ---
        foreach (var c in toDestroy)
        {
            if (c == null || c.ps == null) continue;

            int x = c.ps.x;
            int y = c.ps.y;

            TryHitObstacle(x + 1, y);
            TryHitObstacle(x - 1, y);
            TryHitObstacle(x, y + 1);
            TryHitObstacle(x, y - 1);
        }

        // --- DESTROY PIECES ---
        foreach (var c in toDestroy)
        {
            if (c == null || c.go == null) continue;

            c.ps.PlayMatchEffect();

            int x = c.ps.x;
            int y = c.ps.y;

            grid[x, y] = null;

            Destroy(c.go);
            c.go = null;
            c.ps = null;
        }

        yield return new WaitForSeconds(0.12f);

        // падение + заполнение
        yield return StartCoroutine(ShiftPiecesDown());

        // каскады
        if (levelInProgress)
        {
            ScoreCalculator.Instance.ResetCascadeMultiplier();
            List<Cell> newMatches = FindMatches();
            if (newMatches.Count > 0)
            {
                yield return new WaitForSeconds(0.08f);
                yield return StartCoroutine(DestroyMatches(newMatches));
            }
            else if (!levelWon && movesRemaining <= 0 && ScoreCalculator.Instance.GetCurrentScore() < currentLevel.scoreGoal)
            {
                LevelFailed("Ходы закончились!");
            }
        }
        
        // Если уровень завершён
        if (!levelInProgress && levelWon)
        {
            yield return new WaitForSeconds(0.5f);
            LevelWon();
        }
    }
    
    private void UpdateHUD()
    {
        GameHUD hud = FindObjectOfType<GameHUD>();
        if (hud != null)
        {
            int lives = PlayerProgress.Instance != null ? PlayerProgress.Instance.GetLives() : 0;
            hud.UpdateHUD(
                movesRemaining,
                (int)timeRemaining,
                ScoreCalculator.Instance.GetCurrentScore(),
                lives,
                GetGoalProgressText(currentLevel.GetPrimaryGoal())
            );
        }
    }
    
    private void LevelWon()
    {
        levelInProgress = false;
        int finalScore = ScoreCalculator.Instance.GetCurrentScore();
        int stars = currentLevel.GetStarCount(finalScore);
        
        PlayerProgress.Instance.CompleteLevelWithScore(currentLevel.levelId, finalScore);
        AudioManager.Instance?.PlayWin();
        
        Debug.Log($"LEVEL WON! Score: {finalScore}, Stars: {stars}");
        
        LevelCompleteUI ui = FindObjectOfType<LevelCompleteUI>();
        if (ui != null)
            ui.ShowWin(finalScore, stars);
    }
    
    private void LevelFailed(string reason)
    {
        levelInProgress = false;
        int currentScore = ScoreCalculator.Instance.GetCurrentScore();
        
        Debug.Log($"LEVEL FAILED: {reason}");
        
        if (PlayerProgress.Instance != null && PlayerProgress.Instance.GetLives() > 0)
        {
            PlayerProgress.Instance.SpendLife();
        }
        
        AudioManager.Instance?.PlayLose();
        UpdateHUD();
        
        LevelCompleteUI ui = FindObjectOfType<LevelCompleteUI>();
        if (ui != null)
            ui.ShowLose(currentScore, currentLevel.scoreGoal);
    }

    // ================== OBSTACLE INTERACTION ==================
    private void TryHitObstacle(int x, int y)
    {
        if (x < 0 || x >= gridSize || y < 0 || y >= gridSize)
            return;

        var ob = obstacles[x, y];
        if (ob == null) return;

        ob.TakeHit();

        if (!ob.IsAlive())
        {
            obstacles[x, y] = null;

            foreach (var goal in currentLevel.goals)
            {
                if (goal != null && goal.type == LevelGoal.GoalType.DestroyObstacles)
                {
                    RegisterGoalProgress(goal, 1);
                    if (IsAllGoalsComplete() && !levelWon)
                    {
                        levelWon = true;
                        levelInProgress = false;
                    }
                }
            }
        }
    }

    // ================== SHIFT PIECES DOWN ==================
    private IEnumerator ShiftPiecesDown()
    {
        for (int x = 0; x < gridSize; x++)
        {
            int emptyBelow = 0;

            for (int y = 0; y < gridSize; y++)
            {
                // препятствие = "пол"
                if (obstacles[x, y] != null)
                {
                    emptyBelow = 0;
                    continue;
                }

                if (grid[x, y] == null)
                {
                    emptyBelow++;
                }
                else if (emptyBelow > 0)
                {
                    Cell c = grid[x, y];
                    grid[x, y - emptyBelow] = c;
                    grid[x, y] = null;

                    c.ps.UpdateCoord(x, y - emptyBelow);

                    StartCoroutine(MovePieceToPosition(
                        c.go.transform,
                        new Vector3(x, y - emptyBelow, 0),
                        pieceFallDuration
                    ));
                }
            }

            // спавним новые фишки сверху
            for (int i = 0; i < emptyBelow; i++)
            {
                int yy = gridSize - 1 - i;

                // нельзя создавать фишку поверх препятствия
                if (obstacles[x, yy] != null) continue;

                CreateNewPieceAt(x, yy);
                yield return new WaitForSeconds(spawnDelay);
            }
        }

        yield return new WaitForSeconds(pieceFallDuration + 0.02f);
    }

    // ================== CREATE NEW PIECE ==================
    private void CreateNewPieceAt(int x, int y)
    {
        int type = Random.Range(0, piecePrefabs.Length);

        for (int i = 0; i < 6 && WouldFormMatch(x, y, type); i++)
            type = Random.Range(0, piecePrefabs.Length);

        GameObject prefab = piecePrefabs[type];
        GameObject obj = Instantiate(prefab, new Vector3(x, gridSize + 2, 0), Quaternion.identity);

        PieceSystem ps = obj.GetComponent<PieceSystem>();
        ps.Init(this, x, y, type, BonusType.None);

        grid[x, y] = new Cell(obj, ps);

        StartCoroutine(MovePieceToPosition(obj.transform,
            new Vector3(x, y, 0),
            pieceFallDuration));
    }
    // ================== MOVE PIECE TO POSITION ==================
    private IEnumerator MovePieceToPosition(Transform t, Vector3 target, float duration)
    {
        Vector3 start = t.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            t.position = Vector3.Lerp(start, target, elapsed / duration);
            yield return null;
        }

        t.position = target;
    }

    // ================== OBSTACLE CREATION ==================
    // Можно вызывать из любого места или из инспектора
    public void CreateObstacle(int x, int y, GameObject prefab, int hp = 2)
    {
        if (obstacles[x, y] != null) return;

        GameObject o = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
        Obstacle ob = o.GetComponent<Obstacle>();
        ob.Init(x, y, hp);

        obstacles[x, y] = ob;

        // Важно: препятствие заменяет фишку
        if (grid[x, y] != null)
        {
            Destroy(grid[x, y].go);
            grid[x, y] = null;
        }
    }
}

