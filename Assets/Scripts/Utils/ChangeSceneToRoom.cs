using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneToRoom : MonoBehaviour
{
    [SerializeField] private string targetSceneName;
    [SerializeField] private string mainRoom = "MainRoom";
    [SerializeField] private GameObject player;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip doorClose;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
    }

    public void SwitchRoom()
    {
        RoomChangeManager.Instance.SavePlayerPosition(player);
        RoomChangeManager.Instance.LoadScene(targetSceneName);
    }

    public void SwitchToMainRoom()
    {
        RoomChangeManager.Instance.LoadScene(mainRoom);
        audioSource.PlayOneShot(doorClose);
    }
}
