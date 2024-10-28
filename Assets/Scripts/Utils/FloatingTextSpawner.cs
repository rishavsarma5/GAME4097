using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextSpawner : MonoBehaviour
{
    public static FloatingTextSpawner Instance;

    public GameObject floatingTextPrefab;

    public Transform playerTransform;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            throw new System.Exception("Can't be two FloatingTextSpawners!");
        }
    }

    public void SpawnFloatingText(string text)
    {
        GameObject textObject = Instantiate(floatingTextPrefab);

        textObject.GetComponent<FloatingTextController>().textField.text = text;

        textObject.transform.position = playerTransform.position + playerTransform.TransformDirection(new Vector3(0, 0.5f, 0.2f));
        textObject.transform.LookAt(playerTransform);
        Debug.Log($"floating text spawned at {textObject.transform}");
    }
}
