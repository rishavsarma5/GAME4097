using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class RoomEntranceTrigger : MonoBehaviour
{
    [SerializeField] private Transform tpHallwaySpot;
    [SerializeField] private Transform tpRoomSpot;

    [SerializeField] private bool outsideRoom = false;

    [SerializeField] private GameObject player;

    [SerializeField] GameObject leftController;
    ControllerInputActionManager inputActionManager;
    [SerializeField] GameObject playerLeftControllerLocoTurn;
    [SerializeField] GameObject playerLeftControllerLocoMove;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player") as GameObject;
        if (player == null)
        {
            throw new System.Exception("No Player with tag 'Player' in Scene");
        } else if (!leftController || !playerLeftControllerLocoMove || !playerLeftControllerLocoTurn)
        {
            throw new System.Exception("Left controller info not set");
        }
        inputActionManager = leftController.GetComponent<ControllerInputActionManager>();
    }

    public void TransitionBetweenRooms()
    {
        if (outsideRoom)
        {
            player.transform.position = tpRoomSpot.position;
            playerLeftControllerLocoTurn.gameObject.SetActive(true);
            playerLeftControllerLocoMove.gameObject.SetActive(true);
            inputActionManager.smoothMotionEnabled = true;
            outsideRoom = false;
            GameStateManager.Instance.SetPlayerInsideRoom(true);
        } else
        {
            player.transform.position = tpHallwaySpot.position;
            playerLeftControllerLocoTurn.gameObject.SetActive(false);
            playerLeftControllerLocoMove.gameObject.SetActive(false);
            inputActionManager.smoothMotionEnabled = false;
            outsideRoom = true;
            GameStateManager.Instance.SetPlayerInsideRoom(false);
        }
    }
}
