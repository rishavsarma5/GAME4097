using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponFoundPopup: MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] TextMeshProUGUI weaponPopupText;

    private void Awake()
    {
        weaponPopupText.text = weapon.weaponFoundText;
    }
}
