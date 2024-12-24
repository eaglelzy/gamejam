using MoreMountains.Tools;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;


/// <summary>
/// ‘∂≥Ãµ–»Àai
/// </summary>
public class RemoteEnemyAI : BaseEnemyAI, MMEventListener<MMStateChangeEvent<EnemyStates.RemoteEnemyState>>
{
    [Tooltip("“∆∂ØÀŸ∂»")]
    [SerializeField]
    private float moveSpeed = 2;

    [Tooltip("æØΩ‰∑∂Œß")]
    [SerializeField]
    private float warnRange = 4;

    [Tooltip("π•ª˜¿‰»¥ ±º‰£®√Î£©")]
    [SerializeField]
    private float coldDown = 2;

    [Tooltip("Œ‰∆˜")]
    [SerializeField]
    private WeaponData weaponData;

    [Tooltip("Œ‰∆˜∑¢…‰µ„")]
    [SerializeField]
    private Transform firePoint;

    private float coldDownTime;

    private MMStateMachine<EnemyStates.RemoteEnemyState> machine;

    protected override void Awake()
    {
        base.Awake();
        machine = new MMStateMachine<EnemyStates.RemoteEnemyState>(gameObject, true);
    }

    private void OnEnable()
    {
		MMEventManager.AddListener<MMStateChangeEvent<EnemyStates.RemoteEnemyState>>(this);
        machine.ChangeState(EnemyStates.RemoteEnemyState.Idle);
        FaceRight = true;
        coldDownTime = 0;
    }

    private void OnDisable()
    {
		MMEventManager.RemoveListener(this);
    }

    private void Update()
    {
        if (machine.CurrentState != EnemyStates.RemoteEnemyState.Attack)
        {
            coldDownTime -= Time.deltaTime;
        }
        var distance = (player.transform.position - transform.position).magnitude;
        if (distance < warnRange)
        {
            if (machine.CurrentState != EnemyStates.RemoteEnemyState.Attack && coldDownTime <= 0)
            {
                machine.ChangeState(EnemyStates.RemoteEnemyState.Attack);
            }
        }
        else
        {
            if (machine.CurrentState == EnemyStates.RemoteEnemyState.Idle)
            {
                machine.ChangeState(EnemyStates.RemoteEnemyState.Move);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (machine.CurrentState == EnemyStates.RemoteEnemyState.Move)
        {
            Vector3 targetDir = (player.transform.position - transform.position).normalized;
            rb.velocity = moveSpeed  * targetDir; 
        } 
    }



    public void OnMMEvent(MMStateChangeEvent<EnemyStates.RemoteEnemyState> eventType)
    {
        if (eventType.Target != gameObject)
        {
            return;
        }
        switch (eventType.NewState)
        {
            case EnemyStates.RemoteEnemyState.Move:
                CheckFace();
                break;
            case EnemyStates.RemoteEnemyState.Attack:
                CheckFace();
                rb.velocity = Vector2.zero;
                animator.SetBool("Attack", true);
                Invoke(nameof(AttackEnd), 1.2f);
                break;
            case EnemyStates.RemoteEnemyState.Dead:
                animator.SetBool("IsDeath", true);
                Invoke(nameof(Death), 1);
                break;
            case EnemyStates.RemoteEnemyState.IsHit:
                animator.SetTrigger("IsHit");
                Invoke(nameof(IsHit), 1);
                break;
            case EnemyStates.RemoteEnemyState.Idle:
                break;
        }
    }

    private void IsHit()
    {
        machine.ChangeState(EnemyStates.RemoteEnemyState.Idle);
    }

    private void Death()
    {
        animator.SetBool("IsDeath", false);
        gameObject.SetActive(false);
    }
    private void AttackEnd()
    {
        animator.SetBool("Attack", false);
        GameObject bullet = Instantiate(weaponData.bulletPrefab, firePoint.position, firePoint.rotation);
        var bulletData = bullet.GetComponent<EnemyBulletData>();
        bulletData.weaponData = weaponData;
        bulletData.caster = gameObject;
        var dir = player.transform.position - bullet.transform.position;
        bullet.transform.right = dir;
        var rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(dir * weaponData.bulletSpeed * rb.mass, ForceMode2D.Impulse);

        machine.ChangeState(EnemyStates.RemoteEnemyState.Idle);
        coldDownTime = coldDown;
    }

}
