# ⚡ Quick Start Guide - OverKitchen Game Enhancements

## 🎯 Что было сделано (Fase 1 - Foundation)

### ✅ Реализовано:
1. **Level System** (LevelManager + LevelData)
   - 100 уровней с прогрессирующей сложностью
   - Каждый уровень имеет: лимит ходов, цель по очкам, препятствия
   - Автоматическая генерация уровней 5-100

2. **Player Progress** (PlayerProgress)
   - Сохранение прогресса в JSON (PlayerPrefs)
   - Отслеживание: монеты, жизни, разблокированные уровни, звёзды
   - Автоматическое сохранение после каждого уровня

3. **Scoring System** (ScoreCalculator)
   - Базовые очки за матчи 3/4/5+
   - Множители за бонусы (1.5x-3x)
   - Каскадные множители (+10% за каждый каскад)

4. **GameManager Integration**
   - Загрузка данных уровня
   - Лимит ходов и времени
   - Проверка победы/поражения
   - Интеграция скоринга

5. **UI System**
   - GameHUD - HUD во время игры
   - LevelCompleteUI - Экран завершения (победа/поражение)
   - LevelSelectUI - Выбор уровня
   - MainMenuUI - Главное меню

---

## 🚀 Как начать

### Вариант 1: Быстрая интеграция (5 минут)
1. Откройте SampleScene в Unity
2. Добавьте Empty GameObject "Managers"
3. Добавьте компоненты:
   - LevelManager.cs
   - PlayerProgress.cs
   - ScoreCalculator.cs
4. Создайте Canvas и добавьте:
   - GameHUD.cs с заполненными ссылками
   - LevelCompleteUI.cs с заполненными ссылками

### Вариант 2: Автоматическая настройка
1. Откройте SampleScene
2. Добавьте GameObject с `SceneSetupHelper.cs`
3. В Inspector → Right Click на компоненте → "Setup Game HUD"
4. Right Click → "Create Managers"

### Вариант 3: Полная интеграция (см. INTEGRATION_GUIDE.md)

---

## 📚 Структура файлов

| Файл | Назначение |
|------|-----------|
| `LevelData.cs` | Конфиг уровня (ходы, цель, препятствия) |
| `LevelManager.cs` | Управление уровнями (singleton) |
| `PlayerProgress.cs` | Сохранение прогресса (singleton) |
| `ScoreCalculator.cs` | Расчёт очков (singleton) |
| `GameHUD.cs` | UI во время игры |
| `LevelCompleteUI.cs` | Экран победы/поражения |
| `LevelSelectUI.cs` | Выбор уровня |
| `MainMenuUI.cs` | Главное меню |

---

## 💾 Как работает сохранение

```csharp
// Автоматически вызывается при завершении уровня:
PlayerProgress.Instance.CompleteLevelWithScore(levelId, score);

// Это сохраняет:
✓ Лучший результат
✓ Количество звёзд (1-3 в зависимости от score)
✓ Разблокировку следующего уровня
✓ Сохраняет в PlayerPrefs (автоматически)
```

---

## 🎮 Примеры использования

### Загрузить уровень
```csharp
LevelData level = LevelManager.Instance.GetLevel(2);
Debug.Log($"Уровень: {level.levelName}, Ходы: {level.moveLimit}");
```

### Получить прогресс игрока
```csharp
int coins = PlayerProgress.Instance.GetCoins();
int level = PlayerProgress.Instance.GetHighestUnlockedLevel();
Debug.Log($"Монеты: {coins}, Разблокирован уровень: {level}");
```

### Добавить очки
```csharp
ScoreCalculator.Instance.CalculateScore(matchCount: 4, hasBonus: true, bonusType: GameManager.BonusType.BombRow);
```

---

## 📋 Уровни и их характеристики

### Первые уровни (1-4): Ручная настройка
```
Level 1: 25 ходов, 20K очков (Tutorial)
Level 2: 22 хода, 25K очков
Level 3: 20 ходов, 30K очков (+ препятствия)
Level 4: 18 ходов, 35K очков
```

### Уровни 5+: Автоматическая генерация
```
Level 5:  15 ходов, 32.5K очков
Level 10: 13 ходов, 45K очков
Level 20: 11 ходов, 70K очков
...и так далее с растущей сложностью
```

---

## 🎯 Условия победы/поражения

**Победа:**
```
Score ≥ LevelData.scoreGoal
```

**Поражение (лимит ходов):**
```
Ходы = 0 И Score < цель
```

**Поражение (лимит времени - опционально):**
```
Время = 0 И Score < цель
```

---

## 🔧 Разработчикам

### Как добавить свой уровень вручную
```csharp
// В LevelManager.InitializeLevels():
LevelData level10 = new LevelData(10, "Мой уровень");
level10.moveLimit = 20;
level10.scoreGoal = 40000;
level10.difficulty = 3;
level10.obstaclePositions.Add(new Vector2Int(2, 2));
levels[10] = level10;
```

### Как переопределить звёзды
```csharp
level.starThreshold1 = 30000; // 1 звезда
level.starThreshold2 = 45000; // 2 звезды
level.starThreshold3 = 60000; // 3 звезды
```

---

## 🐛 Частые проблемы

### Ошибка: "LevelManager.Instance is null"
**Решение:** Убедитесь, что LevelManager добавлен на сцену как компонент

### Ошибка: TextMesh Pro не найден
**Решение:** Окно → TextMesh Pro → Import TMP Essentials

### Сохранение не работает
**Решение:** Проверьте, что вызывается `PlayerProgress.Instance.SaveGame()`

---

## 📊 Статистика проекта

- **Создано файлов**: 11 скриптов
- **Строк кода**: ~2000
- **Уровней**: 100 (автогенерация)
- **Фичей**: 5 ключевых систем

---

## 🎊 Что дальше?

**Фаза 2 (Next):**
- [ ] Daily Challenges
- [ ] Shop & Boosters
- [ ] Дополнительные типы бонусов
- [ ] Leaderboards

Готово к commit! 🚀
