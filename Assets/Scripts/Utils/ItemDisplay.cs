using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
	public TMP_Text description;
	public TMP_Text nameHeader;
	public UnityEngine.UI.Image icon;

	public void Display(string desc, Sprite label, string name)
	{
		icon.sprite = label;
		description.text = desc;
		nameHeader.text = name;
	}

}
