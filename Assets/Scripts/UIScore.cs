using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    SnakeController snakeController;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text recordText;

    void Start()
    {
        snakeController = FindObjectOfType<SnakeController>();
        if (snakeController != null)
        {
            snakeController.OnTailCountChanged += UpdateScoreUI;
        }
        recordText.text = PlayerPrefs.GetInt("Record", 0).ToString();
        scoreText.text = "0";
    }

    void OnDestroy()
    {
        if (snakeController != null)
        {
            snakeController.OnTailCountChanged -= UpdateScoreUI;
        }
    }

    void UpdateScoreUI(int tailCount)
    {
        Debug.Log("UpdateScoreUI called with tailCount: " + tailCount);
        if (scoreText != null)
        {
            scoreText.text = tailCount.ToString();
        }

        if (tailCount > PlayerPrefs.GetInt("Record", 0))
        {
            recordText.text = tailCount.ToString();
        } else
        {
            recordText.text = PlayerPrefs.GetInt("Record", 0).ToString();
        }
    }
}
