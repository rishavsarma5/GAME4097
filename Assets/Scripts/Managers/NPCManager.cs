using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance;

    [SerializeField] List<GameObject> npcs;
    private List<InteractionDistanceHandler> nPCInteractionDistances;
    private List<NPCController> nPCController;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            throw new System.Exception("Can't be two NPC Managers!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (npcs.Count == 0)
        {
            npcs = new List<GameObject>(GameObject.FindGameObjectsWithTag("NPCInteractable"));
        }
        
        foreach(GameObject npc in npcs)
        {
            nPCInteractionDistances.Add(npc.GetComponent<InteractionDistanceHandler>());
            nPCController.Add(npc.GetComponent<NPCController>());
        }
    }

    public void MoveNPCsToNewWaypoint()
    {
        foreach(var movement in nPCController)
        {
            movement.GoToNextWaypoint();
        }
    }

    public bool GetAllNPCSFinishedMoving()
    {
        foreach (var npcController in nPCController)
        {
            if (npcController.IsMoving())
            {
                return false;
            }
        }
        return true;
    }

    public void ResetAllNPCInteractionDistances()
    {
        foreach(var interact in nPCInteractionDistances)
        {
            interact.TurnOffCollider();
        }
    }
}
