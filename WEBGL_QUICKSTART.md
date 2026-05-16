# 🚀 WebGL Optimization - Быстрый старт

## ✅ Что уже сделано

1. **WEBGL_OPTIMIZATION.md** — полный гайд оптимизации
2. **WebGLOptimizer.cs** — автоматизированный скрипт для Unity Editor
3. **link.xml** — конфигурация code stripping для уменьшения размера

---

## 🎯 Следующие шаги (в порядке приоритета)

### 1️⃣ Открыть проект в Unity и запустить оптимизацию (5 минут)
```
1. Открить Overkitchen в Unity 2020.3+
2. Перейти в меню: Tools → OverKitchen → Optimize for WebGL
3. Увидеть зелёное сообщение ✅ об успехе
```

### 2️⃣ Сжать спрайты (15 минут)
```
1. Открить Assets/Sprites/Products/ в Project
2. Для каждого PNG файла:
   - Select файл
   - Inspector → TextureImporter
   - Compression: Compressed (ASTC 6x6) или RGBA Compressed
   - Max Size: 1024
   - Apply
3. Проверить размер: должен уменьшиться на 60-70%
```

### 3️⃣ Создать Sprite Atlases (20 минут)
```
1. Tools → OverKitchen → Create Sprite Atlases (прочитает инструкцию)
2. Создать в Assets/Sprites/Atlases/:
   - UI_Atlas (меню, кнопки, счёт)
   - Pieces_Atlas (фишки - 8 типов)
   - Effects_Atlas (взрывы, бонусы)
3. Переопубликовать спрайты в коде (если надо обновить ссылки)
```

### 4️⃣ Тестовая сборка WebGL (30 минут)
```
1. File → Build Settings
2. Add Open Scenes (добавить текущие сцены)
3. Switch Platform → WebGL
4. Build
5. Открить index.html в браузере
6. Проверить:
   - Загружается < 3 сек
   - FPS 55-60
   - Нет ошибок в Console
```

---

## 📊 Ожидаемые результаты

| До | После | Улучшение |
|----|-------|-----------|
| Sprites: 2.2MB | ~600KB | -73% ✅ |
| Build: ~100MB | ~30MB | -70% ✅ |
| Load Time: 5s | 2s | -60% ✅ |
| FPS: 40 | 60 | +50% ✅ |

---

## ⚡ Тонкие настройки (опционально)

Если после шагов 1-4 всё работает хорошо, можно дальше не идти.

Если есть проблемы с производительностью:
- Отключить Anti-aliasing (Quality Settings)
- Уменьшить shadow distance
- Отключить VSync (if needed)

---

## 🔗 Что дальше?

После оптимизации WebGL:
→ Фаза 3: **Интеграция Яндекс SDK** (авторизация, сохранения, реклама)

---

**Created:** 2026-05-16  
**For:** OverKitchen Yandex Games Release  
**Status:** Ready to implement in Unity
