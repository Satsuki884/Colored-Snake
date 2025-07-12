using UnityEngine;
using System.Collections.Generic;

public class SnakeHead : MonoBehaviour
{

    [Header("Настройки")]
    [HideInInspector] public float MoveSpeed;
    [SerializeField] private GameObject bodyPrefab;

    [Header("Цвета")]
    [SerializeField] private Color redColor;
    [SerializeField] private Color yellowColor;
    [SerializeField] private Color blueColor;
    [SerializeField] private Color greenColor;
    private SpriteRenderer sr;
    public List<Transform> BodyParts = new List<Transform>();

    private Vector2 moveDirection = Vector2.right;
    private Vector2 inputDirection = Vector2.right;
    private string currentColorTag = "Red";
    private Vector2 currentGridPos;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ChangeColor("Red");
        currentGridPos = Vector2Int.RoundToInt(transform.position);
        transform.position = currentGridPos;

        InvokeRepeating(nameof(Move), MoveSpeed, MoveSpeed);
    }

    void Update()
    {
        // Ввод направления
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && moveDirection != Vector2.down)
            inputDirection = Vector2.up;
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && moveDirection != Vector2.up)
            inputDirection = Vector2.down;
        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && moveDirection != Vector2.right)
            inputDirection = Vector2.left;
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && moveDirection != Vector2.left)
            inputDirection = Vector2.right;

        // Смена цвета
        if (Input.GetKeyDown(KeyCode.R)) ChangeColor("Red");
        if (Input.GetKeyDown(KeyCode.Y)) ChangeColor("Yellow");
        if (Input.GetKeyDown(KeyCode.B)) ChangeColor("Blue");
        if (Input.GetKeyDown(KeyCode.G)) ChangeColor("Green");
    }
    void Move()
    {
        // Применяем направление только в начале клетки
        if (inputDirection != moveDirection)
        {
            moveDirection = inputDirection;
        }

        Vector2 nextPosition = currentGridPos + moveDirection;

        // Двигаем тело
        if (BodyParts.Count > 0)
        {
            for (int i = BodyParts.Count - 1; i > 0; i--)
            {
                BodyParts[i].position = BodyParts[i - 1].position;
            }
            BodyParts[0].position = currentGridPos;
        }

        currentGridPos = nextPosition;
        transform.position = currentGridPos;
    }

    void ChangeColor(string colorTag)
    {
        currentColorTag = colorTag;
        switch (colorTag)
        {

            case "Red": sr.color = redColor; break;
            case "Yellow": sr.color = yellowColor; break;
            case "Blue": sr.color = blueColor; break;
            case "Green": sr.color = greenColor; break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Border") ||
            (other.CompareTag("Red") && currentColorTag != "Red") ||
            (other.CompareTag("Yellow") && currentColorTag != "Yellow") ||
            (other.CompareTag("Blue") && currentColorTag != "Blue") ||
            (other.CompareTag("Green") && currentColorTag != "Green"))
        {
            Debug.Log("Game Over!");
            Time.timeScale = 0f;
            Debug.Log("Score: " + FindObjectOfType<LeaderboardUI>());
            FindObjectOfType<LeaderboardUI>().Show(BodyParts.Count);
        }
        else if (other.CompareTag(currentColorTag))
        {
            FindObjectOfType<ColorBlockSpawner>().RemoveBlock(other.gameObject);
            Destroy(other.gameObject);
            Grow();
        }
    }

    [SerializeField] private float segmentSpacing = 0.5f; // Расстояние между частями тела

    void Grow()
    {
        GameObject newPart = Instantiate(bodyPrefab);

        if (BodyParts.Count == 0)
        {
            // Первая часть: разместить сбоку от головы (например, вправо)
            Vector3 sideOffset = (Vector3)Vector2.Perpendicular(moveDirection.normalized) * segmentSpacing;
            newPart.transform.position = transform.position + sideOffset;
        }
        else
        {
            // Остальные части: на расстоянии segmentSpacing за последней частью
            Vector3 lastPartPos = BodyParts[^1].position;
            Vector3 secondToLastPos = BodyParts.Count > 1 ? BodyParts[^2].position : transform.position;
            Vector3 direction = (lastPartPos - secondToLastPos).normalized;
            newPart.transform.position = lastPartPos - direction * segmentSpacing;
        }

        BodyParts.Add(newPart.transform);
    }


}
