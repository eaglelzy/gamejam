using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiledBackgroundScroll : MonoBehaviour
{
    public float speed = 2f; // �����ٶ�
    public float tileSizeX = 10f; // �����Ŀ��

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * speed, tileSizeX); // ѭ��λ��
        transform.position = startPosition + Vector3.left * newPosition;
    }
}