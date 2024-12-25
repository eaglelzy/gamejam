using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using System;
using MoreMountains.Feedbacks;
using UnityEditor.SceneTemplate;

/// <summary>
/// ���˹���
/// </summary>
public class EnemyManager : MMSingleton<EnemyManager>
{
    [Tooltip("��������")]
    [SerializeField]
    private List<EnemyConfig> enemyConfigList = new();

    [Tooltip("���")]
    [SerializeField]
    public GameObject player;

    [Tooltip("��ʼ���ɵ���ʱ��")]
    [SerializeField]
    private float startTime = 3f;

    [Tooltip("���ɵ��˼��")]
    [SerializeField]
    private float intervalTime = 5f;

    [Tooltip("ÿ��������������Ϸ�Ѷ�")]
    [SerializeField]
    private int hardTime = 10;

    [Tooltip("������Ϸ�Ѷ�rate")]
    [SerializeField]
    private float hardRate = 0.8f;

    [Tooltip("��С���ʱ��")]
    [SerializeField]
    private float minIntervalTime = 0.6f;

    private float gameTime = 0f;
    private bool flag = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private void Update()
    {
        gameTime += Time.deltaTime;
        int v = (int)(gameTime - startTime) % hardTime;
        if (v == 0 && !flag)
        {
            if (intervalTime > minIntervalTime)
            {
                intervalTime *= hardRate;
            }
            flag = true;
        } else if (v == 1)
        {
            flag = false;
        }
    }

    /// <summary>
    /// ���ɵ���
    /// </summary>
    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(startTime);
        Debug.Log("SpawnEnemy..");
        while (true)
        {
            yield return new WaitForSeconds(intervalTime);
            // ��ȡ��Ļ��Ե����������
            Camera camera = Camera.main;
            Vector3 topLeft = camera.ScreenToWorldPoint(new Vector3(0, Screen.height, camera.nearClipPlane));
            Vector3 topRight = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camera.nearClipPlane));
            EnemyConfig config = RandomEnemy();
            var enemy = ((MMMultipleObjectPooler)MMObjectPooler.Instance).GetPooledGameObjectOfType(config.enemyPrefab.name);
            if (enemy != null)
            {
                float x = UnityEngine.Random.Range(topLeft.x, topRight.x);
                enemy.transform.localPosition = new Vector3(x, topLeft.y, 0);
                enemy.gameObject.SetActive(true);
            }

        }
    }

    private EnemyConfig RandomEnemy()
    {
        float total = 0f;
        for (int i = 0; i < enemyConfigList.Count; i++)
        {
            total += enemyConfigList[i].rate;
        }
        float value = UnityEngine.Random.Range(0, total);
        float rate = 0f;
        int index = 0;
        for (int i = 0; i < enemyConfigList.Count; i++)
        {
            rate += enemyConfigList[i].rate;
            if (value > rate)
            {
                index = i + 1; 
                break;
            }
        }
        if (index > enemyConfigList.Count)
        {
            index--;
        }
        return enemyConfigList[index];
    }
}
