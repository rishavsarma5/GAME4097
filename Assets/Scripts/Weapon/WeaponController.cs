using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Weapon weapon;
    private GameObject descriptionTextObject;

    public void OnWeaponGrabbed()
    {
        ClueGameManager.Instance.OnWeaponFound(weapon);
    }

    public void SpawnWeaponDescriptionText()
    {
        descriptionTextObject = FloatingTextSpawner.Instance.SpawnFloatingTextAndReturnGameObject(weapon.weaponFoundText);
    }

    public void DestroyWeaponDescriptionText()
    {
        Destroy(descriptionTextObject);
    }
}
