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
    [SerializeField] private NotepadUI notepadUI;

    [SerializeField] private string mainRoom = "MainRoom";
    [SerializeField] private string suspectSelectRoom = "SuspectSelect";

    [SerializeField] private int totalTurnsInGame = 10;
    [SerializeField] private int turnsLeft = 0;
    [SerializeField] private int numTurnsPlayed = 0;
    [SerializeField] private Vector3 playerStartExplorationPosition;
    [SerializeField] private int savedDiceRoll;
    

    [SerializeField] private List<DiceMovementTriggerHandler> teleportAnchors = new();

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        } else
        {
            Destroy(this.gameObject);
        }

        var allTPs = GameObject.FindGameObjectsWithTag("TeleportAnchor");
        notepadUI = GameObject.FindGameObjectWithTag("NotepadUI").GetComponent<NotepadUI>();
        diceSpawner = GetComponent<DiceSpawnManager>();

        foreach (var tp in allTPs)
        {
            teleportAnchors.Add(tp.GetComponentInChildren<DiceMovementTriggerHandler>());
        }

        turnsLeft = totalTurnsInGame;
    }

    private void Start()
    {
        diceSpawner.enabled = false;
        UpdateGameState(GameState.InitializeGame);
    }

    private void OnEnable()
    {
        Application.quitting += OnApplicationQuit;
    }

    private void OnDisable()
    {
        Application.quitting -= OnApplicationQuit;
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Application is quitting, unsubscribing events.");
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // update game progress
        if (turnsLeft == totalTurnsInGame)
        {
            GameProgressManager.Instance.gameProgress.ResetGame();
        } else if (player != null)
        {
            SaveNumTurnsPlayed();
            ClueGameManager.Instance.SaveCluesAndWeaponsFound();
            GameProgressManager.Instance.SavePlayerPosition(player.transform.position);
            SaveTurnsLeft();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Game state manager on scene loaded called!");
        // Repopulate teleport anchors if back in the relevant scene
        if (scene.name == mainRoom)
        {
            var gameProgress = GameProgressManager.Instance.gameProgress;
            if (gameProgress.continueGame)
            {
                Debug.Log("Game state manager met in continue");
                gameProgress.continueGame = false;
                GameProgressManager.Instance.gameProgress.SaveGameContinued();
                GameProgressManager.Instance.gameProgress.LoadGameProgress();
                ReInitializeMainRoom();
                RestoreGameState();
                return;
            }

            
            Debug.Log("loaded game progress");
            
            ReInitializeMainRoom();
        }
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

        notepadUI.TurnOffOtherTabs();

        GameProgressManager.Instance.SaveTotalTurns(totalTurnsInGame);

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
        GameProgressManager.Instance.gameProgress.SaveDiceRollPosition(player.transform.position);
        UpdateGameState(GameState.Exploration);
    }

    private void HandleExploration()
    {
        Debug.Log("Entered Exploration State");
        Debug.Log($"Exploration Position: {player.transform.position}");
        playerStartExplorationPosition = player.transform.position;
        notepadUI.TurnOnOtherTabs();
        FloatingTextSpawner.Instance.SpawnFloatingTextWithTimedDestroy("Visit rooms to search for clues/weapons or talk to NPCs in range.", 5f);
        StartCoroutine(WaitForEndTurnPress());
    }

    private IEnumerator WaitForEndTurnPress()
    {
        yield return new WaitUntil(() => notepadUI.GetEndTurnPressed());
        UpdateGameState(GameState.EndRoundUpdates);
    }

    private void HandleEndOfRoundUpdates()
    {
        Debug.Log("Entered End of Round State");

        if (notepadUI.areTurnsLimited)
        {
            turnsLeft--;
            GameProgressManager.Instance.SaveTurnsLeft(turnsLeft);
            numTurnsPlayed++;
        }

        if (turnsLeft <= 0)
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
            notepadUI.ResetToBase();
            notepadUI.TurnOffOtherTabs();

            // update game progress
            SaveNumTurnsPlayed();
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
    
    /*
    private IEnumerator WaitForNPCFinishMoving()
    {
        yield return new WaitUntil(() => NPCManager.Instance.GetAllNPCSFinishedMoving());
    }
    */

    private void HandleSuspectSelection()
    {
        Debug.Log("Entered Suspect Selection State");
        notepadUI.ResetToBase();

        SceneManager.LoadSceneAsync("SuspectSelect", LoadSceneMode.Single);
    }

    private void RestoreGameState()
    {
        Debug.Log("restoring game state to explore");
        var gameProgress = GameProgressManager.Instance.gameProgress;

        // Restore player's position
        player.transform.position = gameProgress.currentPlayerPosition;

        // Restore number of turns
        totalTurnsInGame = gameProgress.totalTurns;
        numTurnsPlayed = gameProgress.numTurnsPlayed;
        turnsLeft = gameProgress.turnsLeft;

        // Restore clues/weapons found
        ClueGameManager.Instance.AddAllFoundCluesAndWeaponsToList();

        // Update current state
        UpdateGameState(GameState.Exploration);
    }

    public void SkipToSuspectSelect()
    {
        turnsLeft = 0;
        SaveTurnsLeft();
        UpdateGameState(GameState.EndRoundUpdates);
    }

    public void SaveNumTurnsPlayed()
    {
        GameProgressManager.Instance.SaveTurnsPlayed(numTurnsPlayed);
    }

    public void SaveTurnsLeft()
    {
        GameProgressManager.Instance.SaveTurnsLeft(turnsLeft);
    }

    public int GetNumTurnsLeft()
    {
        return turnsLeft;
    }

    private void ReInitializeMainRoom()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        diceSpawner = GetComponent<DiceSpawnManager>();
        notepadUI = GameObject.FindGameObjectWithTag("NotepadUI").GetComponent<NotepadUI>();
        notepadUI.TurnOnOtherTabs();

        playerStartExplorationPosition = GameProgressManager.Instance.gameProgress.diceRollPlayerPosition;
        savedDiceRoll = GameProgressManager.Instance.gameProgress.lastDiceRoll;

        teleportAnchors.Clear();
        var allTPs = GameObject.FindGameObjectsWithTag("TeleportAnchor");
        foreach (var tp in allTPs)
        {
            var handler = tp.GetComponentInChildren<DiceMovementTriggerHandler>();
            if (handler)
                teleportAnchors.Add(handler);
        }

        // Restore active anchors
        Debug.Log("got to restoring active anchors");
        Debug.Log($"player start pos for tele distance box after return: {playerStartExplorationPosition}");
        Debug.Log($"last dice roll: {savedDiceRoll}");
        TeleportDistanceManager.Instance.CreateTeleportDistanceBoxAfterReturnFromRoom(playerStartExplorationPosition, savedDiceRoll);
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
