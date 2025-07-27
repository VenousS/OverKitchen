using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private int gridSize = 8;
    [SerializeField] private GameObject[] piecePrefabs;
    private GameObject[,] piecies;
    private bool IsSwapping;
    private float swapDuration = 0.3f;
    [Header("Camera Settings")]
    [SerializeField] private float padding = 1f;
    void Start()
    {
        InitializeGrid();
        CenterCamera();
    }

    void InitializeGrid()
    {
        piecies = new GameObject[gridSize, gridSize];
        for (int y = 0; y < gridSize; y++) { 
            for (int x = 0; x < gridSize; x++)
            {
                CreatePiece(x, y);
            }
        }
    }
    void CenterCamera()
    {
        Camera mainCamera = Camera.main;

        float centerX = (gridSize - 1) * 0.5f;
        float centerY = (gridSize - 1) * 0.5f;
        mainCamera.transform.position = new Vector3(centerX, centerY, -10f);
        float aspectRatio = (float)Screen.width / Screen.height;
        float verticalSize = (gridSize + padding) * 0.5f;
        float horizantaleSize = (gridSize + padding) * 0.5f / aspectRatio;
        mainCamera.orthographicSize = Mathf.Max(verticalSize, horizantaleSize);
    }
    void CreatePiece(int x, int y)
    {
        GameObject newPiece = Instantiate(piecePrefabs[Random.Range(0, piecePrefabs.Length)], new Vector3(x, y, 0), Quaternion.identity);
        newPiece.GetComponent<PieceSystem>().x = x;
        newPiece.GetComponent<PieceSystem>().y = y;
        piecies[x, y] = newPiece;
    }

    public void MovePiece(PieceSystem piece, float swipeAngle)
    {
        if (IsSwapping) return;
        int targetX = piece.x;
        int targetY = piece.y;
        if (swipeAngle > -45 && swipeAngle <= 45) targetX++;
        else if (swipeAngle > 45 && swipeAngle <= 135) targetY++;
        else if (swipeAngle > 135 ||  swipeAngle <= -135) targetX--;
        else targetY--;

        if (targetX >= 0 && targetX < gridSize && targetY >= 0 && targetY < gridSize)
        {
            StartCoroutine(SwapPiecies(piece.x, piece.y, targetX, targetY));
        }
    }
    IEnumerator SwapPiecies(int x1, int y1, int x2, int y2)
    {
        IsSwapping = true;
        GameObject piece1 = piecies[x1, y1];
        GameObject piece2 = piecies[x2, y2];
        piecies[x1, y1] = piece2;
        piecies[x2, y2] = piece1;
        piece1.GetComponent<PieceSystem>().x = x2;
        piece1.GetComponent<PieceSystem>().y = y2;
        piece2.GetComponent<PieceSystem>().x = x1;
        piece2.GetComponent<PieceSystem>().y = y1;

        float elapsed = 0;
        Vector3 startPos1 = piece1.transform.position;
        Vector3 startPos2 = piece2.transform.position;

        while (elapsed < swapDuration) { 
            elapsed += Time.deltaTime;
            float t = elapsed / swapDuration;
            piece1.transform.position = Vector3.Lerp(startPos1, startPos2, t);
            piece2.transform.position = Vector3.Lerp(startPos2, startPos1, t);

            yield return null;
        }
        piece1.transform.position = startPos2;
        piece2.transform.position = startPos1;
        IsSwapping = false;
    }
}
