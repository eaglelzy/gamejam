using System.Collections;
using System.Collections.Generic;
using TS.Weapon;
using UnityEngine;
using UnityEngine.UI;

public class UIWeapon : MonoBehaviour
{
    public static UIWeapon Instance;

    private Image weaponImage;
    private Image ammoImage;

    public void Awake() {
        Instance = this;
        weaponImage = GetComponent<Image>();
        ammoImage = transform.Find("Ammo").GetComponent<Image>();
    }

    public void SetImage(Sprite sprite)
    {
        weaponImage.sprite = sprite;
    }

    public void UpdateAmmo(int ammo, int maxAmmo)
    {
        ammoImage.fillAmount = (float)ammo / maxAmmo;
    }
}
