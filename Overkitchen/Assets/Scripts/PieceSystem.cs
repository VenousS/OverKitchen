using System.Collections;
using UnityEngine;

public class PieceSystem : MonoBehaviour
{
    private GameManager gameManager;

    public int x { get; private set; }
    public int y { get; private set; }

    [SerializeField] private int pieceType;  // теперь можно увидеть в инспекторе
    private GameManager.BonusType bonus = GameManager.BonusType.None;

    [Header("References")]
    [SerializeField] private SpriteRenderer sr;

    [Header("VFX")]
    public ParticleSystem destroyEffect;
    public ParticleSystem bonusCreateEffect;
    public ParticleSystem bombRowFX;
    public ParticleSystem bombColumnFX;
    public ParticleSystem bombColorFX;

    private bool isAnimating = false;

    // ---------------------------------------------------------
    // INIT
    // ---------------------------------------------------------
    public void Init(GameManager gm, int startX, int startY, int typeIndex, GameManager.BonusType bonusType)
    {
        gameManager = gm;
        x = startX;
        y = startY;
        pieceType = typeIndex;
        bonus = bonusType;

        if (sr == null)
            sr = GetComponent<SpriteRenderer>();
    }

    public void UpdateCoord(int newX, int newY)
    {
        x = newX;
        y = newY;
    }

    // ---------------------------------------------------------
    // NEW: доступ к типу фишки
    // ---------------------------------------------------------
    public int GetPieceType()
    {
        return pieceType;
    }

    // Альтернативно можно использовать свойство
    public int PieceType => pieceType;

    // ---------------------------------------------------------
    // BONUS SYSTEM
    // ---------------------------------------------------------
    public void SetBonus(GameManager.BonusType newBonus)
    {
        bonus = newBonus;
        StartCoroutine(BonusAppearAnimation());
        sr.color = new Color(1.2f, 1.2f, 1.2f, 1f);
    }

    public bool HasBonus() => bonus != GameManager.BonusType.None;
    public GameManager.BonusType GetBonus() => bonus;

    // ---------------------------------------------------------
    // VISUAL EFFECTS
    // ---------------------------------------------------------
    private IEnumerator BonusAppearAnimation()
    {
        if (bonusCreateEffect != null)
            Instantiate(bonusCreateEffect, transform.position, Quaternion.identity);

        float t = 0;
        Vector3 original = transform.localScale;
        Vector3 maxScale = original * 1.35f;

        while (t < 0.15f)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(original, maxScale, t / 0.15f);
            yield return null;
        }

        t = 0;
        while (t < 0.15f)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(maxScale, original, t / 0.15f);
            yield return null;
        }

        transform.localScale = original;
    }

    public void PlayBonusExplosion()
    {
        switch (bonus)
        {
            case GameManager.BonusType.BombRow:
                if (bombRowFX != null)
                    Instantiate(bombRowFX, transform.position, Quaternion.identity);
                break;

            case GameManager.BonusType.BombColumn:
                if (bombColumnFX != null)
                    Instantiate(bombColumnFX, transform.position, Quaternion.identity);
                break;

            case GameManager.BonusType.BombColor:
                if (bombColorFX != null)
                    Instantiate(bombColorFX, transform.position, Quaternion.identity);
                break;
        }
    }

    public void PlayMatchEffect()
    {
        if (bonus != GameManager.BonusType.None)
            PlayBonusExplosion();

        if (destroyEffect != null)
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
    }

    // ---------------------------------------------------------
    // SWIPE CONTROL
    // ---------------------------------------------------------
    private Vector2 firstTouchPos;
    private Vector2 lastTouchPos;

    private void OnMouseDown()
    {
        firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        lastTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 delta = lastTouchPos - firstTouchPos;

        float dist = delta.magnitude;
        if (dist < 0.1f) return;

        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

        gameManager.MovePiece(this, angle, dist);
    }
}
