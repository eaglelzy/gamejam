using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using System;
using MoreMountains.Feedbacks;
using UnityEditor.SceneTemplate;

/// <summary>
/// 敌人管理
/// </summary>
public class EnemyManager : MMSingleton<EnemyManager>
{
    [Tooltip("敌人配置")]
    [SerializeField]
    private List<EnemyConfig> enemyConfigList = new();

    [Tooltip("玩家")]
    [SerializeField]
    public GameObject player;

    [Tooltip("开始生成敌人时间")]
    [SerializeField]
    private float startTime = 3f;

    [Tooltip("生成敌人间隔")]
    [SerializeField]
    private float intervalTime = 5f;

    [Tooltip("每隔多少秒增加游戏难度")]
    [SerializeField]
    private int hardTime = 10;

    [Tooltip("增加游戏难度rate")]
    [SerializeField]
    private float hardRate = 0.8f;

    [Tooltip("最小间隔时间")]
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
    /// 生成敌人
    /// </summary>
    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(startTime);
        Debug.Log("SpawnEnemy..");
        while (true)
        {
            yield return new WaitForSeconds(intervalTime);
            // 获取屏幕边缘的世界坐标
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
