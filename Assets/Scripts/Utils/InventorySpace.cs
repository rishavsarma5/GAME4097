using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class InventorySpace : MonoBehaviour
{
	public enum Type
	{
		Clue,
		Weapon
	}

	public Type type;

	private bool filled;

	public Image icon;

	private Clue clueInfo;
	private Weapon weaponInfo;

	private void Awake()
	{
		Debug.Log("Called on each inventory spaces");
		filled = false;
	}

	public bool fillWithClue(Clue clue)
	{
        if (!filled && type == Type.Clue) 
        {
            clueInfo = clue;
			filled = true;
			icon.sprite = clueInfo.icon;
			return true;
        }
		return false;
    }

	public bool fillWithWeapon(Weapon weapon)
	{
		if (!filled && type == Type.Weapon)
		{
			weaponInfo = weapon;
			filled = true;
			icon.sprite = weaponInfo.icon;
			return true;
		}
		return false;
	}

	public void FillWithClueFromList(List<Clue> clues, int index)
    {
		Debug.Log($"Filling with clue {clues[index]}");
		fillWithClue(clues[index]);
    }

	public void FillWithWeaponFromList(List<Weapon> weapons, int index)
	{
		Debug.Log($"Filling with weapon {weapons[index]}");
		fillWithWeapon(weapons[index]);
	}

	public void clicktoItemPage()
	{
		if (filled)
		{
			if (type == Type.Clue)
			{
				ClueGameManager.Instance.inventoryNotepad.goToItemPage(clueInfo);
			} else if (type == Type.Weapon) 
			{
				ClueGameManager.Instance.inventoryNotepad.goToItemPage(weaponInfo);
			}
		}
	}

}
