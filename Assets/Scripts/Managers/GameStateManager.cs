using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameState currentState;

    [Header("Player Specific Values Needed")]
    [SerializeField] private GameObject player;
    [SerializeField] private DiceSpawnManager diceSpawner;
    [SerializeField] private EndTurnMenu endTurnMenu;
    [SerializeField] private TextMeshProUGUI numTurnsText;
    [SerializeField] private bool playerInsideRoom = false;

    [SerializeField] private string mainRoom = "MainRoom";
    [SerializeField] private string suspectSelectRoom = "SuspectSelectRoom";

    [SerializeField] private Camera mainCamera;
    [SerializeField] private int numTurnsInGame = 5;

    [SerializeField] private List<DiceMovementTriggerHandler> teleportAnchors = new();

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(this.gameObject);
        }

        var allTPs = GameObject.FindGameObjectsWithTag("TeleportAnchor");
        diceSpawner = GetComponent<DiceSpawnManager>();

        foreach (var tp in allTPs)
        {
            teleportAnchors.Add(tp.GetComponentInChildren<DiceMovementTriggerHandler>());
        }
    }

    private void Start()
    {
        diceSpawner.enabled = false;
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
        ClueGameManager.Instance.InitializeAllClues();
        ClueGameManager.Instance.InitializeStartingWeapons();
        ClueGameManager.Instance.SaveInitialization();

        foreach(var tp in teleportAnchors)
        {
            tp.ResetTeleportAnchor();
        }

        endTurnMenu.ResetEndTurn();
        numTurnsText.text = $"Num Turns: {numTurnsInGame}";
        GameProgressManager.Instance.SaveTotalTurns(numTurnsInGame);

        UpdateGameState(GameState.MovementDiceRolling);
    }

    private void HandleDiceRolling()
    {
        Debug.Log("Entered Dice Rolling State");

        // enable dice spawn manager
        if (!diceSpawner.isActiveAndEnabled)
        {
            diceSpawner.enabled = true;
        }

        StartCoroutine(HandleDiceRollingSequence());
    }

    private IEnumerator HandleDiceRollingSequence()
    {
        //Wait for the player to spawn in
        yield return WaitForPlayerToSpawnIn();

        //Show message and wait for dice to be spawned
        FloatingTextSpawner.Instance.SpawnFloatingTextWithTimedDestroy("Press L Trigger To Spawn the Dice in Front of You!", 5f);
        yield return WaitForPlayerToSpawnDice();

        diceSpawner.ResetDiceSpawner();

        Debug.Log("Dice has been spawned.");

        // Wait for teleport distance boxes to spawn
        TeleportDistanceManager.Instance.diceMovementStageActive = true;
        yield return WaitForTeleportDistanceBoxesToSpawn();

        UpdateGameState(GameState.Exploration);
    }

    private IEnumerator WaitForPlayerToSpawnIn()
    {
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator WaitForPlayerToSpawnDice()
    {
        yield return new WaitUntil(() =>
        {
            return diceSpawner.DiceSpawned();
        });
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
        FloatingTextSpawner.Instance.SpawnFloatingTextWithTimedDestroy("Visit rooms to search for clues/weapons or talk to NPCs in range.", 5f);
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
            FloatingTextSpawner.Instance.SpawnFloatingTextWithTimedDestroy("Transitioning to suspect select stage...", 5f);
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

            // update game progress
            GameProgressManager.Instance.SaveTurnsPlayed(numTurnsInGame);
            ClueGameManager.Instance.SaveCluesAndWeaponsFound();
            GameProgressManager.Instance.SavePlayerPosition(player.transform.position);

            Debug.Log("Game Progress saved in end of round!");

            diceSpawner.enabled = false;

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
        Debug.Log("Entered Suspect Selection State");
        endTurnMenu.ResetEndTurn();
        List<Weapon> weaponsList = ClueGameManager.Instance.foundWeapons;
		FindObjectOfType<SuspectGuessUI>().setUp(weaponsList);

        SceneManager.LoadSceneAsync(suspectSelectRoom, LoadSceneMode.Single);
    }

    public void SetPlayerInsideRoom(bool inside)
    {
        playerInsideRoom = inside;
    }

    public bool IsPlayerInsideRoom()
    {
        return playerInsideRoom;
    }

    private void RestoreGameState()
    {
        var gameProgress = GameProgressManager.Instance.gameProgress;

        // Restore player's position
        player.transform.position = gameProgress.currentPlayerPosition;

        // Restore number of turns
        numTurnsInGame = gameProgress.numTurnsPlayed;

        // Update current state
        UpdateGameState(GameState.Exploration);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainRoom)
        {
            var gameProgress = GameProgressManager.Instance.gameProgress;
            if (gameProgress.continueGame)
            {
                gameProgress.continueGame = false;

                GameProgressManager.Instance.gameProgress.LoadGameProgress();
                RestoreGameState();
            }
        }
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
