using TS.Weapon;
using UnityEngine;

namespace TS.Player {

    public class PlayerControl : MonoBehaviour {
        public float moveSpeed = 5f; // �ƶ��ٶ�
        public Rigidbody2D rb; // ��ɫ�� Rigidbody2D ���
        public Animator animator; // ���ڿ��ƶ�������ѡ��
        public WeaponController weaponController;
        public Camera playerCamera;
        private Vector2 movement;

        private void Update() {
            // ��ȡ����
            movement.x = Input.GetAxisRaw("Horizontal"); // ���ң�-1��1��
            movement.y = Input.GetAxisRaw("Vertical");   // ���£�-1��1��

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
            var aimPoint = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            weaponController.Aim(aimPoint);
        }

        private void FixedUpdate() {
            // ͨ�� Rigidbody2D �ƶ���ɫ
            rb.MovePosition(rb.position + (moveSpeed * Time.fixedDeltaTime * movement));
        }
    }
}