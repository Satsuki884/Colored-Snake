using UnityEngine;
using System.Collections.Generic;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> colorBlocksPrefabs;
    [SerializeField] private List<GameObject> additionalBlocksPrefabs;
    [SerializeField] private GameObject blocksParent;

    [SerializeField] private float spawnInterval = 3f;

    private GridManager gridManager;
    private SnakeController snake;

    private List<GameObject> spawnedBlocks = new List<GameObject>();
    private const int maxBlocks = 10;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        snake = FindObjectOfType<SnakeController>();

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
                newBlock.transform.SetParent(blocksParent.transform);
                spawnedBlocks.Add(newBlock);
                break;
            }
        }
    }

    GameObject GetRandomPrefab()
    {
        float chance = Random.value; // от 0.0 до 1.0

        if (chance < 0.9f && colorBlocksPrefabs.Count > 0)
        {
            int index = Random.Range(0, colorBlocksPrefabs.Count);
            return colorBlocksPrefabs[index];
        }
        else if (additionalBlocksPrefabs.Count > 0)
        {
            int index = Random.Range(0, additionalBlocksPrefabs.Count);
            return additionalBlocksPrefabs[index];
        }
        return null;
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
            if (block == null) continue;
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
