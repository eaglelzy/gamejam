using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] [Tooltip("生成路预制体列表")]
    private List<GameObject> roadPrefabList;

    [SerializeField]
    private List<GameObject> itemPrefabList;

    [SerializeField]
    private List<GameObject> platformPrefabList;

    //需要生成路的标志，相机越过该标志，生成路
    Transform loadRoadMark;

    private Transform currentRoad;

    //当前路要生成的位置
    private Vector3 currentAngle = new Vector3(0, 0, 12);

    private Vector3 roadOffset = new Vector3(5, 5, 0);

    //生成的路list
    private List<GameObject> roadList = new List<GameObject>();

    public int RoadCount {
        get {
            return roadList.Count;
        }
    }

    #region 参数
    int roadCountPerStage = 4;


    #endregion
    
    //初始化loadRoadMark
    void Init() {
        currentRoad = transform.GetChild(0);
        loadRoadMark = currentRoad.Find("loadRoadMark");
        roadList.Add(currentRoad.gameObject);
    }

    void GenerateRoad() {
        float roadWidth = currentRoad.GetComponent<SpriteRenderer>().sprite.bounds.size.x * 3;
        float y = roadWidth * Mathf.Sin(currentAngle.z * Mathf.Deg2Rad);
        float x = roadWidth * Mathf.Cos(currentAngle.z * Mathf.Deg2Rad);
        Vector3 targetPosition = currentRoad.position + new Vector3(x, y, 0);
        Quaternion quaternion = Quaternion.Euler(currentAngle);

        //若干层后，换一种路
        int index = RoadCount / roadCountPerStage;
        if(index >= roadPrefabList.Count) index = roadPrefabList.Count - 1;
        GameObject roadPrefab = roadPrefabList[index];


        currentRoad = Instantiate(roadPrefab, targetPosition, quaternion).transform;
        roadList.Add(currentRoad.gameObject);

        loadRoadMark = currentRoad.transform.Find("loadRoadMark");

        //增加角度
        currentAngle += new Vector3(0, 0, .13f);

        LevelManager.Instance.UpdateHeightText();

        GeneratePlatform();
        //GenerateItems();
    }

    private void Start() {
        Init();
    }

    private void Update() {
        if (loadRoadMark != null && Camera.main.transform.position.x > loadRoadMark.position.x) {
            GenerateRoad();
        }
    }

    //每个阶段生成个道具, 在道具列表里随机选一个
    private void GenerateItems(){
        int index = Random.Range(0, itemPrefabList.Count);
        GameObject itemPrefab = itemPrefabList[index];
        Vector3 position = currentRoad.position + new Vector3(3f, 4.1f, 0);
        Instantiate(itemPrefab, position, Quaternion.identity);
    }

    private void GenerateItems(Vector3 position){
        //if(itemPrefabList.Count == 0) return;
        int index = Random.Range(0, itemPrefabList.Count);
        //int index = itemPrefabList.Count - 1;
        GameObject itemPrefab = itemPrefabList[index];
        //itemPrefabList.RemoveAt(index);

        Instantiate(itemPrefab, position, Quaternion.identity);
    }

    //生成平台
    private void GeneratePlatform(){
        int index = Random.Range(0, platformPrefabList.Count);
        GameObject platformPrefab = platformPrefabList[index];
        //位置随机
        float y = Random.Range(10, 20) / 10f + 3.5f;

        Vector3 position = currentRoad.position + new Vector3(0, y, 0);
        Instantiate(platformPrefab, position, Quaternion.identity);

        GenerateItems(position+new Vector3(0, 0.5f, 0));
    }
}
