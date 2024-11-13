using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueController : MonoBehaviour
{
    public Clue clue;
	//public GameObject simplifiedGameObject;

    public void OnClue1Grabbed()
    {
        ClueGameManager.Instance.OnClue1Found(clue);
    }

    public void OnClue2Grabbed()
    {
        ClueGameManager.Instance.OnClue2Found(clue);
    }
}
