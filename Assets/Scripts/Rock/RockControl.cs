using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockControl : MonoBehaviour
{

    [Tooltip("初始下降速度")]
    [SerializeField]
    private float force = 10;


    private Rigidbody2D rb;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(1, 1) * force;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
/*        if (collision.gameObject.CompareTag("Bullet"))
        {
            rb.AddForce(scrollDirection * bulletForce, ForceMode2D.Impulse);
            if (rb.velocity.x > maxSpeed)
            {
                rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            }
        }*/
    }
}
