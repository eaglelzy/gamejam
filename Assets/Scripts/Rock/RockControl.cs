using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockControl : MonoBehaviour
{
    [Tooltip("碰撞给的力大小")]
    [SerializeField]
    private int bulletForce = 500;

    [Tooltip("石头最大速度")]
    [SerializeField]
    private int maxSpeed = 2;

    private Vector2 scrollDirection = new(1, 0.2f);
    

    Rigidbody2D rb;
    public float force = 10;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(1, 1) * force;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            rb.AddForce(scrollDirection * bulletForce, ForceMode2D.Impulse);
            if (rb.velocity.x > maxSpeed)
            {
                rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            }
        }
    }
}
