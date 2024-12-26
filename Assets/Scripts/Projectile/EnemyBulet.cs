using System.Collections;
using System.Collections.Generic;
using TS.Character;
using TS.Player;
using UnityEngine;

public class EnemyBulletData : MonoBehaviour
{
    // 施放者
    public GameObject caster;
    public WeaponData weaponData;

    [Tooltip("爆炸特效")]
    [SerializeField]
    private GameObject explodeEffect;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 3)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            //var effect = Instantiate(explodeEffect, transform.position, Quaternion.identity);
            //Destroy(effect, 0.5f);
            var mat = other.GetComponent<SpriteRenderer>().material;
            other.GetComponent<PlayerControl>().StartBlink(mat);
            Health health = other.GetComponent<Health>();
            health.Damage(weaponData.damage, gameObject);
            //Debug.Log("玩家遭受" + gameObject.name + "攻击，受到 " + weaponData.damage  + " 点伤害");
            Destroy(gameObject);
        }
    }

}
