using UnityEngine;

public class RedFood : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (
            other.CompareTag("Yellow")||
            other.CompareTag("Blue") ||
            other.CompareTag("Green"))
        {
            other.gameObject.GetComponent<SnakeController>().GameOver();
        }
        else if (other.CompareTag("Red") || other.CompareTag("Shield"))
        {
            FindObjectOfType<BlockSpawner>().RemoveBlock(gameObject);
            Destroy(gameObject);
            other.gameObject.GetComponent<SnakeController>().Grow();
        }
    }
}