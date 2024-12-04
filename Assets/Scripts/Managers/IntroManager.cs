using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
	public GameObject roof;
	public AudioClip lightClick;

	public GameObject menu;
	public AudioClip menuSound;

    void Start()
    {
        roof.SetActive(false);
		menu.SetActive(false);
		Invoke("TurnOnLights", 3f);
    }

	public void ActivateMenu()
	{
		menu.SetActive(true);
		AudioSource.PlayClipAtPoint(menuSound, Camera.main.transform.position);
	}

	public void TurnOnLights()
	{
		roof.SetActive(true);
		AudioSource.PlayClipAtPoint(lightClick, Camera.main.transform.position);
	}
}
