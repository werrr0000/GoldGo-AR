using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class InteractionManager : MonoBehaviour
{
    public GameObject Collectpopup; // ���� UI
   
    private Camera arCamera; //����arcamera

    //public GameObject[] gameObjectCollect;

    public bool isCollected = false; // ���ģ���Ƿ����ռ�

    public BackPackManager backpackManager; // ���ñ���������

    public DialogueManager dialogueManager; // ���öԻ�������
    public List<DialogueData> dialogues;    // �洢���жԻ����б�
    private void Start()
    {
        // ��ȡ AR Camera
        arCamera = Camera.main;
    }

    private void Update()
    {
        // ��ⴥ�����루�����ڴ������豸��
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // ��⵽ģ�ͱ����
                GameObject ClickedObject = hit.transform.gameObject;

                //���ö����tag�Ƿ�Ϊcollectible
                if (ClickedObject.CompareTag("Collectible"))
                {
                    HandleModelClick(ClickedObject);
                }
                
            }
        }
        // ������������������ã�
        else if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject ClickedObject = hit.transform.gameObject;
                //���ö����tag�Ƿ�Ϊcollectible
                if (ClickedObject.CompareTag("Collectible"))
                {
                    HandleModelClick(ClickedObject);
                }
                
            }
        }
    }

    void HandleModelClick(GameObject model)
    {

        // ��� Collectpopup �Ƿ�Ϊ��
        if (Collectpopup == null)
        {
            Debug.LogError("Collectpopup is not assigned in the Inspector or missing in the scene!");
            return;
        }

        // ��ȡ�����е� Text ���
        Text popupText = Collectpopup.GetComponentInChildren<UnityEngine.UI.Text>();
        if (popupText == null)
        {
            Debug.LogError("No Text component found in Collectpopup or its children!");
            return;
        }

        
        
        AttachModelAttributes attributes = model.GetComponent<AttachModelAttributes>();
        if (attributes == null)
        {
            Debug.LogError($"ģ�� {model.name} ȱ�� AttachModelAttributes �ű���");
            return;
        }

        // ���õ����ı�
        popupText.text = $"{attributes.modelName} �ѱ��ռ���";
        //��ʾ����
        Collectpopup.SetActive(true);

        // ����ģ��
        model.SetActive(false);

    


        Debug.Log($"{model.name} �ѱ��ռ�������ʾ������");


        // ���ģ�͵�����
        if (backpackManager != null)
        {
            backpackManager.AddToBackpack(model);
        }
        else
        {
            Debug.LogError("BackpackManager is not assigned!");
        }



        // ������Ӧģ�͵ĶԻ�
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

    




   
