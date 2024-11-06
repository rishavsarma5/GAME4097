using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Clue", menuName = "Clue")]
public class Clue : ScriptableObject
{
    public GameNPC relatedNPC;

    public bool isFound = false;

    [TextArea] public string description;
}
