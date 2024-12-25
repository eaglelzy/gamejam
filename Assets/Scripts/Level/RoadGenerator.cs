using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] [Tooltip("生成路预制体列表")]
    private List<GameObject> roadPrefabList;

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
    
    //初始化loadRoadMark
    void Init() {
        currentRoad = transform.GetChild(0);
        loadRoadMark = currentRoad.Find("loadRoadMark");
        roadList.Add(currentRoad.gameObject);
    }

    void GenerateRoad() {
        float roadWidth = currentRoad.GetComponent<SpriteRenderer>().bounds.size.x;
        float y = roadWidth * Mathf.Sin(currentAngle.z * Mathf.Deg2Rad);
        float x = roadWidth * Mathf.Cos(currentAngle.z * Mathf.Deg2Rad);
        Vector3 targetPosition = currentRoad.position + new Vector3(x, y, 0);
        Quaternion quaternion = Quaternion.Euler(currentAngle);

        //若干层后，换一种路
        int index = RoadCount / 4;
        if(index >= roadPrefabList.Count) index = roadPrefabList.Count - 1;
        GameObject roadPrefab = roadPrefabList[index];


        currentRoad = Instantiate(roadPrefab, targetPosition, quaternion).transform;
        roadList.Add(currentRoad.gameObject);

        loadRoadMark = currentRoad.transform.Find("loadRoadMark");

        //增加角度
        currentAngle += new Vector3(0, 0, .1f);

        LevelManager.Instance.UpdateHeightText();
    }

    private void Start() {
        Init();
    }

    private void Update() {
        if (loadRoadMark != null && Camera.main.transform.position.x > loadRoadMark.position.x) {
            GenerateRoad();
        }
    }

    private void GenerateItems(){

    }
}
