using UnityEngine;

public class ColorBlockSpawner : MonoBehaviour
{
    public GameObject redPrefab, yellowPrefab, bluePrefab;
    public float spawnInterval = 3f;
    public Vector2 spawnAreaMin, spawnAreaMax;

    void Start()
    {
        InvokeRepeating(nameof(SpawnBlock), 1f, spawnInterval);
    }

    void SpawnBlock()
    {
        int r = Random.Range(0, 3);
        GameObject prefab = (r == 0) ? redPrefab : (r == 1) ? yellowPrefab : bluePrefab;
        Vector2 spawnPos = new Vector2(Random.Range(spawnAreaMin.x, spawnAreaMax.x), Random.Range(spawnAreaMin.y, spawnAreaMax.y));
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
