using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f; // �ƶ��ٶ�
    public Rigidbody2D rb; // ��ɫ�� Rigidbody2D ���
    public Animator animator; // ���ڿ��ƶ�������ѡ��

    private Vector2 movement;

    void Update()
    {
        // ��ȡ����
        movement.x = Input.GetAxisRaw("Horizontal"); // ���ң�-1��1��
        movement.y = Input.GetAxisRaw("Vertical");   // ���£�-1��1��

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
        // ͨ�� Rigidbody2D �ƶ���ɫ
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
