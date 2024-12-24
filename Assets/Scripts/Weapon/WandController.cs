using System.Collections;
using UnityEngine;

namespace TS.Weapon {

    public class WandController : WeaponController {
        [Tooltip("武器配置")]
        public WeaponData currentWeaponData;
        private float _fireCoolDown = 0f;

        public bool isReloading = false;

        public GameObject projectilePrefab;
        public Transform firePoint;
        public float speed = 16;
        private float _nextFireTime;  //下一次可以开火的时间

        public override void Aim(Vector2 target) {
            Vector3 mousePosition=Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mousePosition - transform.position;

            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg ;
            transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        }
        

        public override void Attack() {
            float angleStep = firePoint.rotation.eulerAngles.z / currentWeaponData.bulletCount;
            float angle = firePoint.rotation.eulerAngles.z;

            for (int i = 0; i < currentWeaponData.bulletCount; i++) {
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                var projectile = Instantiate(currentWeaponData.bulletPrefab, firePoint.position, rotation);
                var rb = projectile.GetComponent<Rigidbody2D>();
                rb.AddRelativeForce(new Vector2(0, speed * rb.mass), ForceMode2D.Impulse);
                angle += angleStep;
            }
        }

        /// <summary>
        /// 尝试攻击，如果可以攻击则返回true，否则返回false
        /// </summary>
        /// <returns></returns>
        public override bool TryAttack(){
            //Debug.Log(_fireCoolDown);
            if (_fireCoolDown > 0 || isReloading) return false;

            Attack();

            StartFireCoolDown();
            return true;
        }

        /// <summary>
        /// 开火冷却
        /// </summary>
        private void StartFireCoolDown()
        {
            _fireCoolDown = 1f / currentWeaponData.fireRate;
            StartCoroutine(CoolDownCoroutine());
        }

        private IEnumerator CoolDownCoroutine()
        {
            while(_fireCoolDown>0)
            {
                yield return new WaitForSeconds(0.01f);
                _fireCoolDown -= 0.01f;
            }
        }
    }
}