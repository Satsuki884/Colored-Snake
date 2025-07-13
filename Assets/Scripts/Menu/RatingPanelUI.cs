using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class RatingPanelUI : MonoBehaviour
{
    public TMP_Text topScoresText;
    public TMP_Text playerRankText;
    [SerializeField] private LeaderboardManager leaderboardManager;

    private void Start()
    {
        Show();
    }

    void ClosePanel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Show()
    {
        topScoresText.text = "Loading...";

        string savedName = PlayerPrefs.GetString("player_name", "");

        if (string.IsNullOrEmpty(savedName))
        {
            playerRankText.text = "- - -";
        }
        else
        {
            playerRankText.text = "Loading...";
            StartCoroutine(leaderboardManager.GetCurrentPlayerRank(savedName, playerRankText));
        }

        StartCoroutine(GetTop10Scores());
    }
    private IEnumerator GetTop10Scores()
    {
        yield return leaderboardManager.GetTopScores(10, topScoresText);
    }
}
