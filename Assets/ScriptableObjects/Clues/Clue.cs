using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Clue", menuName = "Clue")]
public class Clue : ScriptableObject
{
    public GameNPC relatedNPC;
    public GameObject clueObject;

    public bool isFound = false;

    [TextArea] public string description;

    public void OnClueFound()
    {
        //relatedNPC
    }
}
