using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f; // 移动速度
    public Rigidbody2D rb; // 角色的 Rigidbody2D 组件
    public Animator animator; // 用于控制动画（可选）

    private Vector2 movement;

    void Update()
    {
        // 获取输入
        movement.x = Input.GetAxisRaw("Horizontal"); // 左右（-1到1）
        movement.y = Input.GetAxisRaw("Vertical");   // 上下（-1到1）

        if(movement.magnitude > 0)
        {
            animator.Play("Samurai_Run");
        }
        else
        {
            animator.Play("Samurai_Idle");
        }

        if(movement.x > 0)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void FixedUpdate()
    {
        // 通过 Rigidbody2D 移动角色
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
