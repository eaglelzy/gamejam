using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public Sprite weaponSprite;

    public float damage;
    public float bulletSpeed;
    public int maxAmmo;
    public bool isBurst;
    public float reloadTime;
    public float recoil;

    [Range(1, 12)]
    [Tooltip("一次发射的子弹数量")] public int bulletCount;
    [Tooltip("每秒攻击的次数")] public int fireRate;

    public GameObject bulletPrefab;
}
