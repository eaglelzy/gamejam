using TS.Weapon;
using UnityEngine;

namespace TS.Player
{

    public class PlayerControl : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public Rigidbody2D rb;
        public Animator animator;
        public WeaponController weaponController;
        //public Camera playerCamera;
        private Vector2 movement;

        public float jumpForce = 10f;
        public LayerMask groundLayer; // 用于检测地面
        public Transform groundCheck; // 检测是否在地面上的点
        public float groundCheckRadius = 0.2f;
        private bool isGrounded;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            weaponController = GetComponent<WeaponController>();
        }

        private void Update()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            movement.x = Input.GetAxisRaw("Horizontal");
            //movement.y = Input.GetAxisRaw("Vertical");   

            // if(movement.magnitude > 0) { animator.Play("Samurai_Run"); } else {
            // animator.Play("Samurai_Idle"); }

            if (movement.x > 0)
            {
                transform.localScale = Vector3.one;
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            if (Input.GetMouseButtonDown(0) && weaponController != null)
            {
                weaponController.Attack();
            }
            // 跳跃
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            //var aimPoint = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            var aimPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); //可以直接用Camera.main获取主相机
            weaponController?.Aim(aimPoint);

            //只用x轴的速度
            rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
        }

        //物体的移动一般在FixedUpdate中执行
        private void FixedUpdate()
        {
            //rb.MovePosition(rb.position + (moveSpeed * Time.fixedDeltaTime * movement));
        }

        void OnDrawGizmosSelected()
        {
            // 可视化地面检测范围
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}