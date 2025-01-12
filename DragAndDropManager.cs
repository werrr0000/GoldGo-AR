using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class DragAndDropManager : MonoBehaviour
{
    
    //获取ar相机
    private Camera arCamera;
    
    public GameObject placementIndicator; // 用于指示放置位置的可视化对象
    public ARRaycastManager raycastManager; // AR Raycast Manager
    private GameObject currentDraggedModel; // 当前正在拖拽的模型

    private bool isDragging = false; // 是否正在拖拽
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>(); // 用于存储 Raycast 的结果

    private Vector2 lastTouchPosition; // 上一次的触摸位置（或鼠标位置）

    private void Update()
    {
        // 检测输入：触屏或鼠标
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            // 获取当前的触摸位置或鼠标位置
            Vector2 touchPosition = Input.touchCount > 0
                ? Input.GetTouch(0).position
                : (Vector2)Input.mousePosition;

            // 处理拖拽
            HandleDrag(touchPosition);
        }
        else if (isDragging && (Input.touchCount == 0 && !Input.GetMouseButton(0)))
        {
            // 当用户松开鼠标或停止触屏时，完成放置
            StopDragging();
        }
    }

    // 开始拖拽
    public void StartDragging(GameObject modelPrefab)
    {
        if (isDragging) return;

        isDragging = true;

        // 创建模型实例
        currentDraggedModel = Instantiate(modelPrefab);

        // 可选：初始化位置（如放置在摄像机前方）
        currentDraggedModel.transform.position = arCamera.transform.position + arCamera.transform.forward * 0.5f;

        Debug.Log($"开始拖拽模型：{modelPrefab.name}");
    }

    // 停止拖拽并放置模型
    public void StopDragging()
    {
        if (!isDragging || currentDraggedModel == null) return;

        isDragging = false;

        // 设置模型为不可收集
        //currentDraggedModel.tag = "Uncollectible";  // 修改 Tag

        // 可选：隐藏指示器
        if (placementIndicator != null)
        {
            placementIndicator.SetActive(false);
        }

        Debug.Log($"{currentDraggedModel.name} 已放置到场景中！");
    }

    // 处理拖拽逻辑

   
    
    void Start()
    {

        // 确保 raycastManager 被正确赋值
        raycastManager = GetComponent<ARRaycastManager>();
        if (raycastManager == null)
        {
            Debug.LogError("ARRaycastManager 未正确赋值！");
        }

        // 获取 AR Camera
        arCamera = Camera.main;
        if (arCamera == null)
        {
            Debug.LogError("没有找到 AR Camera！");
        }

    }
    private void HandleDrag(Vector2 touchPosition)
    {


        // 检查 raycastManager 是否为空
        if (raycastManager == null)
        {
            Debug.LogError("ARRaycastManager 未找到，无法执行拖拽！");
            return;
        }

        if (placementIndicator == null)
        {
            Debug.LogError("放置指示器 (placementIndicator) 未赋值！");
            return;
        }

        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            // 更新模型的位置到 Raycast 命中的位置
            if (currentDraggedModel != null) // 确保 currentDraggedModel 已被赋值
            {
                currentDraggedModel.transform.position = hitPose.position;
            }

            // 可选：更新放置指示器的位置和旋转
            placementIndicator.transform.position = hitPose.position;
            placementIndicator.transform.rotation = hitPose.rotation;
            placementIndicator.SetActive(true);

            lastTouchPosition = touchPosition; // 记录触摸位置
        }
    }
}

