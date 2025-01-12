using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class DragAndDropManager : MonoBehaviour
{
    
    //��ȡar���
    private Camera arCamera;
    
    public GameObject placementIndicator; // ����ָʾ����λ�õĿ��ӻ�����
    public ARRaycastManager raycastManager; // AR Raycast Manager
    private GameObject currentDraggedModel; // ��ǰ������ק��ģ��

    private bool isDragging = false; // �Ƿ�������ק
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>(); // ���ڴ洢 Raycast �Ľ��

    private Vector2 lastTouchPosition; // ��һ�εĴ���λ�ã������λ�ã�

    private void Update()
    {
        // ������룺���������
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            // ��ȡ��ǰ�Ĵ���λ�û����λ��
            Vector2 touchPosition = Input.touchCount > 0
                ? Input.GetTouch(0).position
                : (Vector2)Input.mousePosition;

            // ������ק
            HandleDrag(touchPosition);
        }
        else if (isDragging && (Input.touchCount == 0 && !Input.GetMouseButton(0)))
        {
            // ���û��ɿ�����ֹͣ����ʱ����ɷ���
            StopDragging();
        }
    }

    // ��ʼ��ק
    public void StartDragging(GameObject modelPrefab)
    {
        if (isDragging) return;

        isDragging = true;

        // ����ģ��ʵ��
        currentDraggedModel = Instantiate(modelPrefab);

        // ��ѡ����ʼ��λ�ã�������������ǰ����
        currentDraggedModel.transform.position = arCamera.transform.position + arCamera.transform.forward * 0.5f;

        Debug.Log($"��ʼ��קģ�ͣ�{modelPrefab.name}");
    }

    // ֹͣ��ק������ģ��
    public void StopDragging()
    {
        if (!isDragging || currentDraggedModel == null) return;

        isDragging = false;

        // ����ģ��Ϊ�����ռ�
        //currentDraggedModel.tag = "Uncollectible";  // �޸� Tag

        // ��ѡ������ָʾ��
        if (placementIndicator != null)
        {
            placementIndicator.SetActive(false);
        }

        Debug.Log($"{currentDraggedModel.name} �ѷ��õ������У�");
    }

    // ������ק�߼�

   
    
    void Start()
    {

        // ȷ�� raycastManager ����ȷ��ֵ
        raycastManager = GetComponent<ARRaycastManager>();
        if (raycastManager == null)
        {
            Debug.LogError("ARRaycastManager δ��ȷ��ֵ��");
        }

        // ��ȡ AR Camera
        arCamera = Camera.main;
        if (arCamera == null)
        {
            Debug.LogError("û���ҵ� AR Camera��");
        }

    }
    private void HandleDrag(Vector2 touchPosition)
    {


        // ��� raycastManager �Ƿ�Ϊ��
        if (raycastManager == null)
        {
            Debug.LogError("ARRaycastManager δ�ҵ����޷�ִ����ק��");
            return;
        }

        if (placementIndicator == null)
        {
            Debug.LogError("����ָʾ�� (placementIndicator) δ��ֵ��");
            return;
        }

        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            // ����ģ�͵�λ�õ� Raycast ���е�λ��
            if (currentDraggedModel != null) // ȷ�� currentDraggedModel �ѱ���ֵ
            {
                currentDraggedModel.transform.position = hitPose.position;
            }

            // ��ѡ�����·���ָʾ����λ�ú���ת
            placementIndicator.transform.position = hitPose.position;
            placementIndicator.transform.rotation = hitPose.rotation;
            placementIndicator.SetActive(true);

            lastTouchPosition = touchPosition; // ��¼����λ��
        }
    }
}

