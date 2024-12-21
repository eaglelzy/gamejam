using UnityEngine;

namespace TS.Weapon {

    public class WandController : WeaponController {
        public GameObject projectilePrefab;
        public Transform firePoint;
        public float speed = 10;
        public float cooldown = 0.1f;
        private float _nextFireTime;  //下一次可以开火的时间

        public override void Aim(Vector2 target) {
            var direction = target - (Vector2)transform.position;
            var angle = Vector3.SignedAngle(Vector3.up, direction, Vector3.forward);
            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        public override void Attack() {
            if (Time.time < _nextFireTime) return;
            var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            var rb = projectile.GetComponent<Rigidbody2D>();
            rb.AddRelativeForce(new Vector2(0, speed * rb.mass), ForceMode2D.Impulse);
            _nextFireTime = Time.time + cooldown;
        }
    }
}