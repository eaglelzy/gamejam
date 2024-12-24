using MoreMountains.Tools;
using System.Collections;
using UnityEngine;

namespace TS.Projectile {

    public class Bullet : MonoBehaviour {
        public float lifetime = 1;
        [Tooltip("击中石头时施加的扭矩")]
        public float torque = -100;

        [Tooltip("碰撞给的力大小")]
        [SerializeField]
        private int bulletForce = 500;

        [Tooltip("石头最大速度")]
        [SerializeField]
        private int maxSpeed = 2;

        [Tooltip("爆炸特效")]
        [SerializeField]
        private GameObject explodeEffect;

        // 滚动方向
        private Vector2 scrollDirection = new(1, 0.2f);

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
            if (other.CompareTag("Rock"))
            {
                var rb = collision.attachedRigidbody;
                var targetDir = other.transform.position - transform.position;
                rb.AddForce(scrollDirection * (targetDir.x > 0 ? 1 : -1) * bulletForce, ForceMode2D.Impulse);
                if (collision.attachedRigidbody.velocity.x > maxSpeed)
                {
                    rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
                }
                Destroy(gameObject);
            }
            else if (other.CompareTag("Enemy"))
            {
                other.SetActive(false);
                var effect = Instantiate(explodeEffect, transform.position, Quaternion.identity);
                Destroy(effect, 0.5f);
                Destroy(gameObject);
            }
        }
    }
}