using UnityEngine;
using System.Collections.Generic;

public class ColorBlockSpawner : MonoBehaviour
{
    public GameObject redPrefab, yellowPrefab, bluePrefab, greenPrefab;
    public float spawnInterval = 3f;

    private GridManager gridManager;
    private SnakeHead snake;

    private List<GameObject> spawnedBlocks = new List<GameObject>();
    private const int maxBlocks = 10;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        snake = FindObjectOfType<SnakeHead>();

        InvokeRepeating(nameof(SpawnBlock), 1f, spawnInterval);
    }

    void SpawnBlock()
    {
        if (spawnedBlocks.Count >= maxBlocks) return;
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
                GameObject newBlock = Instantiate(prefab, worldPos, Quaternion.identity);
                spawnedBlocks.Add(newBlock);
                break;
            }
        }
    }

    GameObject GetRandomPrefab()
    {
        int r = Random.Range(0, 4);
        return (r == 0) ? redPrefab : (r == 1) ? yellowPrefab : (r == 3) ? greenPrefab : bluePrefab;
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

        foreach (GameObject block in spawnedBlocks)
        {
            if (block == null) continue; // на случай если блок был собран
            if (Vector2.Distance(position, block.transform.position) < 0.4f)
                return true;
        }

        return false;
    }

    public void RemoveBlock(GameObject block)
    {
        if (spawnedBlocks.Contains(block))
        {
            spawnedBlocks.Remove(block);
            Destroy(block);
        }
    }
}
