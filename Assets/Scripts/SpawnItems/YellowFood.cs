using UnityEngine;

public class YellowFood : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (
            other.CompareTag("Red")||
            other.CompareTag("Blue") ||
            other.CompareTag("Green"))
        {
            other.gameObject.GetComponent<SnakeController>().GameOver();
        }
        else if (other.CompareTag("Yellow") || other.CompareTag("Shield"))
        {
            FindObjectOfType<BlockSpawner>().RemoveBlock(gameObject);
            Destroy(gameObject);
            other.gameObject.GetComponent<SnakeController>().Grow();
        }
    }
}