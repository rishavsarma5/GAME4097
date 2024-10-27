using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameState currentState;

    //public static event Action<GameState> onGameStateChanged;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        } else
        {
            throw new System.Exception("Can't be two Game State Managers!");
        }
    }

    private void Start()
    {
        UpdateGameState(GameState.InitializeGame);
    }

    public void UpdateGameState(GameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GameState.InitializeGame:
                HandleInitializeGame();
                break;
            case GameState.MovementDiceRolling:
                break;
            case GameState.Exploration:
                break;
            case GameState.EndRoundUpdates:
                break;
            case GameState.SuspectSelection:
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        //onGameStateChanged?.Invoke(newState);
    }

    private void HandleInitializeGame()
    {
        //ClueGameManager.Instance.InitializeAllFirstClues();
        ClueGameManager.Instance.InitializeStartingWeapons();

        //UpdateGameState(GameState.MovementDiceRolling);
    }
}

public enum GameState
{
    InitializeGame,
    MovementDiceRolling,
    Exploration,
    EndRoundUpdates,
    SuspectSelection,
    Victory,
    Lose
}
