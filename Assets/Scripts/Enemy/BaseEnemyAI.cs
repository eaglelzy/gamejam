using MoreMountains.Tools;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;


/// <summary>
/// 敌人基类ai
/// </summary>
public abstract class BaseEnemyAI : MonoBehaviour
{
    [Tooltip("初始朝向是否朝右")]
    [SerializeField]
    protected bool IsRight = true;

    protected MMStateMachine<EnemyStates.Enemy1State> machine;

    protected GameObject player;

    protected Rigidbody2D rb;

    protected bool faceRight = true;

    protected Animator animator;
    protected bool FaceRight { 
        set {
            if (faceRight != value)
            {
                faceRight = value;

                Vector3 scale = transform.localScale;
                if ((scale.x < 0 && faceRight) || (scale.x > 0 && !faceRight))
                {
                    scale.x *= -1;
                }
                if (!IsRight)
                {
                    scale.x *= -1;

                }
                transform.localScale = scale;
            }
        } 
    }
    protected void CheckFace()
    {
        Vector3 targetDir = (player.transform.position - transform.position).normalized;
        FaceRight = targetDir.x > 0;
    }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        machine = new MMStateMachine<EnemyStates.Enemy1State>(gameObject, true); 
        machine.ChangeState(EnemyStates.Enemy1State.Idle);
        player = EnemyManager.Instance.player;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        FaceRight = true;
    }
}
