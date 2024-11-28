using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGamePanel : MonoBehaviour
{
    [SerializeField] private string mainMenu = "MainRoom";
    [SerializeField] private string introCutscene = "IntroScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject restartButton;

    private void OnEnable()
    {
        var gameProgress = GameProgressManager.Instance.gameProgress;
        continueButton.SetActive(false);
        startButton.SetActive(false);
        restartButton.SetActive(false);

        if (gameProgress.gameStarted)
        {
            continueButton.SetActive(true);
            restartButton.SetActive(true);
        } else
        {
            startButton.SetActive(true);
        }
    }

    public void OnStartGameButtonPressed()
    {
        GameProgressManager.Instance.gameProgress.gameStarted = true;
        SceneManager.LoadSceneAsync(mainMenu, LoadSceneMode.Single);
    }

    public void OnContinueGameButtonPressed()
    {
        SceneManager.LoadSceneAsync(mainMenu, LoadSceneMode.Single);
    }

    public void OnRestartGameButtonPressed()
    {
        GameProgressManager.Instance.gameProgress.ResetGame();
        SceneManager.LoadSceneAsync(introCutscene, LoadSceneMode.Single);
    }
}
