using MoreMountains.Tools;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;


/// <summary>
/// Ô¶³ÌµÐÈËai
/// </summary>
public class RemoteEnemyAI : BaseEnemyAI, MMEventListener<MMStateChangeEvent<EnemyStates.Enemy1State>>
{
    [Tooltip("ÒÆ¶¯ËÙ¶È")]
    [SerializeField]
    private float moveSpeed = 2;

    [Tooltip("¾¯½ä·¶Î§")]
    [SerializeField]
    private float warnRange = 4;

    [Tooltip("¹¥»÷ÀäÈ´Ê±¼ä£¨Ãë£©")]
    [SerializeField]
    private float coldDown = 2;

    private float coldDownTime;

    private void OnEnable()
    {
        Debug.Log("OnEnable");
		MMEventManager.AddListener<MMStateChangeEvent<EnemyStates.Enemy1State>>(this);
        machine.ChangeState(EnemyStates.Enemy1State.Idle);
        FaceRight = true;
        coldDownTime = 0;
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable");
		MMEventManager.RemoveListener(this);
    }

    private void Update()
    {
        if (machine.CurrentState != EnemyStates.Enemy1State.Attack)
        {
            coldDownTime -= Time.deltaTime;
        }
        var distance = (player.transform.position - transform.position).magnitude;
        if (distance < warnRange)
        {
            if (machine.CurrentState != EnemyStates.Enemy1State.Attack && coldDownTime <= 0)
            {
                machine.ChangeState(EnemyStates.Enemy1State.Attack);
            }
        }
        else
        {
            if (machine.CurrentState == EnemyStates.Enemy1State.Idle)
            {
                machine.ChangeState(EnemyStates.Enemy1State.Move);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (machine.CurrentState == EnemyStates.Enemy1State.Move)
        {
            Vector3 targetDir = (player.transform.position - transform.position).normalized;
            rb.velocity = moveSpeed  * targetDir; 
        } 
    }



    public void OnMMEvent(MMStateChangeEvent<EnemyStates.Enemy1State> eventType)
    {
        if (eventType.Target != gameObject)
        {
            return;
        }
        Debug.Log(eventType.NewState);
        switch (eventType.NewState)
        {
            case EnemyStates.Enemy1State.Move:
                CheckFace();
                break;
            case EnemyStates.Enemy1State.Attack:
                CheckFace();
                rb.velocity = Vector2.zero;
                animator.SetBool("Attack", true);
                Invoke(nameof(AttackEnd), 2);
                break;
            case EnemyStates.Enemy1State.Dead:
                animator.SetBool("IsDeath", true);
                Invoke(nameof(Death), 1);
                break;
            case EnemyStates.Enemy1State.IsHit:
                animator.SetTrigger("IsHit");
                Invoke(nameof(IsHit), 1);
                break;
            case EnemyStates.Enemy1State.Idle:
                break;
        }
    }

    private void IsHit()
    {
        machine.ChangeState(EnemyStates.Enemy1State.Idle);
    }

    private void Death()
    {
        animator.SetBool("IsDeath", false);
        gameObject.SetActive(false);
    }
    private void AttackEnd()
    {
        animator.SetBool("Attack", false);
        machine.ChangeState(EnemyStates.Enemy1State.Idle);
        coldDownTime = coldDown;
    }

    IEnumerator PlayAnimation(string name, float seconds, Action action = null)
    {
        yield return new WaitForSeconds(seconds); 
        animator.SetBool(name, false);
        action?.Invoke();
    }

    IEnumerator PlayTrggerAnimation(string name, float seconds, Action action = null)
    {
        animator.SetTrigger(name);
        yield return new WaitForSeconds(seconds); 
        action?.Invoke();
    }
}
