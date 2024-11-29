using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    [SerializeField] private string lobby = "Lobby";
    private void OnEnable()
    {
        SceneManager.LoadSceneAsync(lobby, LoadSceneMode.Single);
    }
}
