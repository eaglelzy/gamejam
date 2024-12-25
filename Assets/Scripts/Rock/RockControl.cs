using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockControl : MonoBehaviour {

    [Tooltip("��ʼ�½��ٶ�")]
    [SerializeField]
    private float force = 10;

    private Rigidbody2D rb;

    private float movePower = 0; 

    // Start is called before the first frame update
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(1, 1) * force;
    }

    //石头被打
    public void Hit(){
        
    }
}