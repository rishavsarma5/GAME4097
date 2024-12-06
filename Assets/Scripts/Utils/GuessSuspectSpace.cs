using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessSuspectSpace : MonoBehaviour
{
	public string subject;

	private Sprite icon;
	private Image buttonImage;
	private Button button;

	private SuspectGuessUI manager;

	private void Start()
	{
		manager = FindObjectOfType<SuspectGuessUI>();
		icon = GetComponentsInChildren<Image>()[1].sprite;
		buttonImage = GetComponent<Image>();
		button = GetComponent<Button>();

		button.interactable = true;
		buttonImage.color = Color.black;

		button.onClick.AddListener(OnClick);
	}

	public void SetSelected()
	{
		buttonImage.color = Color.red;
		button.interactable = false;
	}

	public void Deselect()
	{
		buttonImage.color = Color.black;
		button.interactable = true;
	}

	private void OnClick()
	{
		manager.HandleGuess(subject);
	}
}
