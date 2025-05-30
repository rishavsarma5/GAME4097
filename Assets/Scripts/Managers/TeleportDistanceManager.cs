using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TeleportDistanceManager : MonoBehaviour
{
    public static TeleportDistanceManager Instance;

    public bool diceMovementStageActive = false;

    [SerializeField] private int size;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private GameObject diceMovementBoxPrefab;
    [SerializeField] private List<GameObject> dice;
    [SerializeField] private float boxScale = 2f;
    private List<bool> allRollsCompleted;
    private bool allBoxesSpawned = false;
    private bool coroutineRunning = false;
    
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            throw new System.Exception("Can't be two Teleport Distance Managers!");
        }

        dice = new List<GameObject>();
        allRollsCompleted = new List<bool>();
    }

    private void Update()
    {
        if (diceMovementStageActive && !coroutineRunning)
        {
            coroutineRunning = true; 
            allRollsCompleted = new List<bool>(new bool[dice.Count]); // reset all rolls completed to false

            StartCoroutine(WaitUntilAllDiceValuesGenerated());
        }   
    }

    private IEnumerator WaitUntilAllDiceValuesGenerated()
    {
        yield return new WaitUntil(() => allRollsCompleted.All(completed => completed));

        Debug.Log("Finished dice rolls");
        allBoxesSpawned = true;
        coroutineRunning = false;
        Debug.Log($"All {dice.Count} Teleport Boxes Spawned!");
    }

    public void ResetTeleportDistanceManager()
    {
        allBoxesSpawned = false;
        diceMovementStageActive = false;
        allRollsCompleted.Clear();
        dice.Clear();
        coroutineRunning = false;
    }

    public void CreateTeleportDistanceBox(int dicePlayerIndex, int diceRollValue)
    {
        Debug.Log($"teleport box size: {diceRollValue}");
        size = diceRollValue;
        GameProgressManager.Instance.gameProgress.SaveLastDiceRoll(diceRollValue);
        GameObject distanceBox = Instantiate(diceMovementBoxPrefab, playerPosition.position, Quaternion.identity);
        distanceBox.transform.localScale *= size * boxScale;
        Destroy(distanceBox, 0.5f);

        // sets roll completed for current dice
        if (dicePlayerIndex >= 0 && dicePlayerIndex < allRollsCompleted.Count)
        {
            allRollsCompleted[dicePlayerIndex] = true;
        }
    }

    public void CreateTeleportDistanceBoxAfterReturnFromRoom(Vector3 oldPlayerPos, int lastDiceRoll)
    {
        Debug.Log("Called to create teleport distance box after reloading!");
        GameObject distanceBox = Instantiate(diceMovementBoxPrefab, oldPlayerPos, Quaternion.identity);
        Vector3 newPosition = new(distanceBox.transform.position.x, 0.5f, distanceBox.transform.position.z);
        distanceBox.transform.position = newPosition;
        distanceBox.transform.localScale *= lastDiceRoll * boxScale;
        Destroy(distanceBox, 0.5f);
    }

    public bool GetAllBoxesSpawned()
    {
        return allBoxesSpawned;
    }

    public void AddCreatedDice(GameObject spawnedDice)
    {
        dice.Add(spawnedDice);
        allRollsCompleted.Add(false);
        spawnedDice.GetComponentInChildren<DiceRolling>().OnDiceRollValue.AddListener(CreateTeleportDistanceBox);

        allBoxesSpawned = false;
    }
}
