using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TeleportDistanceManager : MonoBehaviour
{
    public static TeleportDistanceManager Instance;

    [SerializeField] private int size;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private GameObject diceMovementBoxPrefab;
    [SerializeField] private List<GameObject> dice;

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
    }

    private void Start()
    {
        dice = GameObject.FindGameObjectsWithTag("Dice").ToList();

        foreach (GameObject die in dice)
        {
            die.GetComponentInChildren<DiceRolling>().OnDiceRollValue.AddListener(CreateTeleportDistanceBox);
        }
        
    }

    private void OnDestroy()
    {
        foreach (GameObject die in dice)
        {
            die.GetComponentInChildren<DiceRolling>().OnDiceRollValue.RemoveListener(CreateTeleportDistanceBox);
        }
    }

    public void CreateTeleportDistanceBox(int diceRollValue)
    {
        Debug.Log($"teleport box size: {diceRollValue}");
        size = diceRollValue;
        GameObject distanceBox = Instantiate(diceMovementBoxPrefab, playerPosition.position, Quaternion.identity);
        distanceBox.transform.localScale *= size;
        Destroy(distanceBox, 0.5f);
    }
}
