using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject lobbyHomeCanvas;
    [SerializeField] private GameObject howToPlayCanvas;
    [SerializeField] private GameObject creditsCanvas;
    [SerializeField] private GameObject gameProgressCanvas;
    [SerializeField] private TextMeshProUGUI startGameText;
    [SerializeField] private TextMeshProUGUI gameProgressText;
    [SerializeField] private string mainMenu = "MainRoom";

    private bool gameStarted = false;
    private float gameTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        howToPlayCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        gameProgressCanvas.SetActive(false);
        lobbyHomeCanvas.SetActive(true);
    }

    public void OnHowToPlayButtonPressed()
    {
        lobbyHomeCanvas.SetActive(false);
        howToPlayCanvas.SetActive(true);
    }

    public void OnStartGameButtonPressed()
    {
        startGameText.text = "Continue Game";
        gameStarted = true;
        SceneManager.LoadSceneAsync(mainMenu, LoadSceneMode.Single);
    }

    public void OnQuitButtonPressed()
    {
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
        lobbyHomeCanvas.SetActive(true);
    }
}
