using TS.Weapon;
using UnityEngine;

namespace TS.Player {

    public class PlayerControl : MonoBehaviour {
        public float moveSpeed = 5f;
        public Rigidbody2D rb;
        public Animator animator;
        public WeaponController weaponController;
        public float jumpForce = 10f;

        public LayerMask groundLayer;

        // 用于检测地面
        public Transform groundCheck;

        // 检测是否在地面上的点
        public float groundCheckRadius = 0.2f;

        public float moveSmoothTime = 0.05f;

        //public Camera playerCamera;
        private Vector2 movement;

        private bool isGrounded;
        private Vector3 _currentVelocity = Vector3.zero;

        private void Start() {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            weaponController = GetComponentInChildren<WeaponController>();
        }

        private void Update() {
            //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            //movement.x = Input.GetAxisRaw("Horizontal");
            //movement.y = Input.GetAxisRaw("Vertical");

            // if(movement.magnitude > 0) { animator.Play("Samurai_Run"); } else {
            // animator.Play("Samurai_Idle"); }

            // if (movement.x > 0) {
            //     transform.localScale = Vector3.one;
            // } else {
            //     transform.localScale = new Vector3(-1, 1, 1);
            // }

            if (Input.GetMouseButtonDown(0)) {
                weaponController.Attack();
            }
            // 跳跃
            // if (Input.GetButtonDown("Jump") && isGrounded) {
            //     rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            // }
            //var aimPoint = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            var aimPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); //可以直接用Camera.main获取主相机
            weaponController.Aim(aimPoint);
        }

        //物体的移动一般在FixedUpdate中执行
        // private void FixedUpdate() {
        //     var targetVelocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
        //     //只用x轴的速度
        //     rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref _currentVelocity, moveSmoothTime);
        // }

    }
}