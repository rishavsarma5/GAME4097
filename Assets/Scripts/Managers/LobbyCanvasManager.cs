using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject lobbyHomeCanvas;
    [SerializeField] private GameObject howToPlayCanvas;
    [SerializeField] private GameObject creditsCanvas;
    [SerializeField] private GameObject gameProgressCanvas;
    [SerializeField] private GameObject playGameCanvas;
    [SerializeField] private TextMeshProUGUI gameProgressText;

    // Start is called before the first frame update
    void Start()
    {
        howToPlayCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        gameProgressCanvas.SetActive(false);
        playGameCanvas.SetActive(false);
        lobbyHomeCanvas.SetActive(true);
    }

    public void OnHowToPlayButtonPressed()
    {
        lobbyHomeCanvas.SetActive(false);
        howToPlayCanvas.SetActive(true);
    }

    public void OnPlayGameButtonPressed()
    {
        lobbyHomeCanvas.SetActive(false);
        playGameCanvas.SetActive(true);
    }

    public void OnQuitButtonPressed()
    {
        if (GameProgressManager.Instance.gameProgress.gameStarted)
        {
            //GameProgressManager.Instance.gameProgress.SaveAllGameProgress();
        }
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void OnCreditsButtonPressed()
    {
        lobbyHomeCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }

    public void OnGameProgressButtonPressed()
    {
        lobbyHomeCanvas.SetActive(false);
        gameProgressCanvas.SetActive(true);
    }

    public void OnGoBackButtonPressed()
    {
        howToPlayCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        gameProgressCanvas.SetActive(false);
        playGameCanvas.SetActive(false);
        lobbyHomeCanvas.SetActive(true);
    }
}
