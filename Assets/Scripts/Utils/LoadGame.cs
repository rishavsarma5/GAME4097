using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    [SerializeField] private string mainRoom = "_Prototype_StateScene";
    private void OnEnable()
    {
        SceneManager.LoadSceneAsync(mainRoom, LoadSceneMode.Single);
    }
}
