using MoreMountains.Tools;
using Unity.VisualScripting;
using UnityEngine;


public class Enemy1AI : MonoBehaviour, MMEventListener<MMStateChangeEvent<EnemyStates.Enemy1State>>
{
    [Tooltip("ÒÆ¶¯ËÙ¶È")]
    [SerializeField]
    private float moveSpeed = 2;

    [Tooltip("¹¥»÷¾àÀë")]
    [SerializeField]
    private float attackDistance = 4;

    private MMStateMachine<EnemyStates.Enemy1State> machine;

    private GameObject player;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        machine = new MMStateMachine<EnemyStates.Enemy1State>(gameObject, true); 
        machine.ChangeState(EnemyStates.Enemy1State.Normal);
        player = EnemyManager.Instance.player;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
		MMEventManager.AddListener(this);
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
            var targetDir = (player.transform.position - transform.position).normalized;
            rb.velocity = moveSpeed  * targetDir; 
        } 
    }

    public void OnMMEvent(MMStateChangeEvent<EnemyStates.Enemy1State> eventType)
    {
        switch(eventType.NewState)
        {
            case EnemyStates.Enemy1State.Move:
                break;
            case EnemyStates.Enemy1State.Attack:
                rb.velocity = Vector2.zero;
                break;
            case EnemyStates.Enemy1State.Dead:
                gameObject.SetActive(false);
                break;
        }
    }
}
