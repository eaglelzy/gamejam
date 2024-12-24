using MoreMountains.Tools;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;


/// <summary>
/// 近战敌人ai
/// </summary>
public class MeleeEnemyAI : BaseEnemyAI, MMEventListener<MMStateChangeEvent<EnemyStates.Enemy1State>>
{
    [Tooltip("移动速度")]
    [SerializeField]
    private float moveSpeed = 2;

    [Tooltip("警戒范围")]
    [SerializeField]
    private float warnRange = 3;

    [Tooltip("攻击移动速度")]
    [SerializeField]
    private float attackSpeed = 3;

    [Tooltip("攻击冷却(秒)")]
    [SerializeField]
    private float attackDistance = 2;

    private float speed = 2;


    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
		MMEventManager.AddListener(this);
        machine.ChangeState(EnemyStates.Enemy1State.Idle);
        FaceRight = true;
        speed = moveSpeed;
    }

    private void OnDisable()
    {
		MMEventManager.RemoveListener(this);
    }

    private void Update()
    {
        var distance = (player.transform.position - transform.position).magnitude;
        if (distance < warnRange)
        {
            if (machine.CurrentState != EnemyStates.Enemy1State.Attack)
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
        switch(eventType.NewState)
        {
            case EnemyStates.Enemy1State.Move:
                CheckFace();
                break;
            case EnemyStates.Enemy1State.Attack:
                CheckFace();
                rb.velocity = Vector2.zero;
                //animator.SetBool("Attack", true);
                //StartCoroutine(PlayAnimation(1));
                //animator.SetBool("Attack", false);
                machine.ChangeState(EnemyStates.Enemy1State.Idle);
                break;
            case EnemyStates.Enemy1State.Dead:
                //animator.SetBool("IsDeath", true);
                //StartCoroutine(PlayAnimation(1));
                //animator.SetBool("IsDeath", false);
                gameObject.SetActive(false);
                break;
            case EnemyStates.Enemy1State.IsHit:
                //animator.SetTrigger("IsHit");
                //StartCoroutine(PlayAnimation(1));
                machine.ChangeState(EnemyStates.Enemy1State.Idle);
                break;
            case EnemyStates.Enemy1State.Idle:
                break;
        }
    }

    IEnumerator PlayAnimation(float seconds)
    {
        yield return new WaitForSeconds(seconds); 
    }


}
