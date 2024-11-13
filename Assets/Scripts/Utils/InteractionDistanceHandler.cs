using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class InteractionDistanceHandler : MonoBehaviour
{
    [SerializeField] XRGrabInteractable grabInteractable;
    [SerializeField] XRSimpleInteractable simpleInteractable;

    [SerializeField] private bool useSimpleInteractable = true;

    private void Start()
    {
        if (simpleInteractable == null && useSimpleInteractable)
        {
            Debug.LogError("Simple Interactable needs to be specified if using it");
        } else if (grabInteractable == null && !useSimpleInteractable)
        {
            Debug.LogError("Grab Interactable needs to be specified if using it");
        }

        TurnOffInteractable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovementDistanceBox"))
        {
            Debug.Log("trigger for npc hit!");
            if (useSimpleInteractable)
            {
                simpleInteractable.enabled = true;
            } else
            {
                grabInteractable.enabled = true;
            }
            
        }
    }

    public void TurnOffInteractable()
    {
        if (useSimpleInteractable)
        {
            simpleInteractable.enabled = false;
        }
        else
        {
            grabInteractable.enabled = false;
        }
    }
}
