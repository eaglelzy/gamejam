using UnityEngine;

namespace TS.Projectile {

    public class Bullet : MonoBehaviour {
        public float lifetime = 1;
        [Tooltip("击中石头时施加的扭矩")]public float torque = -100;
        private float _destroyTime;

        private void Start() {
            _destroyTime = Time.time + lifetime;
        }

        private void Update() {
            if (Time.time < _destroyTime) return;
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            var other = collision.gameObject;
            if (other.CompareTag("Rock")) {
                collision.attachedRigidbody.AddTorque(torque, ForceMode2D.Impulse);
            } else if (other.CompareTag("Enemy")) {
            }
            Destroy(gameObject);
        }
    }
}