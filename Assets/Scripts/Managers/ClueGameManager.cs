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
    [SerializeField] private List<Clue> foundWeapons;

    [Space(15)]
    [SerializeField] private int numWeaponsToSpawn = 2;

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

    public void InitializeStartingWeapons()
    {
        for(int i = 0; i < numWeaponsToSpawn; i++)
        {
            Weapon weapon = initialWeaponList[Random.Range(0, initialWeaponList.Count)];
            activeWeapons.Add(weapon);
            Instantiate(weapon.weaponPrefab, weapon.spawnLocation.position, weapon.spawnLocation.rotation);
            Debug.Log($"Weapon {weapon.weaponName} spawned!");
        }
    }

    public void InitializeAllFirstClues()
    {
        foreach(Clue clue in firstClues) {
            activeClues.Add(clue);
            clue.clueObject.SetActive(true);
        }
        Debug.Log($"All fist clues spawned!");
    }
}
