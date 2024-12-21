using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject roadPrefab;

    //需要生成路的标志，相机越过该标志，生成路
    Transform loadRoadMark;

    private Transform currentRoad;

    //当前路要生成的位置
    public Vector3 CurrentAngle = new Vector3(0, 0, 12);

    private Vector3 roadOffset = new Vector3(5, 5, 0);
    
    //初始化loadRoadMark
    void Init() {
        currentRoad = transform.GetChild(0);
        loadRoadMark = currentRoad.Find("loadRoadMark");   
    }

    void GenerateRoad() {
        float roadWidth = currentRoad.GetComponent<SpriteRenderer>().bounds.size.x;
        float y = roadWidth * Mathf.Sin(CurrentAngle.z * Mathf.Deg2Rad);
        float x = roadWidth * Mathf.Cos(CurrentAngle.z * Mathf.Deg2Rad);
        Vector3 targetPosition = currentRoad.position + new Vector3(x, y, 0);
        Quaternion quaternion = Quaternion.Euler(CurrentAngle);

        GameObject road = Instantiate(roadPrefab, targetPosition, quaternion);

        loadRoadMark = road.transform.Find("loadRoadMark");
    }

    private void Start() {
        Init();
    }

    private void Update() {
        if (loadRoadMark != null && Camera.main.transform.position.x > loadRoadMark.position.x) {
            GenerateRoad();
        }
    }
}
