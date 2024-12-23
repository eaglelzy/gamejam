using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制背景移动
public class EnvironmentControl : MonoBehaviour
{
    [Tooltip("移动的节点")]
    [SerializeField] private Transform moveNode;

    Transform target;

    private void Awake() {
        target = Camera.main.transform;
    }

    private void Update() {
        
        moveNode.position = new Vector3(target.position.x, target.position.y, moveNode.position.z);
    }
}
