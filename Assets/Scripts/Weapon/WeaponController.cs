using UnityEngine;

namespace TS.Weapon {

    public abstract class WeaponController : MonoBehaviour {
        public static readonly int attackId = Animator.StringToHash("attack");

        public abstract void Attack();

        public abstract bool TryAttack();

        public abstract void Aim(Vector2 target);
    }
}