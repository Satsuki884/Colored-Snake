using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(15f);

        var spawner = FindObjectOfType<BlockSpawner>();
        if (spawner != null)
        {
            spawner.RemoveBlock(gameObject);
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (
            other.CompareTag("Yellow") ||
            other.CompareTag("Blue") ||
            other.CompareTag("Green") ||
            other.CompareTag("Red"))
        {
            FindObjectOfType<BlockSpawner>().RemoveBlock(gameObject);
            Destroy(gameObject);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.Bomb);
            other.gameObject.GetComponent<SnakeController>().GameOver();
        }
        else if (other.CompareTag("Shield"))
        {
            FindObjectOfType<BlockSpawner>().RemoveBlock(gameObject);
            Destroy(gameObject);
        }
    }
}