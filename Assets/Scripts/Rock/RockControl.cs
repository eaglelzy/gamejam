using System.Collections;
using System.Collections.Generic;
using TS.Character;
using UnityEngine;

public class RockControl : MonoBehaviour {

    [SerializeField]
    private float force = 10;

    private Rigidbody2D rb;

    [SerializeField]
    private GameObject rockHitEffect;

    public float MovePower { get; private set; } = 0; 

    private float decreaseRate = 2.5f; // 减少速率

    // Start is called before the first frame update
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        //rb.velocity = new Vector2(1, 1) * force;
    }

    //石头被打
    public void PowerUp(float power) {
        if (MovePower >= 100) return;
        MovePower += power;
    }

    private void Update() {
        if(MovePower > 0){
            rb.velocity = new Vector3(3, 0, 0);
            MovePower -= (decreaseRate + LevelManager.Instance.Height/10) * Time.deltaTime; // 每秒减少
            MovePower = Mathf.Max(MovePower, 0); // 防止变成负数
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            Health health = other.gameObject.GetComponent<Health>();
            health.Damage(2, gameObject);
            Rigidbody2D rigidbody2D = other.gameObject.GetComponent<Rigidbody2D>();
            Vector3 dir = other.transform.position - transform.position;
            rigidbody2D.AddForce(dir.normalized * 12, ForceMode2D.Impulse);
            Instantiate(rockHitEffect, other.transform.position, Quaternion.identity);
        }
    }
}