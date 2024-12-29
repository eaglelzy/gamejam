using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人配置
/// </summary>
[System.Serializable]
public class EnemyConfig
{
    [Tooltip("敌人预制件")]
    [SerializeField]
    public GameObject enemyPrefab;

    [Tooltip("出现概率")]
    [SerializeField]
    public float rate = 1f;

    [Tooltip("出现倍率调整")]
    [SerializeField]
    public float rateRate = 1.1f;

    //[Tooltip("多长时间后会出现(单位：秒)")]
    //[SerializeField]
    //public int spawnStartTime;

    //[Tooltip("多长时间后不在出现(单位：秒), -1代表一直出现")]
    //[SerializeField]
    //public int spawnEndTime = -1;

    //[Tooltip("每个多少秒生成")]
    //[SerializeField]
    //public float spawnIntervalTime;
}
