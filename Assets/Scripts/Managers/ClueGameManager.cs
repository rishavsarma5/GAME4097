using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueGameManager : MonoBehaviour
{
    public static ClueGameManager Instance;

    [Header("Initial Lists")]
    public List<Clue> firstClues;
    public List<Clue> secondClues;
    public List<Clue> thirdClues;
    public List<Weapon> initialWeaponList;

    [Space(10)]
    [Header("Active Clues and Weapons")]
    public List<Clue> activeClues;
    public List<Weapon> activeWeapons;

    [Space(10)]
    [Header("Clues and Weapons Found")]
    [SerializeField] private List<Clue> foundClues;
    [SerializeField] private List<Weapon> foundWeapons;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClue1Found(Clue clue)
    {
        if (firstClues.Contains(clue))
        {
            //firstClues.Remove(clue);
            foundClues.Add(clue);
            clue.isFound = true;
            Clue clue2 = clue.relatedNPC.clues[1];
            if (secondClues.Contains(clue2))
            {
                //secondClues.Remove(clue2);
                activeClues.Add(clue2);
                Instantiate(clue2.clueObject, clue2.clueObject.transform.position, Quaternion.identity);
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
        for(int i = 0; i < numWeaponsToSpawn; i++)
        {
            Weapon weapon = initialWeaponList[Random.Range(0, initialWeaponList.Count)];
            //initialWeaponList.Remove(weapon);
            activeWeapons.Add(weapon);
            Instantiate(weapon.weaponPrefab, weapon.spawnLocation.position, Quaternion.identity);
            Debug.Log($"Weapon {weapon.weaponName} spawned at {weapon.spawnLocation.position}");
        }
    }

    public void InitializeAllFirstClues()
    {
        foreach(Clue clue in firstClues) {
            //firstClues.Remove(clue);
            activeClues.Add(clue);
            Instantiate(clue.clueObject, clue.clueObject.transform.position, Quaternion.identity);
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
