using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDistanceManager : MonoBehaviour
{
    public static TeleportDistanceManager Instance;

    [SerializeField] private int size;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private GameObject diceMovementBoxPrefab;

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

    public void CreateTeleportDistanceBox(int diceRollValue)
    {
        size = diceRollValue;
        GameObject distanceBox = Instantiate(diceMovementBoxPrefab, playerPosition.position, Quaternion.identity);
        distanceBox.transform.localScale *= size;
        Destroy(distanceBox, 0.5f);
    }
}
