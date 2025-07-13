using UnityEngine;

public class Buff : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (
            other.CompareTag("Yellow")||
            other.CompareTag("Blue") ||
            other.CompareTag("Green") ||
            other.CompareTag("Red"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.Speed);
            FindObjectOfType<BlockSpawner>().RemoveBlock(gameObject);
            Destroy(gameObject);
            other.gameObject.GetComponent<SnakeController>().GetEatBoost();
        }
    }
}