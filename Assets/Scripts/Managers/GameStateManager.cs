using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameState currentState;
    [SerializeField] private GameObject player;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private Transform player1SuspectSelectLocation;

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
		//player = GameObject.FindGameObjectWithTag("Player") as GameObject;
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
                HandleDiceRolling();
                break;
            case GameState.Exploration:
                HandleExploration();
                break;
            case GameState.EndRoundUpdates:
                HandleEndOfRoundUpdates();
                break;
            case GameState.SuspectSelection:
                HandleSuspectSelection();
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
        ClueGameManager.Instance.InitializeAllFirstClues();
        ClueGameManager.Instance.InitializeStartingWeapons();

        UpdateGameState(GameState.MovementDiceRolling);
    }

    private void HandleDiceRolling()
    {
        

        UpdateGameState(GameState.Exploration);
    }

    private void HandleExploration()
    {
        //FloatingTextSpawner.Instance.SpawnFloatingText("debug text");
        StartCoroutine(WaitForMeaningfulAction());
    }

    private IEnumerator WaitForMeaningfulAction()
    {
        yield return new WaitUntil(() => ClueGameManager.Instance.GetActionCompleted());
        ClueGameManager.Instance.ResetActionCompleted();
        UpdateGameState(GameState.EndRoundUpdates);
    }

    private void HandleEndOfRoundUpdates()
    {
        UpdateGameState(GameState.SuspectSelection);
    }

    private void HandleSuspectSelection()
    {
		List<Weapon> weaponsList = ClueGameManager.Instance.foundWeapons;
		FindObjectOfType<SuspectGuessUI>().setUp(weaponsList);

		player.transform.position = player1SuspectSelectLocation.position;

        // create selection ui popup
        Debug.Log("got to suspect selection");
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
