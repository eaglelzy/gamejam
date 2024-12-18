using TS.Weapon;
using UnityEngine;

namespace TS.Player {

    public class PlayerControl : MonoBehaviour {
        public float moveSpeed = 5f; 
        public Rigidbody2D rb; 
        public Animator animator; 
        public WeaponController weaponController;
        //public Camera playerCamera;
        private Vector2 movement;

        private void Update() {

            movement.x = Input.GetAxisRaw("Horizontal"); 
            movement.y = Input.GetAxisRaw("Vertical");   

            // if(movement.magnitude > 0) { animator.Play("Samurai_Run"); } else {
            // animator.Play("Samurai_Idle"); }

            if (movement.x > 0) {
                transform.localScale = Vector3.one;
            } else {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            if (Input.GetMouseButtonDown(0)) {
                weaponController.Attack();
            }
            //var aimPoint = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            var aimPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); //可以直接用Camera.main获取主相机
            weaponController.Aim(aimPoint);
        }

        //物体的移动一般在FixedUpdate中执行
        private void FixedUpdate() {
            rb.MovePosition(rb.position + (moveSpeed * Time.fixedDeltaTime * movement));
        }
    }
}