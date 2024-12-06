using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class InventorySpace : MonoBehaviour
{
	public enum Type
	{
		Clue,
		Weapon
	}

	public Type type;

	private bool filled;

	private Clue clueInfo;
	private Weapon weaponInfo;

	private void Awake()
	{
		filled = false;
	}

	public bool fillWithClue(Clue clue)
	{
        if (!filled && type == Type.Clue) 
        {
            clueInfo = clue;
			//item = itemObject;
			filled = true;
			DisplayIcon();
			return true;
        }
		return false;
    }

	public bool fillWithWeapon(Weapon weapon)
	{
		if (!filled && type == Type.Weapon)
		{
			weaponInfo = weapon;
			//item = itemObject;
			filled = true;
			DisplayIcon();
			return true;
		}
		return false;
	}

	public InventorySpace FillWithClueFromList(List<Clue> clues, int index)
    {
		InventorySpace newClue = new();
		newClue.fillWithClue(clues[index]);

		return newClue;
    }

	public InventorySpace FillWithWeaponFromList(List<Weapon> weapons, int index)
	{
		InventorySpace newWeapon = new();
		newWeapon.fillWithWeapon(weapons[index]);

		return newWeapon;
	}

	private void DisplayIcon()
	{
		if (type == Type.Clue)
		{
			GetComponentsInChildren<UnityEngine.UI.Image>()[1].sprite = clueInfo.icon;
		}
		else if (type == Type.Weapon)
		{
			GetComponentsInChildren<UnityEngine.UI.Image>()[1].sprite = weaponInfo.icon;
		}
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
