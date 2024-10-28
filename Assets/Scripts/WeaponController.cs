using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnWeaponGrabbed()
    {
        ClueGameManager.Instance.OnWeaponFound(weapon);
    }

    public void SpawnFloatingText()
    {
        FloatingTextSpawner.Instance.SpawnFloatingText(weapon.weaponFoundText);
    }
}
