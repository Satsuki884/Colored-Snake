using UnityEngine;

public class BlueFood : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (
            other.CompareTag("Yellow") ||
            other.CompareTag("Red") ||
            other.CompareTag("Green"))
        {
            other.gameObject.GetComponent<SnakeController>().GameOver();
        }
        else if (other.CompareTag("Blue") || other.CompareTag("Shield"))
        {
            FindObjectOfType<BlockSpawner>().RemoveBlock(gameObject);
            Destroy(gameObject);
            other.gameObject.GetComponent<SnakeController>().Grow();
        }
    }
}