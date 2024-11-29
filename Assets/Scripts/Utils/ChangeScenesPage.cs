using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ChangeScenesPage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptHeader;
    [SerializeField] private TextMeshProUGUI promptFooter;
    [SerializeField] private GameObject player;
    [SerializeField] private string lobby = "Lobby";

    private void OnEnable()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        promptHeader.text = "Change Scenes";
        promptFooter.text = "Click one of the buttons above.";
    }

    public void OnReturnToLobbyPressed()
    {
        // update game progress
        GameStateManager.Instance.SaveNumTurnsPlayed();
        ClueGameManager.Instance.SaveCluesAndWeaponsFound();
        GameProgressManager.Instance.SavePlayerPosition(player.transform.position);

        SceneManager.LoadSceneAsync(lobby, LoadSceneMode.Single);
    }

    public void OnGoToSuspectSelectPressed()
    {
        // update game progress
        GameStateManager.Instance.SaveNumTurnsPlayed();
        ClueGameManager.Instance.SaveCluesAndWeaponsFound();
        GameProgressManager.Instance.SavePlayerPosition(player.transform.position);

        GameStateManager.Instance.SkipToSuspectSelect();
    }
}
