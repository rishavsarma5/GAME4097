using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueController : MonoBehaviour
{
    public Clue clue;
    private GameObject descriptionTextObject;

    public void OnClue1Grabbed()
    {
        ClueGameManager.Instance.OnClue1Found(clue);
    }

    public void OnClue2Grabbed()
    {
        ClueGameManager.Instance.OnClue2Found(clue);
    }

    public void SpawnClueDescriptionText()
    {
        descriptionTextObject = FloatingTextSpawner.Instance.SpawnFloatingTextAndReturnGameObject(clue.description);
    }

    public void DestroyClueDescriptionText()
    {
        Destroy(descriptionTextObject);
    }
}
