using UnityEngine;
using System.Collections.Generic;
using System;

public class SnakeController : MonoBehaviour
{
    // Событие для уведомления об изменении количества частей тела
    public event Action<int> OnTailCountChanged;

    [Header("Настройки")]
    [HideInInspector] public float MoveSpeed;
    private int eatMultiplier = 1;
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
        Grow();
        // Time.timeScale = 0f;
        currentGridPos = Vector2Int.RoundToInt(transform.position);
        transform.position = currentGridPos;

        InvokeRepeating(nameof(Move), MoveSpeed, MoveSpeed);
    }

    void Update()
    {
        // Ввод направления
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && moveDirection != Vector2.down)
        {
            inputDirection = Vector2.up;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && moveDirection != Vector2.up)
        {
            inputDirection = Vector2.down;
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && moveDirection != Vector2.right)
        {
            inputDirection = Vector2.left;
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && moveDirection != Vector2.left)
        {
            inputDirection = Vector2.right;
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }


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
        gameObject.tag = colorTag;

        Color newColor = Color.white;

        switch (colorTag)
        {
            case "Red": newColor = redColor; break;
            case "Yellow": newColor = yellowColor; break;
            case "Blue": newColor = blueColor; break;
            case "Green": newColor = greenColor; break;
        }

        sr.color = newColor;

        // Меняем цвет у всех частей тела
        foreach (var part in BodyParts)
        {
            var partSr = part.GetComponent<SpriteRenderer>();
            if (partSr != null)
                partSr.color = newColor;
        }
    }


    public void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f;
        Debug.Log("Score: " + FindObjectOfType<LeaderboardUI>());
        var score = BodyParts.Count - 1;
        FindObjectOfType<LeaderboardUI>().Show(score);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Border"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.Wall);
            GameOver();
        }
    }

    [SerializeField] private float segmentSpacing = 1f;

    public void Grow()
    {
        GameObject newPart = Instantiate(bodyPrefab);

        if (BodyParts.Count == 0)
        {
            // Первая часть: разместить сбоку от головы (например, вправо)
            Vector3 sideOffset = new Vector3(1.3f, 0.5f, 0f);
            newPart.transform.position = transform.position - sideOffset;
            BodyParts.Add(newPart.transform);
        }
        else
        {
            for (int i = 0; i < eatMultiplier; i++)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.Eat);
                Vector3 lastPartPos = BodyParts[^1].position;
                Vector3 secondToLastPos = BodyParts.Count > 1 ? BodyParts[^2].position : transform.position;
                Vector3 direction = (lastPartPos - secondToLastPos).normalized;
                newPart.transform.position = lastPartPos - direction * segmentSpacing;
                BodyParts.Add(newPart.transform);
            }
            // AudioManager.Instance.PlaySFX(AudioManager.Instance.Eat);
            // Vector3 lastPartPos = BodyParts[^1].position;
            // Vector3 secondToLastPos = BodyParts.Count > 1 ? BodyParts[^2].position : transform.position;
            // Vector3 direction = (lastPartPos - secondToLastPos).normalized;
            // newPart.transform.position = lastPartPos - direction * segmentSpacing;
        }


        OnTailCountChanged?.Invoke(BodyParts.Count - 1);

        ChangeColor(currentColorTag);
    }

    public void GetShield(Color shieldColor)
    {
        StartCoroutine(ShieldRoutine(shieldColor));
    }

    private System.Collections.IEnumerator ShieldRoutine(Color shieldColor)
    {
        Color originalColor = sr.color;
        sr.color = shieldColor;
        gameObject.tag = "Shield";
        yield return new WaitForSeconds(5f);
        sr.color = originalColor;
        gameObject.tag = currentColorTag;
    }

    public void GetEatBoost()
    {
        StartCoroutine(SpeedBoostRoutine());
    }

    private System.Collections.IEnumerator SpeedBoostRoutine()
    {
        eatMultiplier++;
        yield return new WaitForSeconds(3f);
        eatMultiplier--;
    }


}
