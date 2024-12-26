using System.Collections;
using System.Collections.Generic;
using TS.Character;
using UnityEngine;

//终结之洞
public class EndHole : MonoBehaviour
{
    Transform target;

    private float normalSpeed = 1f;

    private float maxSpeed = 10f;

    private void Start() {
        target = LevelManager.Instance.rock.transform;
    }

    private void Update() {
        float speed = normalSpeed;

        // 如果距离大于30
        if(Vector3.Distance(target.position, transform.position) > 35f){
            speed = maxSpeed;
        }

        Vector3 direction = (target.position - transform.position).normalized;

        // 计算新的位置
        Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;

        // 更新物体位置
        transform.position = newPosition;

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            Health health = other.GetComponent<Health>();
            health.Damage(health.CurrentHealth, gameObject);
        }
    }

}
