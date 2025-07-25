using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject snakePrefab;
    [SerializeField] private int max = 30;
    [SerializeField] private int min = 5;
    [SerializeField] private GameObject wallsParent;

    [HideInInspector] public Vector2Int gridSize;
    [HideInInspector] public Vector2 origin;

    void Awake()
    {
        gridSize = new Vector2Int(
            GetRandomOdd(),
            GetRandomOdd()
        );

        origin = -new Vector2(gridSize.x, gridSize.y) * 0.5f;

        AdjustCamera();
        GenerateBorders();
        SpawnSnake();
    }

    int GetRandomOdd()
    {
        int randomHalf = Random.Range(min, max + 1);
        if(randomHalf % 2 == 0)
        {
            randomHalf++;
        }
        return randomHalf;
    }

    void GenerateBorders()
    {
        for (int x = -1; x <= gridSize.x; x++)
        {
            for (int y = -1; y <= gridSize.y; y++)
            {
                if (x == -1 || y == -1 || x == gridSize.x || y == gridSize.y)
                {
                    Vector2 pos = origin + new Vector2(x + 0.5f, y + 0.5f);
                    var wall = Instantiate(wallPrefab, pos, Quaternion.identity);
                    wall.transform.SetParent(wallsParent.transform);
                }
            }
        }
    }

    void SpawnSnake()
    {
        Vector2 spawnPos = new Vector2(0.5f, 0.5f);
        GameObject snake = Instantiate(snakePrefab, spawnPos, Quaternion.Euler(0, 0, -90));

        // Устанавливаем скорость в компоненте змейки
        float speed = CalculateSnakeSpeed();
        SnakeController head = snake.GetComponent<SnakeController>();
        if (head != null)
        {
            head.MoveSpeed = speed;
            // head.BodyParts.Add(snake.transform);
        }
    }


    void AdjustCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null || !mainCamera.orthographic) return;

        float aspect = (float)Screen.width / Screen.height;

        float verticalSize = gridSize.y * 0.5f + 2f;

        float horizontalSize = (gridSize.x * 0.5f + 2f) / aspect;

        mainCamera.orthographicSize = Mathf.Max(verticalSize, horizontalSize);

        Vector3 center = origin + new Vector2(gridSize.x, gridSize.y) * 0.5f;
        mainCamera.transform.position = new Vector3(center.x, center.y, -10f);
    }

    float CalculateSnakeSpeed()
    {
        float sizeFactor = (gridSize.x + gridSize.y) * 0.5f;

        float t = Mathf.InverseLerp(min, max, sizeFactor);

        return Mathf.Lerp(0.2f, 0.4f, t);
    }


}
