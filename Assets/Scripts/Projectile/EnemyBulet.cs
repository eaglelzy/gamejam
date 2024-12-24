using System.Collections;
using System.Collections.Generic;
using TS.Player;
using UnityEngine;

public class EnemyBulletData : MonoBehaviour
{
    // ʩ����
    public GameObject caster;
    public WeaponData weaponData;

    [Tooltip("��ը��Ч")]
    [SerializeField]
    private GameObject explodeEffect;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            //var effect = Instantiate(explodeEffect, transform.position, Quaternion.identity);
            //Destroy(effect, 0.5f);
            var mat = other.GetComponent<SpriteRenderer>().material;
            other.GetComponent<PlayerControl>().StartBlink(mat);
            Destroy(gameObject);
        } else
        {
            Destroy(gameObject, 2);
        }
    }

}
