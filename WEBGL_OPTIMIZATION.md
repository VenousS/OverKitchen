# 🌐 WebGL Optimization Guide для Яндекс Игры

## 📊 Текущее состояние

| Папка | Размер | Статус |
|-------|--------|--------|
| Sprites (Products) | 1.1M | 🔴 Требует сжатия |
| ProductsV2.png | 762K | 🟡 Можно оптимизировать |
| Products folder | 389K | 🟢 OK |
| Scripts | 107K | 🟢 OK |
| Effects | 126K | 🟡 Требует проверки |

**Итого:** ~2.5MB (для WebGL должно быть <5MB, идеально <3MB)

---

## 🎯 Фаза 2.1: Сжатие ассетов

### Шаг 1: Компрессия спрайтов
```
Для каждого .png в Products/:
1. Экспортировать в WebP формат (сжимает на 25-35%)
2. Или использовать PNG с максимальной компрессией
3. Установить TextureImporter settings:
   - Format: RGBA Compressed (ASTC 6x6) или ETC2/ETC2+Alpha
   - Max Size: 1024 для UI, 512 для мелких элементов
   - Compression: High
```

### Шаг 2: Удаление дубликатов
```
- ProductsV2.png vs Products/ - оставить только один набор
- Удалить неиспользуемые текстуры
```

### Шаг 3: Атласы текстур (Sprite Atlas)
```
Создать SpriteAtlas для группировки по типам:
- UI_Atlas (меню, кнопки, иконки)
- Pieces_Atlas (фишки игры)
- Effects_Atlas (бонусы, взрывы)
```

---

## ⚙️ Фаза 2.2: WebGL Build Settings

### Сценарий для ProjectSettings:
```json
{
  "WebGL": {
    "Resolution": {
      "Width": 960,
      "Height": 600
    },
    "Quality": "Fast",
    "Compression": "Brotli",
    "TemplateVariants": "Minimal",
    "MemorySize": 256,
    "ExceptionSupport": "None",
    "namingStrategy": "Hashed"
  }
}
```

### Ключевые параметры:
| Параметр | Значение | Причина |
|----------|----------|---------|
| Quality Level | Fastest | Минимум расчетов |
| Anti-aliasing | Disabled | Сэкономить память |
| VSync | Disabled | Лучше на браузерах |
| Target Frame Rate | 60 | Стабильность |
| GPU Instancing | Enabled | Производительность |
| Compression | Brotli | Лучше сжимает |

---

## 🎨 Фаза 2.3: Оптимизация сцен

### Шаг 1: Canvas optimization
```csharp
// GameHUD.cs, LevelCompleteUI.cs и т.д.
public class OptimizedUIPanel : MonoBehaviour 
{
    private CanvasGroup canvasGroup; // Вместо visibility
    private GraphicRaycaster raycaster;
    
    void Start()
    {
        // Отключить RaycastTarget на фоновых элементах
        GetComponent<Image>().raycastTarget = false;
    }
}
```

### Шаг 2: LOD Groups для спрайтов
```
Для больших сцен использовать LOD:
- Уменьшить resolution спрайтов на расстоянии
- Отключить компоненты дальних объектов
```

### Шаг 3: Object pooling (уже в GameManager)
```csharp
// Переиспользовать объекты вместо Create/Destroy
private Queue<Piece> piecePool = new Queue<Piece>();

Piece GetPiece()
{
    return piecePool.Count > 0 ? piecePool.Dequeue() : Instantiate(piecePrefab);
}

void ReturnPiece(Piece piece)
{
    piece.gameObject.SetActive(false);
    piecePool.Enqueue(piece);
}
```

---

## 🔧 Фаза 2.4: Shader Optimization

### Шаг 1: Использовать Mobile шейдеры
```shader
// Использовать вместо Standard:
Shader "Mobile/Diffuse"
Shader "Mobile/VertexLit"
// Или упрощенные версии для UI
```

### Шаг 2: Отключить ненужные эффекты
```csharp
// AudioManager.cs
#if UNITY_WEBGL
    // Упростить обработку звука
    audioSource.spatialBlend = 0; // 2D звук вместо 3D
#endif
```

---

## 📦 Фаза 2.5: Code Stripping

Добавить в `link.xml`:
```xml
<linker>
    <assembly fullname="Assembly-CSharp" preserve="partial">
        <type fullname="*" preserve="all" when="instantiated"/>
    </assembly>
</linker>
```

---

## ✅ Чеклист оптимизации

### До сборки:
- [ ] Скомпрессированы все PNG (RGBA Compressed)
- [ ] Max texture size = 1024
- [ ] Созданы Sprite Atlas
- [ ] Удалены неиспользуемые ассеты
- [ ] Отключен Debug mode в Build
- [ ] Development Build = OFF

### WebGL Settings:
- [ ] Quality Level = Fastest
- [ ] Strip unused Mesh components = ON
- [ ] Strip unused Variants = ON
- [ ] Memory: 256MB
- [ ] Compression: Brotli

### Код:
- [ ] Object pooling для пулов фишек
- [ ] Canvas optimization (raycastTarget, batching)
- [ ] Удалены Debug.Log в продакшене
- [ ] Использованы mobile шейдеры

### Размеры после оптимизации:
```
Target: 
- Build size: < 50MB (сжато < 20MB)
- Загрузка сцены: < 2 сек
- FPS: 60 стабильно
```

---

## 🚀 Скрипт автоматизации

Создать `Assets/Editor/WebGLOptimizer.cs`:
```csharp
using UnityEditor;

public class WebGLOptimizer
{
    [MenuItem("Tools/Optimize for WebGL")]
    public static void OptimizeForWebGL()
    {
        // 1. Сжать текстуры
        CompressTextures();
        
        // 2. Настроить качество
        QualitySettings.SetQualityLevel(0);
        
        // 3. Сообщить статус
        EditorUtility.DisplayDialog("✅", "WebGL оптимизация завершена!", "OK");
    }
    
    private static void CompressTextures()
    {
        // Implementation...
    }
}
```

---

## 📈 Результаты (ожидаемые)

| Метрика | До | После | Улучшение |
|---------|----|----|-----------|
| Build Size | ~100MB | ~30MB | -70% |
| Compressed | ~40MB | ~12MB | -70% |
| Load Time | 5-7s | 1-2s | -70% |
| Memory (Runtime) | ~400MB | ~250MB | -38% |
| FPS (WebGL) | 30-45 | 55-60 | +80% |

---

## 📚 Ресурсы

- [Unity WebGL Build Settings](https://docs.unity3d.com/Manual/webgl-building.html)
- [Asset Optimization Best Practices](https://docs.unity3d.com/Manual/OptimizingGraphicsPerformance.html)
- [Texture Compression WebGL](https://docs.unity3d.com/Manual/class-TextureImporter.html)

---

**Версия:** 1.0  
**Дата:** 2026-05-16  
**Статус:** Ready to Implement
