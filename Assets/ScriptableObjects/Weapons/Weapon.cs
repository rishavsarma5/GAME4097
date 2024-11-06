using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;

    public GameNPC relatedNPC;

    public bool isFound = false;

    [TextArea] public string weaponFoundText;

    public void SpawnWeaponFoundText()
    {
        FloatingTextSpawner.Instance.SpawnFloatingText(weaponFoundText);
    }
   
}
