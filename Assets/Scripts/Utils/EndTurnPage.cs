using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndTurnPage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptHeader;
    [SerializeField] private TextMeshProUGUI promptFooter;
    [SerializeField] private TextMeshProUGUI numTurnsText;
    [SerializeField] private GameObject turnLimitCheckmark;
    [SerializeField] private GameObject player;
    [SerializeField] private string lobby = "Lobby";
    [SerializeField] private NotepadUI notepadUI;

    private void OnEnable()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        if (!notepadUI) notepadUI = GameObject.FindGameObjectWithTag("NotepadUI").GetComponent<NotepadUI>();
        promptHeader.text = "End Turn";
        promptFooter.text = "Click button to end turn.";
        DisplayNumTurnsText();
    }

    public void OnToggleTurnLimit()
    {
        notepadUI.areTurnsLimited = !notepadUI.areTurnsLimited;
        DisplayNumTurnsText();
    }

    private void DisplayNumTurnsText()
    {
        if (notepadUI.areTurnsLimited)
        {
            turnLimitCheckmark.SetActive(true);
            numTurnsText.text = $"Turns Left: {GameStateManager.Instance.GetNumTurnsLeft()}";
        }
        else
        {
            turnLimitCheckmark.SetActive(false);
            numTurnsText.text = "Turns Left: ∞";
        }
    }

    public void OnEndTurnButtonPressed()
    {
        notepadUI.endTurnPressed = true;
    }
}
