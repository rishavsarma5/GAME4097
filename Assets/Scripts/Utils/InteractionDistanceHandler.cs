using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDistanceHandler : MonoBehaviour
{
    [SerializeField] BoxCollider _interactionCollider;

    private void Start()
    {
        BoxCollider _interactionCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovementDistanceBox"))
        {
            Debug.Log("trigger for npc hit!");
            _interactionCollider.gameObject.SetActive(true);
        }
    }

    public void TurnOffCollider()
    {
        _interactionCollider.gameObject.SetActive(false);
    }
}
