using TS.Weapon;
using UnityEngine;

namespace TS.Player {

    public class PlayerControl : MonoBehaviour {
        public Animator animator;
        public WeaponController weaponController;

        private void Start() {
            animator = GetComponent<Animator>();
            weaponController = GetComponentInChildren<WeaponController>();
        }

        private void Update() {

            if (Input.GetMouseButton(0)) {
                weaponController.TryAttack();
            }
            //var aimPoint = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            var aimPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); //可以直接用Camera.main获取主相机
            weaponController.Aim(aimPoint);
        }
    }
}