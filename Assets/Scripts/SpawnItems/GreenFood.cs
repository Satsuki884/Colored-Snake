using UnityEngine;

public class GreenFood : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (
            other.CompareTag("Yellow")||
            other.CompareTag("Blue") ||
            other.CompareTag("Red"))
        {
            other.gameObject.GetComponent<SnakeController>().GameOver();
        }
        else if (other.CompareTag("Green") || other.CompareTag("Shield"))
        {
            FindObjectOfType<BlockSpawner>().RemoveBlock(gameObject);
            Destroy(gameObject);
            other.gameObject.GetComponent<SnakeController>().Grow();
        }
    }
}