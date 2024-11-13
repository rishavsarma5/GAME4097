using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game NPC", menuName = "Game NPC")]
public class GameNPC : ScriptableObject
{
    public string npcName;
    public Weapon weapon;
    public List<Clue> clues;
    public NPCInteractable npcInteractInfo;
}
