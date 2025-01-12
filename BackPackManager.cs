using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class BackPackManager: MonoBehaviour
{
    public GameObject backpackUI;       // �������� UI
    public Transform contentArea;       // ���� ScrollView ����������
    public GameObject backpackItemPrefab; // �������Ԥ���壨������ʾģ��ͼ�꣩

    private List<string> collectedModelNames = new List<string>(); // �洢���ռ���ģ������

    public DragAndDropManager dragAndDropManager; //��ȡ��ק����

    public int backpackItemCount; //�������������ռ�����ģ�͵�������

    // �������ԣ����ڶ�ȡ����
    


    // ���ģ�͵�����
    public void AddToBackpack(GameObject model)
    {
        // ��ȡģ�͵�����
        AttachModelAttributes attributes = model.GetComponent<AttachModelAttributes>();
        if (attributes == null)
        {
            Debug.LogError($"ģ�� {model.name} ȱ�� AttachModelAttributes �ű���");
            return;
        }

        // ����Ƿ��Ѿ��ռ�
        if (collectedModelNames.Contains(attributes.modelName))
        {
            Debug.Log($"{attributes.modelName} �Ѿ��ڱ����У�");
            return;
        }

        // ��ӵ��б�
        collectedModelNames.Add(attributes.modelName);

        // ���������� UI
        GameObject newItem = Instantiate(backpackItemPrefab, contentArea);

        // ����ͼ�������
        Image iconImage = newItem.GetComponentInChildren<Image>();
        Text nameText = newItem.GetComponentInChildren<Text>();

        if (iconImage != null) iconImage.sprite = attributes.modelIcon; // ����ͼ��
        if (nameText != null) nameText.text = attributes.modelName;     // ��������

        // ��ӵ���¼�
        newItem.GetComponent<Button>().onClick.AddListener(() => OnBackpackItemClicked(attributes.modelPrefab));
       

        Debug.Log($"{attributes.modelName} ����ӵ�������");

        //���ӱ���������
        backpackItemCount = backpackItemCount + 1;
       
        Debug.Log($"�ռ�����{backpackItemCount} ��ģ��");

    }


    //����������Place Object�¼�
    private void OnBackpackItemClicked(GameObject modelPrefab)
    {
        if (dragAndDropManager != null)
        {
            dragAndDropManager.StartDragging(modelPrefab);
            Debug.Log($"��ʼ��קģ�ͣ�{modelPrefab.name}");
        }
    }

    // ��ʾ�����ر���
    public void ToggleBackpack()
    {
        backpackUI.SetActive(!backpackUI.activeSelf);
    }

}
