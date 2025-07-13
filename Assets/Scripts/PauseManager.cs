using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button restartButton;
    void Start()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        pauseButton.onClick.AddListener(TogglePauseMenu);
        continueButton.onClick.AddListener(ContinueGame);
        menuButton.onClick.AddListener(OpenMainMenu);
        restartButton.onClick.AddListener(RestartGame);
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OpenMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    private void ContinueGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    private void TogglePauseMenu()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }
}
