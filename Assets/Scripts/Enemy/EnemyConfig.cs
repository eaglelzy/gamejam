using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
[System.Serializable]
public class EnemyConfig
{
    [Tooltip("����Ԥ�Ƽ�")]
    [SerializeField]
    public GameObject enemyPrefab;

    [Tooltip("�೤ʱ�������(��λ����)")]
    [SerializeField]
    public int spawnStartTime;

    [Tooltip("�೤ʱ����ڳ���(��λ����), -1����һֱ����")]
    [SerializeField]
    public int spawnEndTime = -1;

    [Tooltip("ÿ������������")]
    [SerializeField]
    public int spawnIntervalTime;
}
