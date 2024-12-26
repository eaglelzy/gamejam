using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using TS.Character;
using UnityEngine;

public class PickableFood : PickableItem
{
    protected override void Pick(GameObject picker)
    {
        Health health = _character.GetComponent<Health>();
        health.Damage(-15, gameObject);
    }
}
