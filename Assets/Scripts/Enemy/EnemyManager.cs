using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using System;
using MoreMountains.Feedbacks;

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

    private float gameTimer = 0f;
    private float timer = 0f;
    private readonly int intervalTime = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameTimer += Time.deltaTime;
        timer += Time.deltaTime;
        if (timer > intervalTime)
        {
            StartCoroutine(SpawnEnemy());
            timer = 0f;
        }
    }

    /// <summary>
    /// 生成敌人
    /// </summary>
    private IEnumerator SpawnEnemy()
    {
        // 获取屏幕边缘的世界坐标
        Camera camera = Camera.main;
        Vector3 topLeft = camera.ScreenToWorldPoint(new Vector3(0, Screen.height, camera.nearClipPlane));
        Vector3 topRight = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camera.nearClipPlane));
        //Vector3 bottomLeft = camera.ScreenToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        int time = (int)gameTimer;
        for (int i = 0; i < enemyConfigList.Count; i++)
        {
            var config = enemyConfigList[i];
            if ((time % config.spawnIntervalTime == 0))
            {
                float x = UnityEngine.Random.Range(topLeft.x, topRight.x);
                if (time > config.spawnStartTime && (time < config.spawnEndTime || config.spawnEndTime == -1))
                {
                    var enemy = ((MMMultipleObjectPooler)MMObjectPooler.Instance).GetPooledGameObjectOfType(config.enemyPrefab.name);
                    if (enemy != null)
                    {
                        enemy.transform.localPosition = new Vector3(x, topLeft.y, 0);
                        enemy.gameObject.SetActive(true);
                        yield return new WaitForSeconds(0.3f);
                    }
                }
            }
        }
    }
}
