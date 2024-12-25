using UnityEngine;

public class UIFollowTarget : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;  // 需要跟随的游戏物体
    public Vector3 offset = Vector3.zero; // 偏移量

    [Header("UI Settings")]
    public RectTransform uiElement; // 需要移动的 UI 元素
    public Canvas canvas; // UI 所在的 Canvas

    private Camera mainCamera;

    void Start()
    {
        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }

        if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera != null)
        {
            mainCamera = canvas.worldCamera;
        }
        else
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (target == null || uiElement == null) return;

        // 将世界坐标转换为屏幕坐标
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position + offset);

        // 如果目标在摄像机前方
        if (screenPos.z > 0)
        {
            // 将屏幕坐标转换为 UI 坐标
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, 
                screenPos, 
                canvas.renderMode == RenderMode.ScreenSpaceCamera ? mainCamera : null, 
                out Vector2 localPos
            );

            uiElement.localPosition = localPos;
        }
        else
        {
            // 如果目标在摄像机后方，可以隐藏 UI 或处理其他逻辑
            uiElement.gameObject.SetActive(false);
        }
    }
}
