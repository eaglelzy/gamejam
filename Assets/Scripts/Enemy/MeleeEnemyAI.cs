using MoreMountains.Tools;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;


/// <summary>
/// ��ս����ai
/// </summary>
public class MeleeEnemyAI : BaseEnemyAI, MMEventListener<MMStateChangeEvent<EnemyStates.Enemy1State>>
{
    [Tooltip("�ƶ��ٶ�")]
    [SerializeField]
    private float moveSpeed = 2;

    [Tooltip("���䷶Χ")]
    [SerializeField]
    private float warnRange = 3;

    [Tooltip("�����ƶ��ٶ�")]
    [SerializeField]
    private float attackSpeed = 3;

    [Tooltip("������ȴ(��)")]
    [SerializeField]
    private float attackDistance = 2;

    private void OnEnable()
    {
		MMEventManager.AddListener(this);
        machine.ChangeState(EnemyStates.Enemy1State.Idle);
        FaceRight = true;
    }

    private void OnDisable()
    {
		MMEventManager.RemoveListener(this);
    }

    private void Update()
    {
        var distance = (player.transform.position - transform.position).magnitude;
        if (distance < attackDistance)
        {
            if (machine.CurrentState != EnemyStates.Enemy1State.Attack)
            {
                machine.ChangeState(EnemyStates.Enemy1State.Attack);
            }
        }
        else
        {
            if (machine.CurrentState != EnemyStates.Enemy1State.Move)
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
                break;
            case EnemyStates.Enemy1State.Dead:
                gameObject.SetActive(false);
                break;
        }
    }
}
