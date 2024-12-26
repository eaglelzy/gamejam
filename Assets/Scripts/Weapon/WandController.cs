using System.Collections;
using TS.Projectile;
using UnityEngine;

namespace TS.Weapon {

    public class WandController : WeaponController {
        [Tooltip("武器配置")]
        public WeaponData currentWeaponData;

        #region Runtime Data
        public float Damage {get; private set;}
        public int BulletCount {get; private set;}
        public int FireRate {get; private set;}
        public int MaxAmmo {get; private set;}
        public int CurrentAmmo {get; private set;}

        #endregion

        private float _fireCoolDown = 0f;

        public bool isReloading = false;

        public GameObject projectilePrefab;
        public Transform firePoint;
        private float speed = 16;
        private float _nextFireTime;  //下一次可以开火的时间

        private void Awake() {
            Init();
        }
        //初始化
        public void Init()
        {
            SwitchWeapon(currentWeaponData);
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
                var bullet = projectile.GetComponent<Bullet>();
                rb.AddRelativeForce(new Vector2(0, speed * rb.mass), ForceMode2D.Impulse);
                // 初始化子弹脚本
                bullet.Init(Damage);

                CurrentAmmo--;
                UIWeapon.Instance.UpdateAmmo(CurrentAmmo, MaxAmmo);
            }else{
                float spreadAngle = 45f; // 扩散角度
                float angleStep = spreadAngle / (currentWeaponData.bulletCount - 1);
                float startAngle = -spreadAngle / 2f + 90f;

                for (int i = 0; i < currentWeaponData.bulletCount; i++) {
                    // 计算每发子弹的角度
                    float angle = startAngle + angleStep * i;
                    // 计算子弹的方向s
                    
                    float bulletAngle = transform.eulerAngles.z + angle;

                    Vector2 direction = new Vector2(Mathf.Cos(bulletAngle * Mathf.Deg2Rad), Mathf.Sin(bulletAngle * Mathf.Deg2Rad));

                    var projectile = Instantiate(currentWeaponData.bulletPrefab, firePoint.position, Quaternion.identity);
                    var rb = projectile.GetComponent<Rigidbody2D>();
                    var bullet = projectile.GetComponent<Bullet>();
                    // 初始化子弹脚本
                    bullet.Init(Damage);
                    // rb.AddRelativeForce(new Vector2(0, speed * rb.mass), ForceMode2D.Impulse);
                    rb.velocity = direction * speed;
                    
                    // 如果子弹需要旋转，可以设置其角度
                    projectile.transform.rotation = Quaternion.Euler(0, 0, angle);

                    CurrentAmmo--;
                    UIWeapon.Instance.UpdateAmmo(CurrentAmmo, MaxAmmo);
                }
            }
            if(CurrentAmmo <= 0)
            {
                TryReload();
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

        public bool TryReload()
        {
            if (CurrentAmmo == MaxAmmo || isReloading) return false;
            isReloading = true;
            Reload();
            return true;
        }

        public void Reload()
        {
            StartCoroutine(ReloadCoroutine());
        }

        private IEnumerator ReloadCoroutine()
        {
            yield return new WaitForSeconds(currentWeaponData.reloadTime);
            CurrentAmmo = MaxAmmo;
            UIWeapon.Instance.UpdateAmmo(CurrentAmmo, MaxAmmo);
            isReloading = false;
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

            UIWeapon.Instance.SetImage(currentWeaponData.weaponSprite);
        }

        // 丢弃武器在场景中
        public void DropWepaon(WeaponData weaponData)
        {
            GameObject newGameObject = new GameObject(weaponData.weaponName);
            // 添加SpriteRenderer组件并设置图片
            SpriteRenderer spriteRenderer = newGameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = weaponData.weaponSprite;

            //添加PickableItem
        }

        #region 武器属性修改接口

        // 修改子弹数量
        public void ModifyBulletCount(int count)
        {
            BulletCount += count;
        }

        //修改射速
        public void ModifyFireRate(int rate)
        {
            FireRate += rate;
        }

        //修改最大弹药数量
        public void ModifyMaxAmmo(int ammo)
        {
            MaxAmmo += ammo;
        }

        //修改伤害
        public void ModifyDamage(float damage)
        {
            Damage += damage;
        }

        #endregion
    }
}