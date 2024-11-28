using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClueGameManager : MonoBehaviour
{
    public static ClueGameManager Instance;

    [Header("Initial Clues and Weapons")]
    public List<Clue> firstClues;
    [SerializeField] private List<GameObject> firstClueGameObjects;
    public List<Clue> secondClues;
    [SerializeField] private List<GameObject> secondClueGameObjects;
    public List<Weapon> initialWeapons;
    [SerializeField] private List<GameObject> weaponGameObjects;

    private Dictionary<Clue, GameObject> listOfClues = new Dictionary<Clue, GameObject>();
    private Dictionary<Weapon, GameObject> listOfWeapons = new Dictionary<Weapon, GameObject>();

    [Space(5)]
    [Header("Active Clues and Weapons")]
    public List<GameObject> activeClues;
    public List<GameObject> activeWeapons;

    [Space(5)]
    [Header("Clues and Weapons Found")]
    [SerializeField] private List<Clue> foundClues;
    [SerializeField] public List<Weapon> foundWeapons;

    [Space(10)]
    [SerializeField] private int numWeaponsToSpawn = 2;
    [SerializeField] private bool actionCompleted = false;

	public NotepadUI inventoryNotepad;
	public InputActionReference menuButton;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        // get all clues and weapons from the scene
        firstClueGameObjects = GameObject.FindGameObjectsWithTag("Clue1Interactable").ToList();
        //Debug.Log($"{firstClueGameObjects.Count} First Clue Objects found!");
        secondClueGameObjects = GameObject.FindGameObjectsWithTag("Clue2Interactable").ToList();
        //Debug.Log($"{secondClueGameObjects.Count} Second Clue Objects found!");
        weaponGameObjects = GameObject.FindGameObjectsWithTag("WeaponInteractable").ToList();
        //Debug.Log($"{weaponGameObjects.Count} Weapon Objects found!");

        AddAllCluesToDict();
    }
    // Start is called before the first frame update
    void Start()
    {
		menuButton.action.started += ToggleMenu;
    }

    private void OnDestroy()
    {
        menuButton.action.started -= ToggleMenu;
    }

    void ToggleMenu(InputAction.CallbackContext context)
	{
		inventoryNotepad.gameObject.SetActive(!inventoryNotepad.gameObject.activeSelf);
	}

    public void OnClue1Found(Clue clue)
    {
        // if clue found is actually a clue 1
        if (firstClues.Contains(clue))
        {
            // get the second clue of the related NPC for found Clue 1
            foundClues.Add(clue);
            clue.isFound = true;

			inventoryNotepad.AddClue(clue);

			Clue clue2 = clue.relatedNPC.clues[1];

            // if second clue is actually a clue 2
            if (secondClues.Contains(clue2))
            {
                // set clue 2 game object to active in the scene and add to active clues
                GameObject clue2Object = listOfClues[clue2];
                clue2Object.SetActive(true);
                activeClues.Add(clue2Object);
            } else
            {
                throw new System.Exception("next clue fetched is not a second clue");
            }
            
        } else
        {
            throw new System.Exception("this clue is not a first clue");
        }
    }

    public void OnClue2Found(Clue clue)
    {
        if (secondClues.Contains(clue))
        {
            //secondClues.Remove(clue);
            foundClues.Add(clue);
            clue.isFound = true;
            //StartCoroutine(TransitionToSuspectSelect());
            /*
            Clue clue3 = clue.relatedNPC.clues[2];
            if (secondClues.Contains(clue3))
            {
                //secondClues.Remove(clue3);
                activeClues.Add(clue3);
                Instantiate(clue3.clueObject, clue3.clueObject.transform.position, Quaternion.identity);
            }
            else
            {
                throw new System.Exception("next clue fetched is not a third clue");
            }
            */
        }
        else
        {
            throw new System.Exception("this clue is not a second clue");
        }
    }

    public void OnWeaponFound(Weapon weapon)
    {
        foundWeapons.Add(weapon);
        weapon.isFound = true;
		inventoryNotepad.AddWeapon(weapon);
    }

    public void InitializeStartingWeapons()
    {
        foundWeapons.Clear();
        activeWeapons.Clear();

        // Shuffle initialWeaponList in place
        ShuffleList(weaponGameObjects);

        for (int i = 0; i < numWeaponsToSpawn; i++)
        {
            GameObject weaponToSpawn = weaponGameObjects[i];
            weaponToSpawn.SetActive(true);
            Weapon weaponSO = weaponToSpawn.GetComponent<WeaponController>().weapon;
            weaponSO.isFound = false;
            listOfWeapons.Add(weaponSO, weaponToSpawn);
            activeWeapons.Add(weaponToSpawn);
            Debug.Log($"Weapon {weaponSO.weaponName} spawned.");
        }
    }
    public void InitializeAllFirstClues()
    {
        foundClues.Clear();

        foreach(GameObject clue in firstClueGameObjects) {
            clue.SetActive(true);
            activeClues.Add(clue);
        }
        Debug.Log($"All first clues spawned!");
    }

    public void SaveInitialization()
    {
        GameProgressManager.Instance.SaveTotalClues(listOfClues.Count);
        GameProgressManager.Instance.SaveTotalWeapons(listOfWeapons.Count);
    }

    public void SaveCluesAndWeaponsFound()
    {
        GameProgressManager.Instance.SaveFoundClues(foundClues.Count);
        GameProgressManager.Instance.SaveFoundWeapons(foundWeapons.Count);
    }

    public void SetActionCompleted()
    {
        actionCompleted = true;
    }

    public bool GetActionCompleted()
    {
        return actionCompleted;
    }

    public void ResetActionCompleted()
    {
        actionCompleted = false;
    }

    public IEnumerator TransitionToSuspectSelect()
    {
        FloatingTextSpawner.Instance.SpawnFloatingText("Directing to Guessing Area...");
        yield return new WaitForSeconds(2f);
        SetActionCompleted();
    }

    private void AddAllCluesToDict()
    {
        foreach (GameObject clue1 in firstClueGameObjects)
        {
            Clue clueSO = clue1.GetComponent<ClueController>().clue;
            clueSO.isFound = false;
            listOfClues.Add(clueSO, clue1);
        }

        foreach (GameObject clue2 in secondClueGameObjects)
        {
            Clue clueSO = clue2.GetComponent<ClueController>().clue;
            clueSO.isFound = false;
            listOfClues.Add(clueSO, clue2);
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
