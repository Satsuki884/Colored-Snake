using UnityEngine;
using System.Collections.Generic;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject bodyPrefab;
    [SerializeField] private Color redColor, yellowColor, blueColor, greenColor;

    private Vector2 moveDirection = Vector2.right;
    private List<Transform> bodyParts = new List<Transform>();
    private SpriteRenderer sr;
    private string currentColorTag = "Red";

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ChangeColor("Red");
        InvokeRepeating(nameof(Move), 0.2f, 0.2f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) ChangeColor("Red");
        if (Input.GetKeyDown(KeyCode.Y)) ChangeColor("Yellow");
        if (Input.GetKeyDown(KeyCode.B)) ChangeColor("Blue");
        if (Input.GetKeyDown(KeyCode.G)) ChangeColor("Green");

        if (Input.GetKeyDown(KeyCode.W)) moveDirection = Vector2.up;
        if (Input.GetKeyDown(KeyCode.S)) moveDirection = Vector2.down;
        if (Input.GetKeyDown(KeyCode.A)) moveDirection = Vector2.left;
        if (Input.GetKeyDown(KeyCode.D)) moveDirection = Vector2.right;
    }

    void FixedUpdate()
    {
        transform.Translate(moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    void Move()
    {
        Vector3 prevPos = transform.position;
        transform.Translate(moveDirection);

        foreach (Transform part in bodyParts)
        {
            Vector3 temp = part.position;
            part.position = prevPos;
            prevPos = temp;
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
        newPart.transform.position = bodyParts.Count == 0 ? transform.position : bodyParts[bodyParts.Count - 1].position;
        bodyParts.Add(newPart.transform);
    }
}
