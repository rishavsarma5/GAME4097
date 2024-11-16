using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NotepadUI : MonoBehaviour
{
	public GameObject[] RowCollection;

	private InventorySpace[][] InventorySpaces;

	public GameObject inventoryPage;
	public GameObject itemPage;

	public AudioClip newItemSound;
	public AudioClip clickSound;

	private List<string> clueAndWeaponList;

	void Awake()
	{
		inventoryPage.SetActive(true);
		itemPage.SetActive(false);
		InventorySpaces = new InventorySpace[RowCollection.Length][];
		InventorySpace[] itemsList;
		for (int row = 0; row < RowCollection.Length; row++)
		{
			InventorySpaces[row] = new InventorySpace[3];
			itemsList = RowCollection[row].GetComponentsInChildren<InventorySpace>();
			for (int item = 0; item < 3; item++)
			{
				InventorySpaces[row][item] = itemsList[item];
			}
		}
	}

	public void AddClue(Clue clue)
	{
		if (!clueAndWeaponList.Contains(clue.name))
		{
			for (int row = 0; row < RowCollection.Length; row++)
			{
				for (int space = 0; space < 2; space++)
				{
					if (InventorySpaces[row][space].fillWithClue(clue))
					{
						AudioSource.PlayClipAtPoint(newItemSound, Camera.main.transform.position);
						clueAndWeaponList.Add(clue.name);
						return;
					}
				}
			}
		}
	}

	public void AddWeapon(Weapon weapon)
	{
		if (!clueAndWeaponList.Contains(weapon.name))
		{
			for (int row = 0; row < RowCollection.Length; row++)
			{
				if (InventorySpaces[row][2].fillWithWeapon(weapon))
				{
					AudioSource.PlayClipAtPoint(newItemSound, Camera.main.transform.position);
					clueAndWeaponList.Add(weapon.name);
					return;
				}
			}
		}
	}

	public void goToItemPage(Clue clue)
	{
		AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
		inventoryPage.SetActive(false);
		itemPage.SetActive(true);
	}

	public void goToItemPage(Weapon weapon)
	{
		AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
		inventoryPage.SetActive(false);
		itemPage.SetActive(true);
	}

	public void goToInvPage()
	{
		AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
		itemPage.SetActive(false);
		inventoryPage.SetActive(true);
	}
}
