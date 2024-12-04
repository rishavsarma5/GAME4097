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

	public GameObject cluesPage;
	public GameObject weaponsPage;
	public GameObject itemPage;
	public GameObject changeScenesPage;
	public GameObject endTurnPage;
	public GameObject mapPage;

	public AudioClip newItemSound;
	public AudioClip clickSound;

	public ItemDisplay itemDisplayPage;

	private List<string> clueAndWeaponList = new();

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
		cluesPage.SetActive(true);
		weaponsPage.SetActive(false);
		
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
			for (int row = 0; row < 3; row++)
			{
				for (int space = 0; space < 3; space++)
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
			for (int row = 3; row < 6; row++)
			{
				for (int space = 0; space < 3; space++)
				{
					if (InventorySpaces[row][space].fillWithWeapon(weapon))
					{
						AudioSource.PlayClipAtPoint(newItemSound, Camera.main.transform.position);
						clueAndWeaponList.Add(weapon.name);
						return;
					}
				}
			}
		}
	}

	public void goToItemPage(Clue clue)
	{
		AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
		itemDisplayPage.Display(clue.description, clue.icon, clue.clueName);
		cluesPage.SetActive(false);
		weaponsPage.SetActive(false);
		endTurnPage.SetActive(false);
		changeScenesPage.SetActive(false);
		itemPage.SetActive(true);
		mapPage.SetActive(false);
	}

	public void goToItemPage(Weapon weapon)
	{
		AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
		itemDisplayPage.Display(weapon.weaponFoundText, weapon.icon, weapon.weaponName);
		cluesPage.SetActive(false);
		weaponsPage.SetActive(false);
		endTurnPage.SetActive(false);
		changeScenesPage.SetActive(false);
		itemPage.SetActive(true);
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
		cluesPage.SetActive(false);
		weaponsPage.SetActive(false);
		endTurnPage.SetActive(true);
		mapPage.SetActive(false);
	}

	public void GoToMapPage()
	{
		if (mapPage.activeSelf)
		{
			return;
		}
		AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
		itemPage.SetActive(false);
		changeScenesPage.SetActive(false);
		cluesPage.SetActive(false);
		weaponsPage.SetActive(false);
		endTurnPage.SetActive(true);
		mapPage.SetActive(true);
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
		cluesPage.SetActive(false);
		weaponsPage.SetActive(false);
		changeScenesPage.SetActive(true);
		mapPage.SetActive(false);
	}

	public void GoToCluesPage()
	{
		if (cluesPage.activeSelf)
        {
			return;
        }
		AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
		endTurnPage.SetActive(false);
		changeScenesPage.SetActive(false);
		itemPage.SetActive(false);
		cluesPage.SetActive(true);
		weaponsPage.SetActive(false);
		mapPage.SetActive(false);
	}

	public void GoToWeaponsPage()
	{
		if (weaponsPage.activeSelf)
		{
			return;
		}
		AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
		endTurnPage.SetActive(false);
		changeScenesPage.SetActive(false);
		itemPage.SetActive(false);
		cluesPage.SetActive(false);
		weaponsPage.SetActive(true);
		mapPage.SetActive(false);
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
		cluesPage.SetActive(true);
		weaponsPage.SetActive(false);
		mapPage.SetActive(false);
	}

	public bool GetEndTurnPressed()
    {
		return endTurnPressed;
    }
}
