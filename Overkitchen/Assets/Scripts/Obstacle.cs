using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Obstacle Settings")]
    [SerializeField] private int hp = 2; // сколько ударов нужно

    public int x;
    public int y;

    [Header("Visual")]
    public SpriteRenderer sr;
    public Sprite[] damageStages;
    // damageStages[0] Ц нет урона
    // damageStages[1] Ц немного сломано
    // damageStages[last] Ц почти разрушено

    private void Awake()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    public void Init(int X, int Y, int health)
    {
        x = X;
        y = Y;
        hp = health;

        UpdateSprite();
    }

    // ==========================
    // ”дар по преп€тствию
    // ==========================
    public void TakeHit()
    {
        hp--;
        UpdateSprite();

        if (hp <= 0)
        {
            DestroyObstacle();
        }
    }

    // ==========================
    // ∆иво ли преп€тствие?
    // ==========================
    public bool IsAlive()
    {
        return hp > 0;
    }

    // ==========================
    // ќбновление спрайта по HP
    // ==========================
    private void UpdateSprite()
    {
        if (damageStages == null || damageStages.Length == 0) return;

        int stageIndex = Mathf.Clamp(damageStages.Length - hp, 0, damageStages.Length - 1);
        sr.sprite = damageStages[stageIndex];
    }

    // ==========================
    // ”ничтожение преп€тстви€
    // ==========================
    private void DestroyObstacle()
    {
        Destroy(gameObject);
    }
}
