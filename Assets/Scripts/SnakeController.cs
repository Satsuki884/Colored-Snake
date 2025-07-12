// using UnityEngine;
using System.Collections.Generic;

// public class SnakeController : MonoBehaviour
// {
//     [Header("Настройки")]
//     [SerializeField] private float moveSpeed = 0.2f;
//     [SerializeField] private GameObject bodyPrefab;

//     [Header("Цвета")]
//     [SerializeField] private Color redColor;
//     [SerializeField] private Color yellowColor;
//     [SerializeField] private Color blueColor;
//     [SerializeField] private Color greenColor;

//     private Rigidbody2D rb;
//     private SpriteRenderer sr;

//     private Vector2 moveDirection = Vector2.right;
//     private Vector2 inputDirection = Vector2.right;

//     private List<Transform> bodyParts = new List<Transform>();
//     private string currentColorTag = "Red";

//     private Vector3 lastHeadPos;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         sr = GetComponent<SpriteRenderer>();
//         ChangeColor("Red");
//         lastHeadPos = transform.position;
//     }

//     void Update()
//     {
//         if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && moveDirection != Vector2.down)
//             inputDirection = Vector2.up;
//         else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && moveDirection != Vector2.up)
//             inputDirection = Vector2.down;
//         else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && moveDirection != Vector2.right)
//             inputDirection = Vector2.left;
//         else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && moveDirection != Vector2.left)
//             inputDirection = Vector2.right;

//         // Смена цвета
//         if (Input.GetKeyDown(KeyCode.R)) ChangeColor("Red");
//         if (Input.GetKeyDown(KeyCode.Y)) ChangeColor("Yellow");
//         if (Input.GetKeyDown(KeyCode.B)) ChangeColor("Blue");
//         if (Input.GetKeyDown(KeyCode.G)) ChangeColor("Green");
//     }

//     void FixedUpdate()
//     {
//         moveDirection = inputDirection;
//         lastHeadPos = rb.position;

//         // Плавное движение
//         Vector2 newPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
//         rb.MovePosition(newPosition);

//         // Двигаем хвост
//         if (bodyParts.Count > 0)
//         {
//             Vector3 prevPos = lastHeadPos;
//             for (int i = 0; i < bodyParts.Count; i++)
//             {
//                 Vector3 temp = bodyParts[i].position;
//                 bodyParts[i].position = prevPos;
//                 prevPos = temp;
//             }
//         }
//     }

//     void ChangeColor(string colorTag)
//     {
//         currentColorTag = colorTag;
//         switch (colorTag)
//         {
//             case "Red": sr.color = redColor; break;
//             case "Yellow": sr.color = yellowColor; break;
//             case "Blue": sr.color = blueColor; break;
//             case "Green": sr.color = greenColor; break;
//         }
//     }

//     void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Border") ||
//             (other.CompareTag("Red") && currentColorTag != "Red") ||
//             (other.CompareTag("Yellow") && currentColorTag != "Yellow") ||
//             (other.CompareTag("Blue") && currentColorTag != "Blue") ||
//             (other.CompareTag("Green") && currentColorTag != "Green"))
//         {
//             Debug.Log("Game Over!");
//             Time.timeScale = 0f;
//         }
//         else if (other.CompareTag(currentColorTag))
//         {
//             Destroy(other.gameObject);
//             Grow();
//         }
//     }

//     void Grow()
//     {
//         GameObject newPart = Instantiate(bodyPrefab);
//         newPart.transform.position = bodyParts.Count == 0 ? transform.position : bodyParts[^1].position;
//         bodyParts.Add(newPart.transform);
//     }
// }

using UnityEngine;

public class SnakeHead : MonoBehaviour
{

    [Header("Настройки")]
    [SerializeField] private float moveSpeed = 0.2f;
    [SerializeField] private GameObject bodyPrefab;

    [Header("Цвета")]
    [SerializeField] private Color redColor;
    [SerializeField] private Color yellowColor;
    [SerializeField] private Color blueColor;
    [SerializeField] private Color greenColor;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private List<Transform> bodyParts = new List<Transform>();

    private Vector2 moveDirection = Vector2.right;
    private Vector2 inputDirection = Vector2.right;
    private Vector3 lastHeadPos;
    private string currentColorTag = "Red";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ChangeColor("Red");
        rb.gravityScale = 0;
        lastHeadPos = rb.position;
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
    }
    void FixedUpdate()
    {
        moveDirection = inputDirection;
        lastHeadPos = rb.position;

        // Плавное движение
        Vector2 newPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);

        // Двигаем хвост
        if (bodyParts.Count > 0)
        {
            Vector3 prevPos = lastHeadPos;
            for (int i = 0; i < bodyParts.Count; i++)
            {
                Vector3 temp = bodyParts[i].position;
                bodyParts[i].position = prevPos;
                prevPos = temp;
            }
        }
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
        }
        else if (other.CompareTag(currentColorTag))
        {
            Destroy(other.gameObject);
            Grow();
        }
    }

    void Grow()
    {
        GameObject newPart = Instantiate(bodyPrefab);
        newPart.transform.position = bodyParts.Count == 0 ? transform.position : bodyParts[^1].position;
        bodyParts.Add(newPart.transform);
    }

}
