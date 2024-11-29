using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class DiceMovementTriggerHandler : MonoBehaviour
{
    [SerializeField] GameObject particleSystemParent;
    [SerializeField] TeleportationAnchor teleportationAnchor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovementDistanceBox"))
        {
            Debug.Log("trigger for teleport anchors hit!");
            TurnOnTeleportAnchor();
        }
    }

    public void ResetTeleportAnchor()
    {
        teleportationAnchor.enabled = false;
        particleSystemParent.SetActive(false);
    }

    public void TurnOnTeleportAnchor()
    {
        teleportationAnchor.enabled = true;
        particleSystemParent.SetActive(true);
    }
}
