using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnMenu : MonoBehaviour
{
    [SerializeField] private GameObject turnMenuCanvas;
    [SerializeField] private bool endTurnPressed = false;

    public void OnEndTurnPressed()
    {
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
        turnMenuCanvas.SetActive(true);
    }

}
