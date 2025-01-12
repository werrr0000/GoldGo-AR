using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class DialogueManager : MonoBehaviour
{
    public Text dialogueText; // UI �ı���ʾ�Ի�
    public GameObject dialogueBox; // �Ի�����󣬿�������ʾ������

    private Queue<string> sentences; // �洢��ǰ�Ի��ľ��Ӷ���
    private string currentSentence; // ��ǰ������ʾ�ľ���
    private bool isTyping; // ��־�Ƿ�����������ʾ
    private bool dialogueFinished; // ��־�Ի��Ƿ���ȫ����
    //private bool IsDialogueActive; //�ж϶Ի�����ʾ״̬

    private void Start()
    {
        sentences = new Queue<string>();//��ʼ��
        isTyping = false;
        dialogueFinished = false;
    }

    public void StartDialogue(DialogueData dialogueData)
    {
       // IsDialogueActive = true; //�Ի�����ʾ״̬
        dialogueBox.SetActive(true); // ��ʾ�Ի���
        sentences.Clear(); // ���֮ǰ�ľ��Ӷ���

        foreach (string sentence in dialogueData.sentences)
        {
            sentences.Enqueue(sentence); // ���¾��Ӽ������
        }

        dialogueFinished = false; // ���öԻ�������־
        DisplayNextSentence(); // ��ʾ��һ��Ի�
    }

    public void DisplayNextSentence()
    {
        // ���������ʾ���ֶ�����ֱ����ɵ�ǰ���ӵ���ʾ
        if (isTyping)
        {
            StopAllCoroutines(); // ֹͣ���ֶ���
            dialogueText.text = currentSentence; // ������ʾ��������
            isTyping = false;
            return;
        }

        // ���û�и�����ӣ���ǶԻ��������ȴ���ҵ������
        if (sentences.Count == 0)
        {
            dialogueFinished = true;
            return;
        }

        // �Ӷ�����ȡ����һ��
        currentSentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(currentSentence)); // ��ʼ������ʾ
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true; // ���Ϊ����������ʾ
        dialogueText.text = ""; // ����ı�������

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter; // ����ƴ��
            yield return new WaitForSeconds(0.05f); // ����������ʾ�ٶ�
        }

        isTyping = false; // ������ʾ���
    }

    private void EndDialogue()
    {
        dialogueBox.SetActive(false); // ���ضԻ���
        dialogueText.text = ""; // ����ı���
        //IsDialogueActive = false;
    }

    private void Update()
    {
        // ����������������¼�
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // ����Ի���ɣ���������ضԻ���
            if (dialogueFinished)
            {
                EndDialogue();
            }
            else
            {
                DisplayNextSentence(); // ��ʾ��һ��
            }
        }
    }
}
