using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class DialogueManager : MonoBehaviour
{
    public Text dialogueText; // UI 文本显示对话
    public GameObject dialogueBox; // 对话框对象，控制其显示与隐藏

    private Queue<string> sentences; // 存储当前对话的句子队列
    private string currentSentence; // 当前正在显示的句子
    private bool isTyping; // 标志是否正在逐字显示
    private bool dialogueFinished; // 标志对话是否完全结束
    //private bool IsDialogueActive; //判断对话框显示状态

    private void Start()
    {
        sentences = new Queue<string>();//初始化
        isTyping = false;
        dialogueFinished = false;
    }

    public void StartDialogue(DialogueData dialogueData)
    {
       // IsDialogueActive = true; //对话框显示状态
        dialogueBox.SetActive(true); // 显示对话框
        sentences.Clear(); // 清空之前的句子队列

        foreach (string sentence in dialogueData.sentences)
        {
            sentences.Enqueue(sentence); // 将新句子加入队列
        }

        dialogueFinished = false; // 重置对话结束标志
        DisplayNextSentence(); // 显示第一句对话
    }

    public void DisplayNextSentence()
    {
        // 如果正在显示逐字动画，直接完成当前句子的显示
        if (isTyping)
        {
            StopAllCoroutines(); // 停止逐字动画
            dialogueText.text = currentSentence; // 立即显示完整句子
            isTyping = false;
            return;
        }

        // 如果没有更多句子，标记对话结束，等待玩家点击隐藏
        if (sentences.Count == 0)
        {
            dialogueFinished = true;
            return;
        }

        // 从队列中取出下一句
        currentSentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(currentSentence)); // 开始逐字显示
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true; // 标记为正在逐字显示
        dialogueText.text = ""; // 清空文本框内容

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter; // 逐字拼接
            yield return new WaitForSeconds(0.05f); // 设置逐字显示速度
        }

        isTyping = false; // 逐字显示完成
    }

    private void EndDialogue()
    {
        dialogueBox.SetActive(false); // 隐藏对话框
        dialogueText.text = ""; // 清空文本框
        //IsDialogueActive = false;
    }

    private void Update()
    {
        // 监听触屏或鼠标点击事件
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // 如果对话完成，点击后隐藏对话框
            if (dialogueFinished)
            {
                EndDialogue();
            }
            else
            {
                DisplayNextSentence(); // 显示下一句
            }
        }
    }
}
