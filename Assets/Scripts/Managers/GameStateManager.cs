using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameState currentState;

    [Header("Player Specific Values Needed")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerDice;
    [SerializeField] private EndTurnMenu endTurnMenu;
    [SerializeField] private TextMeshProUGUI numTurnsText;
    [SerializeField] private Transform player1SuspectSelectLocation;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private int numTurnsInGame = 5;

    [SerializeField] private List<DiceMovementTriggerHandler> teleportAnchors = new();

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


        var allTPs = GameObject.FindGameObjectsWithTag("TeleportAnchor");

        foreach (var tp in allTPs)
        {
            teleportAnchors.Add(tp.GetComponentInChildren<DiceMovementTriggerHandler>());
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
        Debug.Log("Entered Initialize Game State");
        ClueGameManager.Instance.InitializeAllFirstClues();
        ClueGameManager.Instance.InitializeStartingWeapons();

        foreach(var tp in teleportAnchors)
        {
            tp.ResetTeleportAnchor();
        }

        endTurnMenu.ResetEndTurn();
        numTurnsText.text = $"Num Turns: {numTurnsInGame}";

        UpdateGameState(GameState.MovementDiceRolling);
    }

    private void HandleDiceRolling()
    {
        Debug.Log("Entered Dice Rolling State");
        // spawn dices on player locations
        playerDice.SetActive(true);

        TeleportDistanceManager.Instance.diceMovementStageActive = true;
        StartCoroutine(WaitForTeleportDistanceBoxesToSpawn());
    }

    private IEnumerator WaitForTeleportDistanceBoxesToSpawn()
    {
        yield return new WaitUntil(() => TeleportDistanceManager.Instance.GetAllBoxesSpawned());
        TeleportDistanceManager.Instance.ResetTeleportDistanceManager();
        UpdateGameState(GameState.Exploration);
    }

    private void HandleExploration()
    {
        Debug.Log("Entered Exploration State");
        FloatingTextSpawner.Instance.SpawnFloatingText("Visit rooms to search for clues/weapons or talk to NPCs in range.");
        endTurnMenu.TurnOnCanvas();
        StartCoroutine(WaitForEndTurnPress());
    }

    private IEnumerator WaitForEndTurnPress()
    {
        yield return new WaitUntil(() => endTurnMenu.GetEndTurnPressed());
        UpdateGameState(GameState.EndRoundUpdates);
    }

    private void HandleEndOfRoundUpdates()
    {
        Debug.Log("Entered End of Round State");
        numTurnsInGame--;

        if (numTurnsInGame <= 0)
        {
            FloatingTextSpawner.Instance.SpawnFloatingText("Transitioning to suspect select stage...");
            StartCoroutine(PauseBeforeSuspectSelection());
        } else
        {
            // reset all teleport anchors to be deactive
            foreach (var tp in teleportAnchors)
            {
                tp.ResetTeleportAnchor();
            }

            // reset end of turn button
            endTurnMenu.ResetEndTurn();
            numTurnsText.text = $"Num Turns: {numTurnsInGame}";

            /*
            // move all npcs and reset interactions
            NPCManager.Instance.MoveNPCsToNewWaypoint();
            StartCoroutine(WaitForNPCFinishMoving());
            NPCManager.Instance.ResetAllNPCInteractionDistances();
            */

            UpdateGameState(GameState.MovementDiceRolling);
        }
    }

    private IEnumerator PauseBeforeSuspectSelection()
    {
        yield return new WaitForSeconds(2f);
        UpdateGameState(GameState.SuspectSelection);
    }

    private IEnumerator WaitForNPCFinishMoving()
    {
        yield return new WaitUntil(() => NPCManager.Instance.GetAllNPCSFinishedMoving());
    }

    private void HandleSuspectSelection()
    {
        Debug.Log("Entered Suspect Seleection State");
        List<Weapon> weaponsList = ClueGameManager.Instance.foundWeapons;
		FindObjectOfType<SuspectGuessUI>().setUp(weaponsList);

		player.transform.position = player1SuspectSelectLocation.position;
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
