using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject lobbyHomeCanvas;
    [SerializeField] private GameObject howToPlayCanvas;
    [SerializeField] private TextMeshProUGUI startGameText;
    [SerializeField] private string mainMenu = "MainRoom";

    // Start is called before the first frame update
    void Start()
    {
        howToPlayCanvas.SetActive(false);
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
        SceneManager.LoadSceneAsync(mainMenu, LoadSceneMode.Single);
    }

    public void OnQuitButtonPressed()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void OnGoBackButtonPressed()
    {
        howToPlayCanvas.SetActive(false);
        lobbyHomeCanvas.SetActive(true);
    }
}
