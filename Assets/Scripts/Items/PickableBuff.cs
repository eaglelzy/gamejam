using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using TS.Weapon;
using UnityEngine;

public class PickableBuff : PickableItem
{
    public int bulletCountBuff = 0;
    public float bulletDamageBuff = 0;
    public int bulletFireRateBuff = 0;
    public int maxAmmoBuff = 0 ;

    protected override void Pick(GameObject picker)
    {
        WandController weapon =  (WandController)_character.weaponController;

        weapon.ModifyBulletCount(bulletCountBuff);
        weapon.ModifyDamage(bulletDamageBuff);
        weapon.ModifyFireRate(bulletFireRateBuff);
        weapon.ModifyMaxAmmo(maxAmmoBuff);   
    }
}
