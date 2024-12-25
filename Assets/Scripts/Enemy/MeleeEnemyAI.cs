using MoreMountains.Tools;
using System.Collections;
using TS.Character;
using TS.Player;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;


/// <summary>
/// 近战敌人ai
/// </summary>
public class MeleeEnemyAI : BaseEnemyAI, MMEventListener<MMStateChangeEvent<EnemyStates.MeleeEnemyState>>
{
    [Tooltip("移动速度")]
    [SerializeField]
    private float moveSpeed = 4;

    [Tooltip("警戒范围")]
    [SerializeField]
    private float warnRange = 4;

    [Tooltip("攻击移动速度")]
    [SerializeField]
    private float attackSpeed = 5;

    [Tooltip("攻击冷却(秒)")]
    [SerializeField]
    private float coldDown = 1;

    [Tooltip("攻击力")]
    [SerializeField]
    private float damage = 20;

    private float speed = 2;
    private float coldDownTime;

    private MMStateMachine<EnemyStates.MeleeEnemyState> machine;

    protected override void Awake()
    {
        base.Awake();
        machine = new MMStateMachine<EnemyStates.MeleeEnemyState>(gameObject, true);
    }

    private void OnEnable()
    {
		MMEventManager.AddListener(this);
        machine.ChangeState(EnemyStates.MeleeEnemyState.Idle);
        FaceRight = true;
        speed = moveSpeed;
        coldDownTime = 0;
    }

    private void OnDisable()
    {
		MMEventManager.RemoveListener(this);
    }

    private void Update()
    {
        if (machine.CurrentState != EnemyStates.MeleeEnemyState.Attack)
        {
            coldDownTime -= Time.deltaTime;
        }
        var distance = (player.transform.position - transform.position).magnitude;
        if (distance < warnRange)
        {
            // 达到攻击距离
            if (distance < 0.1 && coldDownTime <= 0)
            {
                if (machine.CurrentState != EnemyStates.MeleeEnemyState.Attack)
                {
                    machine.ChangeState(EnemyStates.MeleeEnemyState.Attack);
                }
            } else if (machine.CurrentState != EnemyStates.MeleeEnemyState.Accelerate && coldDownTime <= 0)
            {
                speed = attackSpeed;
                machine.ChangeState(EnemyStates.MeleeEnemyState.Accelerate);
            }
        }
        else
        {
            if (machine.CurrentState == EnemyStates.MeleeEnemyState.Idle && coldDownTime <= 0)
            {
                machine.ChangeState(EnemyStates.MeleeEnemyState.Move);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (machine.CurrentState == EnemyStates.MeleeEnemyState.Move 
            || machine.CurrentState == EnemyStates.MeleeEnemyState.Accelerate)
        {
            Vector3 targetDir = (player.transform.position - transform.position).normalized;
            rb.velocity = speed  * targetDir; 
        } 
    }



    public void OnMMEvent(MMStateChangeEvent<EnemyStates.MeleeEnemyState> eventType)
    {
        if (eventType.Target != gameObject)
        {
            return;
        }
        switch(eventType.NewState)
        {
            case EnemyStates.MeleeEnemyState.Move:
                CheckFace();
                break;
            case EnemyStates.MeleeEnemyState.Accelerate:
                CheckFace();
                break;
            case EnemyStates.MeleeEnemyState.Attack:
                CheckFace();
                rb.velocity = Vector2.zero;
                coldDownTime = coldDown;
                Invoke(nameof(AttackEnd), 1f);
                break;
            case EnemyStates.MeleeEnemyState.Dead:
                gameObject.SetActive(false);
                break;
            case EnemyStates.MeleeEnemyState.IsHit:
                Invoke(nameof(IsHit), 1);
                break;
            case EnemyStates.MeleeEnemyState.Idle:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var other = collision.gameObject;
        if (other.CompareTag("Player")) {
            if (coldDownTime > 0)
            {
                return;
            }
            var mat = other.GetComponent<SpriteRenderer>().material;
            other.GetComponent<PlayerControl>().StartBlink(mat);
            Health health = other.GetComponent<Health>();
            health.Damage(damage, gameObject);
            //Debug.Log("玩家遭受" + gameObject.name + "攻击，受到 " + damage  + " 点伤害");
        }
    }

    private void IsHit()
    {
        machine.ChangeState(EnemyStates.MeleeEnemyState.Idle);
    }

    private void AttackEnd()
    {
        machine.ChangeState(EnemyStates.MeleeEnemyState.Idle);
    }
}
