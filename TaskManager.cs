using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public BackPackManager backpackManager; // 引用背包管理器

    public Text taskText;           // 任务栏的文本显示
    public List<Task> tasks;        // 任务列表

    private int currentTaskIndex = 0; // 当前任务索引

    public GameObject TaskBarUI; //TaskBar UI

    public GameObject animationObject;      // 任务完成时播放的动画对象
    public DialogueManager dialogueManager; // 引用对话管理器
    public InteractionManager interactionManager; // 引用交互管理器

    private bool isTaskComplete = false;    // 当前任务是否完成


    void Update()
    {
        // 检测当前任务状态
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
                isTaskComplete = false; //准备进入下一个任务
            }
            else
            {
                taskText.text = "已收集完"; // 所有任务完成
                StartCoroutine(PlaySequenceAnimation());       // 播放动画
            }
        }
        else
        {
            taskText.text = $"{currentTask.description} ({collectedItems}/{currentTask.requiredCount})";
        }
    }

    
    //编写任务栏引导
    void Start()
    {

        tasks = new List<Task>
        {
            new Task { description = "门口有人等你，去收集第一个模型", requiredCount = 1, isCompleted = false },
            new Task { description = "现在去收集更多模型", requiredCount = 2, isCompleted = false }

        };

    }

    //控制动画播放顺序在对话框与弹窗之后（协同）
    private IEnumerator PlaySequenceAnimation()
    {
        // 等待对话框和弹窗关闭
        while (dialogueManager.dialogueBox.activeSelf || interactionManager.Collectpopup.activeSelf)
        {
            yield return null;
        }

        // 播放动画
        PlayAnimation();

        // 假设动画时长是 5 秒
        yield return new WaitForSeconds(5f);

        // 更新任务栏显示
        taskText.text = "任务完成！";
    }

    //动画播放
    private void PlayAnimation()
    {
        Animation animation = animationObject.GetComponent<Animation>();
        if (animation != null)
        {
            animation.Play("AnimationClipName"); // 替换为你的动画剪辑名称
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



