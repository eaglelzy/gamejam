using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStates 
{
    public enum MeleeEnemyState { Idle, Move, Accelerate, Attack, IsHit, Dead }
    public enum RemoteEnemyState { Idle, Move, Attack, IsHit, Dead }
}
