using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockControl : MonoBehaviour
{
    Rigidbody2D rb;
    public float force = 10;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(1, 1) * force;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
