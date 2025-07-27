using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSystem : MonoBehaviour
{
    [SerializeField] private int pieceType;
    public int x;
    public int y;
    private bool isMatched;
    private GameManager gameManager;
    private Vector2 firstTouchPos;
    private Vector2 lastTouchPos;
    private float swipeAngle = 0;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnMouseDown()
    {
        firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseUp() {
        lastTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }
    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(lastTouchPos.y - firstTouchPos.y, lastTouchPos.x - firstTouchPos.x) * 180 / Mathf.PI;
        gameManager.MovePiece(this, swipeAngle);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(firstTouchPos, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(lastTouchPos, 0.1f);
    }
}
