using UnityEngine;

namespace TS.Weapon {

    public class SwordController : WeaponController {
        public Animator swordAnimator;

        public override bool TryAttack() {
            Attack();
            return true;
        }

        public override void Attack() {
            swordAnimator.SetTrigger(attackId);
            //Physics2D.OverlapArea()
        }

        public override void Aim(Vector2 target) {
            transform.LookAt(target);
        }
    }
}