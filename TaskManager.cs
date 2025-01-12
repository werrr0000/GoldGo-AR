using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public BackPackManager backpackManager; // ���ñ���������

    public Text taskText;           // ���������ı���ʾ
    public List<Task> tasks;        // �����б�

    private int currentTaskIndex = 0; // ��ǰ��������

    public GameObject TaskBarUI; //TaskBar UI

    public GameObject animationObject;      // �������ʱ���ŵĶ�������
    public DialogueManager dialogueManager; // ���öԻ�������
    public InteractionManager interactionManager; // ���ý���������

    private bool isTaskComplete = false;    // ��ǰ�����Ƿ����


    void Update()
    {
        // ��⵱ǰ����״̬
        UpdateTaskStatus();
    }

    void UpdateTaskStatus()
    {
        if (currentTaskIndex >= tasks.Count) return;

        Task currentTask = tasks[currentTaskIndex];
        int collectedItems = backpackManager.backpackItemCount;

        if (collectedItems >= currentTask.requiredCount && !isTaskComplete)
        {
            isTaskComplete = true;
            currentTask.isCompleted = true;
            currentTaskIndex++;
            if (currentTaskIndex < tasks.Count)
            {
                taskText.text = tasks[currentTaskIndex].description;
                isTaskComplete = false; //׼��������һ������
            }
            else
            {
                taskText.text = "���ռ���"; // �����������
                StartCoroutine(PlaySequenceAnimation());       // ���Ŷ���
            }
        }
        else
        {
            taskText.text = $"{currentTask.description} ({collectedItems}/{currentTask.requiredCount})";
        }
    }

    
    //��д����������
    void Start()
    {

        tasks = new List<Task>
        {
            new Task { description = "�ſ����˵��㣬ȥ�ռ���һ��ģ��", requiredCount = 1, isCompleted = false },
            new Task { description = "����ȥ�ռ�����ģ��", requiredCount = 2, isCompleted = false }

        };

    }

    //���ƶ�������˳���ڶԻ����뵯��֮��Эͬ��
    private IEnumerator PlaySequenceAnimation()
    {
        // �ȴ��Ի���͵����ر�
        while (dialogueManager.dialogueBox.activeSelf || interactionManager.Collectpopup.activeSelf)
        {
            yield return null;
        }

        // ���Ŷ���
        PlayAnimation();

        // ���趯��ʱ���� 5 ��
        yield return new WaitForSeconds(5f);

        // ������������ʾ
        taskText.text = "������ɣ�";
    }

    //��������
    private void PlayAnimation()
    {
        Animation animation = animationObject.GetComponent<Animation>();
        if (animation != null)
        {
            animation.Play("AnimationClipName"); // �滻Ϊ��Ķ�����������
        }
        else
        {
            Debug.LogError("Animation not found on animationObject!");
        }

    }


    public void ToggleTaskBarUI()
    {
        TaskBarUI.SetActive(!TaskBarUI.activeSelf);
    }

    
}



