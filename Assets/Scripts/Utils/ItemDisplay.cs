using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
	public TMP_Text description;
	public UnityEngine.UI.Image icon;

	public void Display(string text, Sprite label)
	{
		icon.sprite = label;
		description.text = text;
	}

}
