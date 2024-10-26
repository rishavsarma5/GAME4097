using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;

    public GameNPC relatedNPC;
    public GameObject weaponPrefab;
    public Transform spawnLocation;

    public bool isFound = false;

    public void OnWeaponFound()
    {
        //relatedNPC
    }
   
}
