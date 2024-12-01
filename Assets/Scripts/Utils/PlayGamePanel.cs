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
    [SerializeField] private LobbyCanvasManager lobbyCanvasManager;

    private void OnEnable()
    {
        var gameProgress = GameProgressManager.Instance.gameProgress;
        continueButton.SetActive(false);
        startButton.SetActive(false);
        restartButton.SetActive(false);

        if (gameProgress.GetGameStarted())
        {
            continueButton.SetActive(true);
            restartButton.SetActive(true);
        } else
        {
            startButton.SetActive(true);
        }

        if (lobbyCanvasManager == null)
        {
            Debug.LogError("Did not set lobby canvas manager");
        }
    }

    public void OnStartGameButtonPressed()
    {
        GameProgressManager.Instance.gameProgress.gameStarted = true;
        GameProgressManager.Instance.gameProgress.SaveGameStarted();
        SceneManager.LoadSceneAsync(mainMenu, LoadSceneMode.Single);
    }

    public void OnContinueGameButtonPressed()
    {
        GameProgressManager.Instance.gameProgress.continueGame = true;
        GameProgressManager.Instance.gameProgress.SaveGameContinued();
        SceneManager.LoadSceneAsync(mainMenu, LoadSceneMode.Single);
    }

    public void OnRestartGameButtonPressed()
    {
        GameProgressManager.Instance.gameProgress.ResetGame();
        DestroyDontDestroyOnLoadObjects();
        //SceneManager.LoadSceneAsync(introCutscene, LoadSceneMode.Single);
        lobbyCanvasManager.OnGoBackButtonPressed();
    }

    private void DestroyDontDestroyOnLoadObjects()
    {
        // Get the DontDestroyOnLoad scene
        Scene dontDestroyOnLoadScene = SceneManager.GetSceneByName("DontDestroyOnLoad");
        if (!dontDestroyOnLoadScene.IsValid()) return;

        // Find all root game objects in the scene
        GameObject[] rootObjects = dontDestroyOnLoadScene.GetRootGameObjects();

        // Destroy each object
        foreach (GameObject obj in rootObjects)
        {
            Destroy(obj);
        }
    }
}
