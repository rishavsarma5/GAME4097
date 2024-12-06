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

	private List<Clue> cluesFoundList = new();
	private List<Weapon> weaponsFoundList = new();

	private bool isInitialized = false;

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

		scenePageTab.SetActive(false);
		endTurnTab.SetActive(false);

		isInitialized = true;
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
		if (isInitialized) ReInitializeInventory();
	}

	public void ReInitializeInventory()
    {
		// re-add clues 
		InventorySpace[] clueItemsList;
		for (int row = 0; row < ClueCollection.Length; row++)
		{
			clueItemsList = ClueCollection[row].GetComponentsInChildren<InventorySpace>();
			for (int item = 0; item < 3; item++)
			{
				int index = row * 3 + item;
				if (index == cluesFoundList.Count) break;

				ClueInventorySpaces[row][item] = clueItemsList[item].FillWithClueFromList(cluesFoundList, index);
			}
		}

		// re-add weapons 
		InventorySpace[] weaponItemsList;
		for (int row = 0; row < WeaponCollection.Length; row++)
		{
			weaponItemsList = WeaponCollection[row].GetComponentsInChildren<InventorySpace>();
			for (int item = 0; item < 3; item++)
			{
				int index = row * 3 + item;
				if (index == weaponsFoundList.Count) break;

				WeaponInventorySpaces[row][item] = weaponItemsList[item].FillWithWeaponFromList(weaponsFoundList, index);
			}
		}
	}

	public void AddClue(Clue clue)
	{
		if (!cluesFoundList.Contains(clue))
		{
			for (int row = 0; row < 3; row++)
			{
				for (int space = 0; space < 3; space++)
				{
					if (ClueInventorySpaces[row][space].fillWithClue(clue))
					{
						AudioSource.PlayClipAtPoint(newItemSound, Camera.main.transform.position);
						cluesFoundList.Add(clue);
						return;
					}
				}
			}
		}
	}

	public void AddWeapon(Weapon weapon)
	{
		if (!weaponsFoundList.Contains(weapon))
		{
			for (int row = 3; row < 6; row++)
			{
				for (int space = 0; space < 3; space++)
				{
					if (WeaponInventorySpaces[row][space].fillWithWeapon(weapon))
					{
						AudioSource.PlayClipAtPoint(newItemSound, Camera.main.transform.position);
						weaponsFoundList.Add(weapon);
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
