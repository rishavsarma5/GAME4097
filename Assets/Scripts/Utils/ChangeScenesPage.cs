using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ChangeScenesPage : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private string lobby = "Lobby";

    private void OnEnable()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
    }

    public void OnReturnToLobbyPressed()
    {
        // update game progress
        GameStateManager.Instance.SaveNumTurnsPlayed();
        GameStateManager.Instance.SaveTurnsLeft();
        ClueGameManager.Instance.SaveCluesAndWeaponsFound();
        GameProgressManager.Instance.SavePlayerPosition(player.transform.position);

        SceneManager.LoadSceneAsync(lobby, LoadSceneMode.Single);
    }

    public void OnGoToSuspectSelectPressed()
    {
        // update game progress
        GameStateManager.Instance.SaveNumTurnsPlayed();
        GameStateManager.Instance.SaveTurnsLeft();
        ClueGameManager.Instance.SaveCluesAndWeaponsFound();
        GameProgressManager.Instance.SavePlayerPosition(player.transform.position);

        GameStateManager.Instance.SkipToSuspectSelect();
    }
}
