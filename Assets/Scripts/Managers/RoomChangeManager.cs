using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomChangeManager : MonoBehaviour
{
    public static RoomChangeManager Instance;

    [SerializeField] private Vector3 savedPlayerPosition;
    [SerializeField] private string lastSavedScene;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(this.gameObject);
        }
    }

    public void SavePlayerPosition(GameObject player)
    {
        savedPlayerPosition = player.transform.position;
        lastSavedScene = SceneManager.GetActiveScene().name;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == lastSavedScene)
        {
            // Find the player and set their position
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = savedPlayerPosition;
                player.transform.Rotate(0, 180f, 0);
            }
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
