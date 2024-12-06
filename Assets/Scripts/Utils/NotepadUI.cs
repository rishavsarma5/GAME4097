using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NotepadUI : MonoBehaviour
{
	public GameObject[] ClueCollection;
	public GameObject[] WeaponCollection;

	private InventorySpace[][] ClueInventorySpaces;
	private InventorySpace[][] WeaponInventorySpaces;

	public GameObject cluesPage;
	public GameObject weaponsPage;
	public GameObject itemPage;
	public GameObject changeScenesPage;
	public GameObject endTurnPage;
	public GameObject mapPage;

	public AudioClip newItemSound;
	public AudioClip clickSound;

	public ItemDisplay itemDisplayPage;

	[Header("Tabs")]
	public GameObject scenePageTab;
	public GameObject endTurnTab;

	public bool endTurnPressed = false;
	public bool areTurnsLimited = true;

	void Awake()
	{
		Debug.Log("got to awake");
		itemPage.SetActive(false);
		changeScenesPage.SetActive(false);
		endTurnPage.SetActive(false);
		cluesPage.SetActive(false);
		weaponsPage.SetActive(false);
		mapPage.SetActive(true);

		// initialize clues inventory
		ClueInventorySpaces = new InventorySpace[ClueCollection.Length][];
		InventorySpace[] clueItemsList;
		for (int row = 0; row < ClueCollection.Length; row++)
		{
			ClueInventorySpaces[row] = new InventorySpace[3];
			clueItemsList = ClueCollection[row].GetComponentsInChildren<InventorySpace>();
			for (int item = 0; item < 3; item++)
			{
				ClueInventorySpaces[row][item] = clueItemsList[item];
			}
		}

		// initialize weapons inventory
		WeaponInventorySpaces = new InventorySpace[WeaponCollection.Length][];
		InventorySpace[] weaponItemsList;
		for (int row = 0; row < WeaponCollection.Length; row++)
		{
			WeaponInventorySpaces[row] = new InventorySpace[3];
			weaponItemsList = WeaponCollection[row].GetComponentsInChildren<InventorySpace>();
			for (int item = 0; item < 3; item++)
			{
				WeaponInventorySpaces[row][item] = weaponItemsList[item];
			}
		}

		//InitializeInventory();

		scenePageTab.SetActive(false);
		endTurnTab.SetActive(false);
	}

	public void InitializeInventory()
    {
		// re-add clues
		if (ClueGameManager.Instance.foundClues.Count != 0) {
			Debug.Log("got to initialize inventory");
			Debug.Log($"printing clue collection: {ClueCollection}");
			Debug.Log($"printing clue inventory: {ClueInventorySpaces}");
			// re-add clues 
			for (int row = 0; row < ClueCollection.Length; row++)
			{
				for (int item = 0; item < 3; item++)
				{
					int index = row * 3 + item;
					Debug.Log($"Index: {index}");
					if (index >= ClueGameManager.Instance.foundClues.Count) break;

					ClueInventorySpaces[row][item].FillWithClueFromList(ClueGameManager.Instance.foundClues, index);
				}
			}
		}

		// re-add weapons 
		if (ClueGameManager.Instance.foundWeapons.Count != 0)
        {
			for (int row = 0; row < WeaponCollection.Length; row++)
			{
				for (int item = 0; item < 3; item++)
				{
					int index = row * 3 + item;
					if (index >= ClueGameManager.Instance.foundWeapons.Count) break;

					WeaponInventorySpaces[row][item].FillWithWeaponFromList(ClueGameManager.Instance.foundWeapons, index);
				}
			}
		}
	}

	public void AddClue(Clue clue)
	{
		for (int row = 0; row < ClueCollection.Length; row++)
		{
			for (int space = 0; space < 3; space++)
			{
				if (ClueInventorySpaces[row][space].fillWithClue(clue))
				{
					AudioSource.PlayClipAtPoint(newItemSound, Camera.main.transform.position);
					return;
				}
			}
		}
	}

	public void AddWeapon(Weapon weapon)
	{
		for (int row = 0; row < WeaponCollection.Length; row++)
		{
			for (int space = 0; space < 3; space++)
			{
				if (WeaponInventorySpaces[row][space].fillWithWeapon(weapon))
				{
					AudioSource.PlayClipAtPoint(newItemSound, Camera.main.transform.position);
					return;
				}
			}
		}
	}

	public void goToItemPage(Clue clue)
	{
		Debug.Log("displaying item page");
		AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
		itemDisplayPage.Display(clue.description, clue.icon, clue.clueName);
		cluesPage.SetActive(false);
		weaponsPage.SetActive(false);
		endTurnPage.SetActive(false);
		changeScenesPage.SetActive(false);
		mapPage.SetActive(false);
		itemPage.SetActive(true);
	}

	public void goToItemPage(Weapon weapon)
	{
		AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
		itemDisplayPage.Display(weapon.weaponFoundText, weapon.icon, weapon.weaponName);
		cluesPage.SetActive(false);
		weaponsPage.SetActive(false);
		endTurnPage.SetActive(false);
		mapPage.SetActive(false);
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
		mapPage.SetActive(false);
		endTurnPage.SetActive(true);
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
		endTurnPage.SetActive(false);
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
		mapPage.SetActive(false);
		changeScenesPage.SetActive(true);
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
		weaponsPage.SetActive(false);
		mapPage.SetActive(false);
		cluesPage.SetActive(true);
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
		mapPage.SetActive(false);
		weaponsPage.SetActive(true);
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
		itemPage.SetActive(false);
		changeScenesPage.SetActive(false);
		weaponsPage.SetActive(false);
		cluesPage.SetActive(false);
		mapPage.SetActive(true);
	}

	public bool GetEndTurnPressed()
    {
		return endTurnPressed;
    }
}
