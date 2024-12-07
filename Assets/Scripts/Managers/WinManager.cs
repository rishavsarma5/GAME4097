using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinManager : MonoBehaviour
{
	public GameObject suspectUI;
	public Light roofLight;

	public AudioClip winSound;
	public AudioClip loseSound;

	public GameObject winCanvas;
	public TMP_Text winText;

	public AudioSource mainSong;

	public void WinGame()
	{
		suspectUI.SetActive(false);
		roofLight.color = Color.green;
		mainSong.Stop();
		AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position);
		winCanvas.SetActive(true);
		winText.text = "CORRECT";
	}

	public void LoseGame()
	{
		suspectUI.SetActive(false);
		roofLight.color = Color.red;
		mainSong.Stop();
		AudioSource.PlayClipAtPoint(loseSound, Camera.main.transform.position);
		winCanvas.SetActive(true);
		winText.text = "INCORRECT";
	}
}
