using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class NotepadUI : MonoBehaviour
{
	public GameObject[] RowCollection;

	private InventorySpace[][] InventorySpaces;

	public GameObject inventoryPage;
	public GameObject itemPage;
	public GameObject changeScenesPage;
	public GameObject endTurnPage;

	public AudioClip newItemSound;
	public AudioClip clickSound;

	public ItemDisplay itemDisplayPage;

	private List<string> clueAndWeaponList;

	public TextMeshProUGUI promptHeader;
	public TextMeshProUGUI promptFooter;

	[Header("Tabs")]
	public GameObject scenePageTab;
	public GameObject endTurnTab;

	public bool endTurnPressed = false;
	public bool areTurnsLimited = true;

	void Awake()
	{
		itemPage.SetActive(false);
		changeScenesPage.SetActive(false);
		endTurnPage.SetActive(false);
		inventoryPage.SetActive(true);
		
		promptHeader.text = "CLUES";
		promptFooter.text = "click a space for info\n[MENU] to close";
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

		scenePageTab.SetActive(false);
		endTurnTab.SetActive(false);
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
		itemDisplayPage.Display(clue.description, clue.icon);
		inventoryPage.SetActive(false);
		endTurnPage.SetActive(false);
		changeScenesPage.SetActive(false);
		itemPage.SetActive(true);
	}

	public void goToItemPage(Weapon weapon)
	{
		AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
		itemDisplayPage.Display(weapon.weaponFoundText, weapon.icon);
		inventoryPage.SetActive(false);
		endTurnPage.SetActive(false);
		changeScenesPage.SetActive(false);
		itemPage.SetActive(true);
	}

	public void goToInvPage()
	{
		AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
		itemPage.SetActive(false);
		endTurnPage.SetActive(false);
		changeScenesPage.SetActive(false);
		inventoryPage.SetActive(true);
	}

	public void GoToEndTurnPage()
	{
		if (endTurnPage.activeSelf)
		{
			return;
		}
		AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
		itemPage.SetActive(false);
		changeScenesPage.SetActive(false);
		inventoryPage.SetActive(false);
		endTurnPage.SetActive(true);
	}

	public void GoToChangeScenesPage()
	{
		if (changeScenesPage.activeSelf)
		{
			return;
		}
		AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
		itemPage.SetActive(false);
		endTurnPage.SetActive(false);
		inventoryPage.SetActive(false);
		changeScenesPage.SetActive(true);
	}

	public void GoToCluesPage()
	{
		if (inventoryPage.activeSelf)
        {
			return;
        }
		AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
		endTurnPage.SetActive(false);
		changeScenesPage.SetActive(false);
		itemPage.SetActive(false);
		inventoryPage.SetActive(true);
		promptHeader.text = "CLUES";
		promptFooter.text = "click a space for info\n[MENU] to close";
	}

	public void TurnOnOtherTabs()
    {
		scenePageTab.SetActive(true);
		endTurnTab.SetActive(true);
	}

	public void TurnOffOtherTabs()
	{
		scenePageTab.SetActive(false);
		endTurnTab.SetActive(false);
	}

	public void ResetToBase()
	{
		endTurnPressed = false;
		endTurnPage.SetActive(false);
		changeScenesPage.SetActive(false);
		itemPage.SetActive(false);
		inventoryPage.SetActive(true);
		promptHeader.text = "CLUES";
		promptFooter.text = "click a space for info\n[MENU] to close";
	}

	public bool GetEndTurnPressed()
    {
		return endTurnPressed;
    }
}
