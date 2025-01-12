using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class InteractionManager : MonoBehaviour
{
    public GameObject Collectpopup; // 弹窗 UI
   
    private Camera arCamera; //定义arcamera

    //public GameObject[] gameObjectCollect;

    public bool isCollected = false; // 标记模型是否已收集

    public BackPackManager backpackManager; // 引用背包管理器

    public DialogueManager dialogueManager; // 引用对话管理器
    public List<DialogueData> dialogues;    // 存储所有对话的列表
    private void Start()
    {
        // 获取 AR Camera
        arCamera = Camera.main;
    }

    private void Update()
    {
        // 检测触摸输入（适用于触摸屏设备）
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 检测到模型被点击
                GameObject ClickedObject = hit.transform.gameObject;

                //检查该对象的tag是否为collectible
                if (ClickedObject.CompareTag("Collectible"))
                {
                    HandleModelClick(ClickedObject);
                }
                
            }
        }
        // 适用于鼠标点击（调试用）
        else if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject ClickedObject = hit.transform.gameObject;
                //检查该对象的tag是否为collectible
                if (ClickedObject.CompareTag("Collectible"))
                {
                    HandleModelClick(ClickedObject);
                }
                
            }
        }
    }

    void HandleModelClick(GameObject model)
    {

        // 检查 Collectpopup 是否为空
        if (Collectpopup == null)
        {
            Debug.LogError("Collectpopup is not assigned in the Inspector or missing in the scene!");
            return;
        }

        // 获取弹窗中的 Text 组件
        Text popupText = Collectpopup.GetComponentInChildren<UnityEngine.UI.Text>();
        if (popupText == null)
        {
            Debug.LogError("No Text component found in Collectpopup or its children!");
            return;
        }

        
        
        AttachModelAttributes attributes = model.GetComponent<AttachModelAttributes>();
        if (attributes == null)
        {
            Debug.LogError($"模型 {model.name} 缺少 AttachModelAttributes 脚本！");
            return;
        }

        // 设置弹窗文本
        popupText.text = $"{attributes.modelName} 已被收集！";
        //显示弹窗
        Collectpopup.SetActive(true);

        // 隐藏模型
        model.SetActive(false);

    


        Debug.Log($"{model.name} 已被收集，并显示弹窗！");


        // 添加模型到背包
        if (backpackManager != null)
        {
            backpackManager.AddToBackpack(model);
        }
        else
        {
            Debug.LogError("BackpackManager is not assigned!");
        }



        // 检索对应模型的对话
        foreach (DialogueData data in dialogues)
        {
            if (data.modelName == model.name)
            {
                dialogueManager.StartDialogue(data);
                break;
            }
        }


    }
}

    




   
