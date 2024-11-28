using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DiceSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject diceToSpawn;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float spawnDistance = 1.5f;

    public InputActionReference LC_TriggerRef;

    private bool diceSpawned = false;

    private void Awake()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        diceSpawned = false;
    }

    private void OnEnable()
    {
        LC_TriggerRef.action.started += LC_TriggerCustomAction;
        diceSpawned = false;
    }

    private void OnDisable()
    {
        LC_TriggerRef.action.started -= LC_TriggerCustomAction;
        diceSpawned = false;
    }

    public void LC_TriggerCustomAction(InputAction.CallbackContext context)
    {
        Debug.Log("Left Trigger pressed to spawn dice");

        // Calculate spawn position in front of the player's camera
        Vector3 spawnPosition = cameraTransform.position + cameraTransform.forward * spawnDistance;

        // Optional: Adjust the height of the spawn position if needed
        //spawnPosition.y = cameraTransform.position.y;

        // Instantiate the object at the calculated position, facing the player
        Quaternion spawnRotation = Quaternion.LookRotation(-cameraTransform.forward, Vector3.up);
        Instantiate(diceToSpawn, spawnPosition, spawnRotation);
        diceSpawned = true;
        Debug.Log("dice spawned set to true");
    }

    public bool DiceSpawned()
    {
        return diceSpawned;
    }

    public void ResetDiceSpawner()
    {
        diceSpawned = false;
        this.enabled = false;
    }

}
