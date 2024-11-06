using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClueGameManager : MonoBehaviour
{
    public static ClueGameManager Instance;

    [Header("Initial Lists")]
    public List<Clue> firstClues;
    public List<GameObject> firstClueGameObjects;
    public List<Clue> secondClues;
    public List<GameObject> secondClueGameObjects;
    public List<Clue> thirdClues;
    public List<GameObject> thirdClueGameObjects;
    public List<Weapon> initialWeaponList;
    public List<GameObject> weaponGameObjects;

    [Space(10)]
    [Header("Active Clues and Weapons")]
    public List<Clue> activeClues;
    public List<Weapon> activeWeapons;

    [Space(10)]
    [Header("Clues and Weapons Found")]
    [SerializeField] private List<Clue> foundClues;
    [SerializeField] public List<Weapon> foundWeapons;


    [Space(15)]
    [SerializeField] private int numWeaponsToSpawn = 2;

    [SerializeField] private bool actionCompleted = false;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            throw new System.Exception("Can't be two Clue Game Managers!");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        firstClueGameObjects = GameObject.FindGameObjectsWithTag("Clue1Interactable").ToList();
        secondClueGameObjects = GameObject.FindGameObjectsWithTag("Clue2Interactable").ToList();
        thirdClueGameObjects = GameObject.FindGameObjectsWithTag("Clue3Interactable").ToList();
        weaponGameObjects = GameObject.FindGameObjectsWithTag("WeaponInteractable").ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClue1Found(Clue clue)
    {
        if (firstClues.Contains(clue))
        {
            foundClues.Add(clue);
            clue.isFound = true;
            Clue clue2 = clue.relatedNPC.clues[1];
            if (secondClues.Contains(clue2))
            {
                GameObject clue2GameObject = secondClueGameObjects
                .Find(go => go.GetComponent<ClueController>().clue == clue2);

                if (clue2GameObject != null)
                {
                    clue2GameObject.SetActive(true);
                    activeClues.Add(clue2);
                    Debug.Log($"Second clue {clue2} GameObject spawned!");
                }
                else
                {
                    Debug.LogWarning("Could not find the GameObject for the second clue.");
                }
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
            StartCoroutine(TransitionToSuspectSelect());
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

    public void OnClue3Found(Clue clue)
    {
        if (thirdClues.Contains(clue))
        {
            //thirdClues.Remove(clue);
            foundClues.Add(clue);
            clue.isFound = true;
        }
        else
        {
            throw new System.Exception("this clue is not a third clue");
        }
    }

    public void OnWeaponFound(Weapon weapon)
    {
        foundWeapons.Add(weapon);
        weapon.isFound = true;
    }

    public void InitializeStartingWeapons()
    {
        foundWeapons.Clear();
        activeWeapons.Clear();

        // Shuffle initialWeaponList in place
        for (int i = weaponGameObjects.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            GameObject temp = weaponGameObjects[i];
            temp.GetComponent<WeaponController>().weapon.isFound = false;
            weaponGameObjects[i] = weaponGameObjects[randomIndex];
            weaponGameObjects[randomIndex] = temp;
        }

        for (int i = 0; i < numWeaponsToSpawn; i++)
        {
            GameObject weaponToSpawn = weaponGameObjects[i];
            weaponToSpawn.SetActive(true);
            Weapon weapon = initialWeaponList.Find(w => w == weaponToSpawn.GetComponent<WeaponController>().weapon);
            activeWeapons.Add(weapon);
            Debug.Log($"Weapon {weapon.weaponName} spawned.");
        }
    }
    public void InitializeAllFirstClues()
    {
        foundClues.Clear();

        foreach(GameObject clue in firstClueGameObjects) {
            clue.SetActive(true);
            activeClues.Add(firstClues.Find(c => c == clue.GetComponent<ClueController>().clue));
        }
        Debug.Log($"All first clues spawned!");
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

}
