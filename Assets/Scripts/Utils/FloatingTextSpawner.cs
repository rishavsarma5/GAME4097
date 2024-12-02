using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FloatingTextSpawner : MonoBehaviour
{
    public static FloatingTextSpawner Instance;

    public GameObject floatingTextPrefab;
    public Vector3 offset = new Vector3(0f, -0.05f, 0.3f);

    public Transform playerTransform;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SpawnFloatingText(string text)
    {
        GameObject textObject = Instantiate(floatingTextPrefab);

        textObject.GetComponent<FloatingTextController>().textField.text = text;

        textObject.transform.position = playerTransform.position + playerTransform.TransformDirection(offset);
        Debug.Log($"floating text spawned at {textObject.transform.position}");
    }

    public GameObject SpawnFloatingTextAndReturnGameObject(string text)
    {
        GameObject textObject = Instantiate(floatingTextPrefab);

        textObject.GetComponent<FloatingTextController>().textField.text = text;

        textObject.transform.position = playerTransform.position + playerTransform.TransformDirection(offset);
        Debug.Log($"floating text spawned at {textObject.transform.position}");

        return textObject;
    }

    public void SpawnFloatingTextWithTimedDestroy(string text, float destroyTimer)
    {
        GameObject textObject = Instantiate(floatingTextPrefab);

        textObject.GetComponent<FloatingTextController>().textField.text = text;

        textObject.transform.position = playerTransform.position + playerTransform.TransformDirection(offset);
        Debug.Log($"floating text spawned at {textObject.transform.position}");

        Destroy(textObject, destroyTimer);
    }
}
