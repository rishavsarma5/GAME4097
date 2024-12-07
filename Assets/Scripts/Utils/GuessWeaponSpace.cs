using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessWeaponSpace : MonoBehaviour
{
	private Image icon;
	private Image buttonImage;
	private Button button;
	private int index;

	private SuspectGuessUI manager;

	private void Awake()
	{
		manager = FindObjectOfType<SuspectGuessUI>();
		icon = GetComponentsInChildren<Image>()[1];
		buttonImage = GetComponent<Image>();
		button = GetComponent<Button>();

		button.interactable = false;
		buttonImage.color = Color.black;

		button.onClick.AddListener(OnClick);
	}

	public void Init(int i, Sprite img)
	{
		this.icon.sprite = img;
		this.index = i;
		button.interactable = true;
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
		manager.HandleGuess(index);
	}
}
