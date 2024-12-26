using UnityEngine;


/// <summary>
/// ���˻���ai
/// </summary>
public abstract class BaseEnemyAI : MonoBehaviour
{
    [Tooltip("��ʼ�����Ƿ���")]
    [SerializeField]
    protected bool IsRight = true;

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
        player = EnemyManager.Instance.player;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        FaceRight = true;
    }
}
