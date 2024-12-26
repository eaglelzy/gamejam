using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using TS.Player;
using TS.Weapon;
using UnityEngine;

public class PickableWeapon : PickableItem
{
    [Tooltip("武器配置")]
    public WeaponData weaponData;
    
    /// <summary>
    /// What happens when the weapon gets picked
    /// </summary>
    protected override void Pick(GameObject picker)
    {
        //目前只考虑远程武器
        WandController weapon =  (WandController)_character.weaponController;
        weapon.SwitchWeapon(weaponData);
        EffectSoundManager.Instance.PlayEffectSound("Pick");
    }
}
