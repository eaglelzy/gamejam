using UnityEngine;

namespace TS.Weapon {

    public class WandController : WeaponController {
        public GameObject projectilePrefab;
        public Transform firePoint;
        public Vector2 force;

        public override void Aim(Vector2 target) {
            var direction = target - (Vector2)transform.position;
            var angle = Vector3.SignedAngle(Vector3.up, direction, Vector3.forward);
            transform.eulerAngles = new Vector3(0, 0, angle);

            Debug.DrawRay(transform.position, target - (Vector2)transform.position, Color.yellow);
        }

        public override void Attack() {
            var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            var rb = projectile.GetComponent<Rigidbody2D>();
            rb.AddRelativeForce(force, ForceMode2D.Impulse);
        }
    }
}