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


    //获取 ARTrackedImageManager的referance
    private void Awake()
    {
        _arTrackedImageManager = GetComponent<ARTrackedImageManager>();
        _arObjects = new Dictionary<string, GameObject>();
    }

    private void Start()
    {

        //监听 TrackedImageManager事件的更改
        _arTrackedImageManager.trackedImagesChanged += OnTrackedImageChanged;
        //给每个图像生成gameobject并隐藏
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
            // 检查图像是否已经实例化
            if (!_arObjects.ContainsKey(trackedImage.referenceImage.name))
            {
                // 如果图像对象还没被创建，实例化并加入字典
                GameObject newObject = Instantiate(prefabsToSpawn[0]); // 选择预制件
                _arObjects.Add(trackedImage.referenceImage.name, newObject);
            }
            else
            {
                // 如果已经存在，确保对象处于启用状态
                _arObjects[trackedImage.referenceImage.name].SetActive(true);
            }

            // 更新位置和其他状态
            UpdateTrackedImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            // 更新已跟踪的图像的状态和位置
            UpdateTrackedImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            // 如果图像被移除，禁用对象并从字典中移除
            if (_arObjects.ContainsKey(trackedImage.referenceImage.name))
            {
                _arObjects[trackedImage.referenceImage.name].SetActive(false);
                _arObjects.Remove(trackedImage.referenceImage.name); // 从字典中删除
            }
        }




        //识别图像的变换
       foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            //展示，隐藏或者变换图像上的gameobjec
            UpdateTrackedImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            //展示，隐藏或者变换图像上的gameobjec
            UpdateTrackedImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            //展示，隐藏或者变换图像上的gameobjec
            _arObjects[trackedImage.referenceImage.name].gameObject.SetActive(false);
        }

    }

    private void UpdateTrackedImage(ARTrackedImage trackedImage)
    {
        //检查图像状态

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
        //展示，隐藏或者变换图像上的gameobjec
    }




}
