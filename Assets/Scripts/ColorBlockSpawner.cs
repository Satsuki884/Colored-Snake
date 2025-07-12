using UnityEngine;
using System.Collections.Generic;

public class ColorBlockSpawner : MonoBehaviour
{
    public GameObject redPrefab, yellowPrefab, bluePrefab, greenPrefab;
    public float spawnInterval = 3f;

    private GridManager gridManager;
    private SnakeHead snake;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        snake = FindObjectOfType<SnakeHead>();

        InvokeRepeating(nameof(SpawnBlock), 1f, spawnInterval);
    }

    void SpawnBlock()
    {
        GameObject prefab = GetRandomPrefab();
        int maxAttempts = 50;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector2Int gridPos = new Vector2Int(
                Random.Range(0, gridManager.gridSize.x),
                Random.Range(0, gridManager.gridSize.y)
            );

            Vector2 worldPos = gridManager.origin + new Vector2(gridPos.x + 0.5f, gridPos.y + 0.5f);

            if (!IsOccupied(worldPos))
            {
                Instantiate(prefab, worldPos, Quaternion.identity);
                break;
            }
        }
    }

    GameObject GetRandomPrefab()
    {
        int r = Random.Range(0, 3);
        return (r == 0) ? redPrefab : (r == 1) ? yellowPrefab : bluePrefab;
    }

    bool IsOccupied(Vector2 position)
    {
        if (snake == null) return false;

        // Проверка головы
        if (Vector2.Distance(position, snake.transform.position) < 0.4f)
            return true;

        // Проверка частей тела
        foreach (Transform part in snake.BodyParts)
        {
            if (Vector2.Distance(position, part.position) < 0.4f)
                return true;
        }

        return false;
    }
}
