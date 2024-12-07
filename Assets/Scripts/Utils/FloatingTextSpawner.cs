using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FloatingTextSpawner : MonoBehaviour
{
    public static FloatingTextSpawner Instance;

    public GameObject floatingTextPrefab;
    public Vector3 offset = new Vector3(0f, 0f, 0.3f);
    public Vector3 objectOffset = new Vector3(0f, 0f, 0.3f);

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float spawnDistance = 1.5f;
    [SerializeField] private float spawnObjectDistance = 1f;
    

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
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Called on scene loaded in floating text spawner");
        if (!cameraTransform) cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void OnEnable()
    {
        Application.quitting += OnApplicationQuit;
    }

    private void OnDisable()
    {
        Application.quitting -= OnApplicationQuit;
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Application is quitting, unsubscribing events.");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SpawnFloatingText(string text)
    {
        GameObject textObject = Instantiate(floatingTextPrefab);

        textObject.GetComponent<FloatingTextController>().textField.text = text;
        textObject.transform.position = cameraTransform.position + cameraTransform.forward * spawnDistance;
        Debug.Log($"floating text spawned at {textObject.transform.position}");
    }

    public GameObject SpawnFloatingTextAndReturnGameObject(string text)
    {
        GameObject textObject = Instantiate(floatingTextPrefab);

        textObject.GetComponent<FloatingTextController>().textField.text = text;

        textObject.transform.position = cameraTransform.position + cameraTransform.forward * spawnObjectDistance;
        Debug.Log($"floating text spawned at {textObject.transform.position}");

        return textObject;
    }

    public void SpawnFloatingTextWithTimedDestroy(string text, float destroyTimer)
    {
        GameObject textObject = Instantiate(floatingTextPrefab);

        textObject.GetComponent<FloatingTextController>().textField.text = text;

        textObject.transform.position = cameraTransform.position + cameraTransform.forward * spawnDistance;
        Debug.Log($"floating text spawned at {textObject.transform.position}");

        Destroy(textObject, destroyTimer);
    }
}
