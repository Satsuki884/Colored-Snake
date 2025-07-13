using UnityEngine;

public class Shield : MonoBehaviour
{

    [SerializeField] private Color shieldColor;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (
            other.CompareTag("Yellow")||
            other.CompareTag("Blue") ||
            other.CompareTag("Green") ||
            other.CompareTag("Red"))
        {
            FindObjectOfType<BlockSpawner>().RemoveBlock(gameObject);
            Destroy(gameObject);
            other.gameObject.GetComponent<SnakeController>().GetShield(shieldColor);
        }
    }
}