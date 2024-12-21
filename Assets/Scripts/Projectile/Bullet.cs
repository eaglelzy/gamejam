using UnityEngine;

namespace TS.Projectile {

    public class Bullet : MonoBehaviour {
        public float lifetime = 1;
        private float destroyTime;

        private void Start() {
            destroyTime = Time.time + lifetime;
        }

        private void Update() {
            if (Time.time < destroyTime) return;
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            var other = collision.gameObject;
            if (other.CompareTag("Enemy"))
            {
                Destroy(other);
            }
            Destroy(gameObject);
        }
    }
}