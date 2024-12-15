using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiledBackgroundScroll : MonoBehaviour
{
    public float speed = 2f; // 滚动速度
    public float tileSizeX = 10f; // 背景的宽度

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * speed, tileSizeX); // 循环位置
        transform.position = startPosition + Vector3.left * newPosition;
    }
}