using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Button closeTutorialButton;
    [SerializeField] private Button leaderBoardButton;
    [SerializeField] private GameObject leaderBoardPanel;
    [SerializeField] private Button closeLeaderBoardButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        tutorialPanel.SetActive(false);
        leaderBoardPanel.SetActive(false);
        playButton.onClick.AddListener(OnPlayButtonClicked);
        tutorialButton.onClick.AddListener(() => tutorialPanel.SetActive(true));
        closeTutorialButton.onClick.AddListener(() => tutorialPanel.SetActive(false));
        closeLeaderBoardButton.onClick.AddListener(() => leaderBoardPanel.SetActive(false));
        leaderBoardButton.onClick.AddListener(OnLeaderBoardButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("Game");
    }

    private void OnLeaderBoardButtonClicked()
    {
        leaderBoardPanel.SetActive(true);
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}