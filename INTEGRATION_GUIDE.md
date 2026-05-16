# 🎮 OverKitchen - Game Enhancements Documentation

## ✅ Что было добавлено

### 1. **Level System (Система уровней)**
- `LevelData.cs` - Данные уровня (размер сетки, лимит ходов, цели, препятствия)
- `LevelManager.cs` - Управление уровнями (100 уровней с прогрессирующей сложностью)

### 2. **Player Progress (Прогресс игрока)**
- `PlayerProgress.cs` - Сохранение прогресса (JSON на основе PlayerPrefs)
  - Монеты и премиум-валюта
  - Жизни
  - Разблокированные уровни
  - Прогресс по уровням (score, stars)
  - Достижения

### 3. **Scoring System (Система скоринга)**
- `ScoreCalculator.cs` - Расчёт очков
  - Базовые очки за матчи (3-5 элементов)
  - Множители за бонусы (Бомбы 1.5-3x)
  - Каскадные множители (каждый каскад +10% к множителю)

### 4. **Game Integration (Интеграция в игру)**
- Обновлён `GameManager.cs`:
  - Загрузка данных уровня при старте
  - Лимит ходов и времени
  - Проверка условий победы/поражения
  - Интеграция с системой скоринга
  - Сохранение результатов уровня

### 5. **UI System (Система интерфейса)**
- `GameHUD.cs` - HUD во время игры (очки, ходы, время, цель)
- `LevelCompleteUI.cs` - Экран завершения уровня (победа/поражение)
- `LevelSelectUI.cs` - Выбор уровня (первые 20 уровней, разблокировка)
- `MainMenuUI.cs` - Главное меню

---

## 🛠️ Инструкция по интеграции в Unity

### Шаг 1: Убедитесь, что TextMeshPro установлен
```
Windows → TextMesh Pro → Import TMP Essentials
```

### Шаг 2: Создайте необходимые сцены
Нужны сцены:
- **MainMenu** - Главное меню
- **LevelSelect** - Выбор уровня  
- **SampleScene** (существует) - Игровой процесс

### Шаг 3: Добавьте компоненты в сцену "SampleScene"
1. **Canvas** (новый) → Добавьте скрипты:
   - `GameHUD.cs` - для отображения ходов, очков, времени
   - `LevelCompleteUI.cs` - для экрана завершения

2. **Empty GameObject** → Назовите "Managers"
   - Добавьте `LevelManager.cs`
   - Добавьте `PlayerProgress.cs`
   - Добавьте `ScoreCalculator.cs`

3. **Существующий GameManager** → Добавьте новые поля:
```csharp
[SerializeField] private GameObject obstaclePrefab; // Если есть препятствия
```

### Шаг 4: Создайте UI для GameHUD
В Canvas создайте структуру:
```
Canvas
├── HUD Panel (RectTransform)
│   ├── Score Text (TextMeshProUGUI)
│   ├── Moves Text (TextMeshProUGUI)
│   ├── Timer Text (TextMeshProUGUI)
│   ├── Goal Text (TextMeshProUGUI)
│   └── Progress Bar (Image)
├── Win Panel (Panel)
│   ├── Score Text
│   ├── Stars (Image)
│   ├── Coins Reward Text
│   └── Buttons (Next Level, Retry, Menu)
└── Lose Panel (Panel)
    ├── Score Text
    ├── Goal Text
    └── Buttons (Retry, Menu)
```

### Шаг 5: Назначьте ссылки в Inspector
В `GameHUD.cs` заполните:
- Score Text → поле scoreText
- Moves Text → поле movesText
- Timer Text → поле timerText
- Goal Text → поле goalText
- Progress Bar → поле progressBar

### Шаг 6: Создайте сцену LevelSelect
1. Создайте новую сцену "LevelSelect"
2. Добавьте Canvas с компонентом `LevelSelectUI.cs`
3. Создайте:
   - Button Prefab для уровней
   - Scroll View с Container для кнопок
   - Preview Panel (справа) с информацией о уровне
   - Back Button

### Шаг 7: Создайте MainMenu
1. Новая сцена "MainMenu"
2. Canvas с компонентом `MainMenuUI.cs`
3. Кнопки: Play, Settings, Achievements, Shop
4. Отображение: Монеты, Звёзды, Текущий уровень

### Шаг 8: Настройка Build Settings
1. File → Build Settings
2. Добавьте сцены в порядке:
   - 0: MainMenu
   - 1: LevelSelect
   - 2: SampleScene

---

## 🎯 Как это работает

### Игровой процесс:
1. Главное меню → Выбор уровня → Игра
2. При свайпе фишек:
   - Проверяется лимит ходов (-1 за каждый свайп с матчем)
   - Рассчитываются очки по формуле
3. Условия завершения:
   - **Победа**: Score ≥ Level.scoreGoal
   - **Поражение**: Ходы = 0 И Score < цель

### Сохранение прогресса:
- PlayerPrefs (автосохранение)
- Данные: уровни, монеты, звёзды, достижения
- Загружается при старте MainMenuUI

### Прогрессия уровней:
- Уровень 1: 20 ходов, цель 20K
- Уровень 2-4: Спецефические данные
- Уровень 5+: Генерируется автоматически (растущая сложность)

---

## 📊 Данные уровня (примеры)

### Уровень 1 - Начало
- Grid: 8x8
- Ходы: 25
- Цель: 20,000 очков
- Сложность: 1
- Звёзды: 1★ при 20K, 2★ при 30K, 3★ при 45K

### Уровень 3 - Препятствия
- Grid: 8x8
- Ходы: 20
- Цель: 30,000 очков
- Препятствия на (3,3) и (4,4)
- Сложность: 3

---

## 🐛 Известные проблемы & TODO

1. ⚠️ **Препятствия не спавнятся** - Нужно добавить prefab и вызвать CreateObstacle
2. ⚠️ **Нет анимации для звёзд** - Можно улучшить визуально
3. ⚠️ **Нет Daily Challenges** - Следующий шаг
4. ⚠️ **Нет Shop** - В планах
5. ⚠️ **Нет Achievements** - В планах

---

## 🚀 Следующие шаги

1. **Daily Challenges** - Ежедневные квесты с наградами
2. **Shop & Boosters** - Покупка экстра-ходов, множителей
3. **More Bonus Types** - Lightning, Fireworks, UFO
4. **Leaderboards** - Глобальный рейтинг
5. **Sound & Music** - Аудио эффекты

---

## 💡 Примечания для разработчиков

### Скоринг:
```csharp
// Базовая формула:
baseScore = (pointsPer3/4/5Match) * matchCount * cascadeMultiplier * bonusMultiplier
```

### Звёзды:
```csharp
// levelData.GetStarCount(score)
3 stars if score >= starThreshold3
2 stars if score >= starThreshold2
1 star  if score >= starThreshold1
0 stars otherwise
```

### Сохранение:
```csharp
PlayerProgress.Instance.CompleteLevelWithScore(levelId, score);
// Автоматически:
// - Сохраняет лучший результат
// - Рассчитывает звёзды
// - Разблокирует следующий уровень
// - Добавляет общие звёзды в профиль
```

Приятной разработки! 🎮✨
