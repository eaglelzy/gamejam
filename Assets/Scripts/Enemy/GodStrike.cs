using System.Collections;
using System.Collections.Generic;
using TS.Character;
using UnityEngine;

public class GodStrike : MonoBehaviour
{
    public Vector3 target; // 目标点
    public float speed = 15f; // 移动速度

    public GameObject hitEffect;

    void Update()
    {
        // 持续向目标点移动
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            Health health = other.GetComponent<Health>();
            health.Damage(10, gameObject);
            Instantiate(hitEffect, other.transform.position, Quaternion.identity);
        }
    }
}
