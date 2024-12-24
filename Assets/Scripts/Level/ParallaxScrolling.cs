using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    public Vector2 parallaxEffectMultiplier; // 视差效果倍率，分别控制 X 和 Y 轴
    private Vector3 lastCameraPosition; // 上一帧摄像机的位置

    private float rate = 0.1f;

    void Start()
    {
        // 初始化摄像机位置
        lastCameraPosition = Camera.main.transform.position;
    }

    void LateUpdate()
    {
        // 计算摄像机的移动距离
        Vector3 cameraDelta = Camera.main.transform.position - lastCameraPosition;

        // 根据视差倍率调整背景位置
        transform.position += new Vector3(cameraDelta.x * parallaxEffectMultiplier.x * rate, cameraDelta.y * parallaxEffectMultiplier.y * rate, 0);

        // 更新上一帧摄像机位置
        lastCameraPosition = Camera.main.transform.position;
    }
}
