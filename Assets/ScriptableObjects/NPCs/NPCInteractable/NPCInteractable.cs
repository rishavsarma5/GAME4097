using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Interactable", menuName = "NPC Interactable")]
public class NPCInteractable : ScriptableObject
{
    public string promptCanvasHeaderText;
    
    [Header("Dialogue Prompts")]
    public List<NPCDialogue> dialoguePrompts;
    public bool hasPlayed = false;
    public void SetHasBeenPlayed()
    {
        hasPlayed = true;
    }
}
