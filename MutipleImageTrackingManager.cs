using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MutipleImageTrackingManager : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabsToSpawn;

    private ARTrackedImageManager _arTrackedImageManager;

    private Dictionary<string, GameObject> _arObjects;


    //��ȡ ARTrackedImageManager��referance
    private void Awake()
    {
        _arTrackedImageManager = GetComponent<ARTrackedImageManager>();
        _arObjects = new Dictionary<string, GameObject>();
    }

    private void Start()
    {

        //���� TrackedImageManager�¼��ĸ���
        _arTrackedImageManager.trackedImagesChanged += OnTrackedImageChanged;
        //��ÿ��ͼ������gameobject������
        foreach (GameObject prefab in prefabsToSpawn)
        {
            GameObject newARObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newARObject.name = prefab.name;
            newARObject.gameObject.SetActive(false);
            _arObjects.Add(newARObject.name, newARObject);



        }

    }

    private GameObject Instantiate(object zero, Quaternion identity)
    {
        throw new NotImplementedException();
    }

    private void OnDestroy()
    {
        _arTrackedImageManager.trackedImagesChanged += OnTrackedImageChanged;
    }

    private void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {

        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            // ���ͼ���Ƿ��Ѿ�ʵ����
            if (!_arObjects.ContainsKey(trackedImage.referenceImage.name))
            {
                // ���ͼ�����û��������ʵ�����������ֵ�
                GameObject newObject = Instantiate(prefabsToSpawn[0]); // ѡ��Ԥ�Ƽ�
                _arObjects.Add(trackedImage.referenceImage.name, newObject);
            }
            else
            {
                // ����Ѿ����ڣ�ȷ������������״̬
                _arObjects[trackedImage.referenceImage.name].SetActive(true);
            }

            // ����λ�ú�����״̬
            UpdateTrackedImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            // �����Ѹ��ٵ�ͼ���״̬��λ��
            UpdateTrackedImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            // ���ͼ���Ƴ������ö��󲢴��ֵ����Ƴ�
            if (_arObjects.ContainsKey(trackedImage.referenceImage.name))
            {
                _arObjects[trackedImage.referenceImage.name].SetActive(false);
                _arObjects.Remove(trackedImage.referenceImage.name); // ���ֵ���ɾ��
            }
        }




        //ʶ��ͼ��ı任
       foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            //չʾ�����ػ��߱任ͼ���ϵ�gameobjec
            UpdateTrackedImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            //չʾ�����ػ��߱任ͼ���ϵ�gameobjec
            UpdateTrackedImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            //չʾ�����ػ��߱任ͼ���ϵ�gameobjec
            _arObjects[trackedImage.referenceImage.name].gameObject.SetActive(false);
        }

    }

    private void UpdateTrackedImage(ARTrackedImage trackedImage)
    {
        //���ͼ��״̬

        if (trackedImage.trackingState is TrackingState.Limited or TrackingState.None)
        {
            _arObjects[trackedImage.referenceImage.name].gameObject.SetActive(false);
            return;
        }

        if (prefabsToSpawn != null)
        {
            _arObjects[trackedImage.referenceImage.name].gameObject.SetActive(true);
            _arObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
        }
        //չʾ�����ػ��߱任ͼ���ϵ�gameobjec
    }




}
