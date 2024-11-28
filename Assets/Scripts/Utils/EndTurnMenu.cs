using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnMenu : MonoBehaviour
{
    [SerializeField] private GameObject turnMenuCanvas;
    [SerializeField] private bool endTurnPressed = false;

    public void OnEndTurnPressed()
    {
        if (GameStateManager.Instance.IsPlayerInsideRoom())
        {
            FloatingTextSpawner.Instance.SpawnFloatingTextWithTimedDestroy("Have to be outside a room to end turn", 3f);
            return;
        }
        endTurnPressed = true;
    }

    public bool GetEndTurnPressed()
    {
        return endTurnPressed;
    }

    public void ResetEndTurn()
    {
        endTurnPressed = false;
        turnMenuCanvas.SetActive(false);
    }

    public void TurnOnCanvas()
    {
        Debug.Log("End Turn Menu TurnedOn");
        turnMenuCanvas.SetActive(true);
    }

}
