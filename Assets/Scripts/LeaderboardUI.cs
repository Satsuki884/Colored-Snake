using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text topScoresText;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Color errorColor;
    [SerializeField] private Button submitButton;
    [SerializeField] private TMP_Text playerRankText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private int currentScore;
    [SerializeField] private LeaderboardManager leaderboardManager;

    private void Start()
    {
        panel.SetActive(false);
        restartButton.onClick.AddListener(ReturnLevel);
        mainMenuButton.onClick.AddListener(() => SceneManager.LoadScene("Menu"));
    }

    void ReturnLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Show(int score)
    {
        currentScore = score;
        panel.SetActive(true);
        topScoresText.text = "Loading...";

        string savedName = PlayerPrefs.GetString("player_name", "");

        if (string.IsNullOrEmpty(savedName))
        {
            playerRankText.gameObject.SetActive(false);
            nameInputField.gameObject.SetActive(true);
            submitButton.gameObject.SetActive(true);
            submitButton.onClick.AddListener(OnSubmitClicked);
            PlayerPrefs.SetInt("Record", score);
            PlayerPrefs.Save();
        }
        else
        {
            playerRankText.gameObject.SetActive(true);
            playerRankText.text = $"Your result is not in the top, but it is saved: {savedName} - {score}.";
            nameInputField.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(false);
            if (score > PlayerPrefs.GetInt("Record", 0))
            {
                PlayerPrefs.SetInt("Record", score);
                PlayerPrefs.Save();
                UpdateScore(savedName, score);
            }

        }

        StartCoroutine(GetTop10Scores());
    }

    private void OnSubmitClicked()
    {
        string enteredName = nameInputField.text;
        if (!string.IsNullOrEmpty(enteredName))
        {
            StartCoroutine(leaderboardManager.CheckUsernameExists(enteredName, (exists) =>
                {
                    if (exists)
                    {
                        nameInputField.text = "";
                        nameInputField.placeholder.GetComponent<TMP_Text>().text = "Name taken. Try another";
                        nameInputField.placeholder.GetComponent<TMP_Text>().color = errorColor;
                    }
                    else
                    {
                        PlayerPrefs.SetString("player_name", enteredName);
                        nameInputField.gameObject.SetActive(false);
                        submitButton.gameObject.SetActive(false);
                        playerRankText.gameObject.SetActive(true);
                        SubmitScore(enteredName);
                    }
                }));
        }
    }

    private void SubmitScore(string name)
    {
        StartCoroutine(SubmitAndShowRank(name, currentScore));
    }

    private IEnumerator SubmitAndShowRank(string name, int score)
    {
        yield return StartCoroutine(leaderboardManager.SubmitScore(name, score));
        yield return StartCoroutine(leaderboardManager.GetPlayerRank(name, score, playerRankText));
        yield return StartCoroutine(GetTop10Scores());
    }

    private void UpdateScore(string name, int score)
    {
        StartCoroutine(UpdateAndShowRank(name, score));
    }

    private IEnumerator UpdateAndShowRank(string name, int score)
    {
        yield return StartCoroutine(leaderboardManager.UpdateScore(score));
        yield return StartCoroutine(leaderboardManager.GetPlayerRank(name, score, playerRankText));
        yield return StartCoroutine(GetTop10Scores());
    }

    private IEnumerator GetTop10Scores()
    {
        yield return leaderboardManager.GetTopScores(5, topScoresText);
    }
}
