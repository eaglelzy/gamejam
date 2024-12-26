using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockControl : MonoBehaviour {

    [SerializeField]
    private float force = 10;

    private Rigidbody2D rb;


    public float MovePower { get; private set; } = 10; 

    private float decreaseRate = 2f; // 减少速率

    // Start is called before the first frame update
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        //rb.velocity = new Vector2(1, 1) * force;
    }

    //石头被打
    public void PowerUp(float power) {
        MovePower += power;
    }

    private void Update() {
        if(MovePower > 0){
            rb.velocity = new Vector3(3, 0, 0);
            MovePower -= decreaseRate * Time.deltaTime; // 每秒减少
            MovePower = Mathf.Max(MovePower, 0); // 防止变成负数
        }
    }
}