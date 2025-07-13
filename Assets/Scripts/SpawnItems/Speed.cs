using UnityEngine;

public class Speed : MonoBehaviour
{

    [SerializeField] private float speedMultiplier = 2f;
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
            other.gameObject.GetComponent<SnakeController>().GetSpeedBoost(speedMultiplier);
        }
    }
}