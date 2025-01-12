using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class BackPackManager: MonoBehaviour
{
    public GameObject backpackUI;       // 背包的主 UI
    public Transform contentArea;       // 背包 ScrollView 的内容区域
    public GameObject backpackItemPrefab; // 背包项的预制体（用于显示模型图标）

    private List<string> collectedModelNames = new List<string>(); // 存储已收集的模型名称

    public DragAndDropManager dragAndDropManager; //获取拖拽功能

    public int backpackItemCount; //背包项数量（收集到的模型的数量）

    // 公共属性，用于读取变量
    


    // 添加模型到背包
    public void AddToBackpack(GameObject model)
    {
        // 获取模型的属性
        AttachModelAttributes attributes = model.GetComponent<AttachModelAttributes>();
        if (attributes == null)
        {
            Debug.LogError($"模型 {model.name} 缺少 AttachModelAttributes 脚本！");
            return;
        }

        // 检查是否已经收集
        if (collectedModelNames.Contains(attributes.modelName))
        {
            Debug.Log($"{attributes.modelName} 已经在背包中！");
            return;
        }

        // 添加到列表
        collectedModelNames.Add(attributes.modelName);

        // 创建背包项 UI
        GameObject newItem = Instantiate(backpackItemPrefab, contentArea);

        // 设置图标和名称
        Image iconImage = newItem.GetComponentInChildren<Image>();
        Text nameText = newItem.GetComponentInChildren<Text>();

        if (iconImage != null) iconImage.sprite = attributes.modelIcon; // 设置图标
        if (nameText != null) nameText.text = attributes.modelName;     // 设置名称

        // 添加点击事件
        newItem.GetComponent<Button>().onClick.AddListener(() => OnBackpackItemClicked(attributes.modelPrefab));
       

        Debug.Log($"{attributes.modelName} 已添加到背包！");

        //增加背包项数量
        backpackItemCount = backpackItemCount + 1;
       
        Debug.Log($"收集到了{backpackItemCount} 个模型");

    }


    //点击背包项唤醒Place Object事件
    private void OnBackpackItemClicked(GameObject modelPrefab)
    {
        if (dragAndDropManager != null)
        {
            dragAndDropManager.StartDragging(modelPrefab);
            Debug.Log($"开始拖拽模型：{modelPrefab.name}");
        }
    }

    // 显示或隐藏背包
    public void ToggleBackpack()
    {
        backpackUI.SetActive(!backpackUI.activeSelf);
    }

}
