using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 记录进度信息
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField]
    private RoadGenerator roadGenerator;

    public RockControl rock;

    [Tooltip("当前高度")]
    public float Height { get; private set; }

    [SerializeField]
    private TextMeshProUGUI heightText;

    [SerializeField]
    private GodStrike strikeEffect;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        StartCoroutine(TriggerRandomEvent());
    }

    public void UpdateHeightText() {
        Height = roadGenerator.RoadCount;
        
        heightText.text = Height.ToString()+"00 m";
    }

    private void GodStrike()
    {

        float left = rock.transform.position.x - 10;
        float right = rock.transform.position.x;

        Vector3 pos = new Vector3(Random.Range(left, right), rock.transform.position.y + 10, 0);
        strikeEffect.transform.position = pos;
        strikeEffect.target = pos - new Vector3(0, 20, 0);
    }


    private IEnumerator TriggerRandomEvent()
    {
        while (true) // 持续运行
        {
            // 随机等待时间
            float waitTime = Random.Range(3, 10);

            // 等待
            yield return new WaitForSeconds(waitTime);

            // 触发事件
            GodStrike();
        }
    }
}
