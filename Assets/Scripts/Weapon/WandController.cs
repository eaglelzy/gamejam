﻿using System.Collections;
using UnityEngine;

namespace TS.Weapon {

    public class WandController : WeaponController {
        [Tooltip("武器配置")]
        public WeaponData currentWeaponData;

        #region Runtime Data
        public float Damage {get, private set;};
        public int BulletCount {get, private set;};
        public int FireRate {get, private set;};
        public int MaxAmmo {get, private set;};
        public int CurrentAmmo {get; private set;};

        #endregion

        private float _fireCoolDown = 0f;

        public bool isReloading = false;

        public GameObject projectilePrefab;
        public Transform firePoint;
        public float speed = 16;
        private float _nextFireTime;  //下一次可以开火的时间

        //初始化
        public void Init(WeaponData weaponData)
        {
            SwitchWeapon(weaponData);
        }

        public override void Aim(Vector2 target) {
            Vector3 mousePosition=Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mousePosition - transform.position;

            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg ;
            transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        }
        

        public override void Attack() {
            //如果一次发射的子弹数量为1，则直接发射
            if (currentWeaponData.bulletCount == 1) {
                var projectile = Instantiate(currentWeaponData.bulletPrefab, firePoint.position, firePoint.rotation);
                var rb = projectile.GetComponent<Rigidbody2D>();
                rb.AddRelativeForce(new Vector2(0, speed * rb.mass), ForceMode);
            }else{
                float spreadAngle = 45f; // 扩散角度
                float angleStep = spreadAngle / (currentWeaponData.bulletCount - 1);
                float startAngle = -spreadAngle / 2f;

                for (int i = 0; i < currentWeaponData.bulletCount; i++) {
                    // 计算每发子弹的角度
                    float angle = startAngle + angleStep * i;
                    // 计算子弹的方向s
                    
                    float bulletAngle = transform.eulerAngles.z + angle;

                    Vector2 direction = new Vector2(Mathf.Cos(bulletAngle * Mathf.Deg2Rad), Mathf.Sin(bulletAngle * Mathf.Deg2Rad));

                    var projectile = Instantiate(currentWeaponData.bulletPrefab, firePoint.position, Quaternion.identity);
                    var rb = projectile.GetComponent<Rigidbody2D>();
                    // rb.AddRelativeForce(new Vector2(0, speed * rb.mass), ForceMode2D.Impulse);
                    rb.velocity = direction * speed;
                    
                    // 如果子弹需要旋转，可以设置其角度
                    projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
                }
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

        /// <summary>
        /// 切换武器
        /// </summary>
        /// <param name="weaponData">要切换的武器数据</param>
        public void SwitchWeapon(WeaponData weaponData)
        {
            currentWeaponData = weaponData;
            Damage = currentWeaponData.damage;
            BulletCount = currentWeaponData.bulletCount;
            FireRate = currentWeaponData.fireRate;
            MaxAmmo = currentWeaponData.maxAmmo;
            
            CurrentAmmo = MaxAmmo;
        }
    }
}