using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NotepadUI : MonoBehaviour
{
	public GameObject[] RowCollection;

	private InventorySpace[][] InventorySpaces;

	public GameObject inventoryPage;
	public GameObject itemPage;

	private List<Clue> ClueList;
	private List<Weapon> WeaponList;

	public InputActionReference menuButton;


	void Awake()
	{
		ClueList = new List<Clue>();
		WeaponList = new List<Weapon>();
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

	private void Start()
	{
		menuButton += ToggleMenu;
	}

	public void AddClue(Clue clue)
	{
		if (!ClueList.Contains(clue))
		{
			for (int row = 0; row < RowCollection.Length; row++)
			{
				for (int space = 0; space < 2; space++)
				{
					if (InventorySpaces[row][space].fillWithClue(clue))
					{
						ClueList.Add(clue);
						return;
					}
				}
			}
		}
	}

	public void AddWeapon(Weapon weapon)
	{
		if (!WeaponList.Contains(weapon))
		{
			for (int row = 0; row < RowCollection.Length; row++)
			{
				if (InventorySpaces[row][2].fillWithWeapon(weapon))
				{
					WeaponList.Add(weapon);
					return;
				}
			}
		}
	}

	public void goToItemPage(Clue clue)
	{
		inventoryPage.SetActive(false);
		itemPage.SetActive(true);
	}

	public void goToItemPage(Weapon weapon)
	{
		inventoryPage.SetActive(false);
		itemPage.SetActive(true);
	}

	public void goToInventoryPage()
	{
		itemPage.SetActive(false);
		inventoryPage.SetActive(true);
	}


	void ToggleMenu(InputAction.CallbackContext context)
	{

	}



}
