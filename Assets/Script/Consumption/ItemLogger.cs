using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class ItemLogger : MonoBehaviour
{
    public Color warningColor;      
    public Color criticalColor;     

    /// <summary>
    /// �ѹ��� ��µǴ� �ִ� �� ��
    /// </summary>
    public int maxLineCount = 20;

    /// <summary>
    /// �α�â�� ��µ� ��� ���ڿ���.
    /// </summary>
    List<string> logLines;

    /// <summary>
    /// ���ڿ��� ��ġ�� ���� StringBuilder�� �ν��Ͻ�
    /// </summary>
    StringBuilder builder;

    /// <summary>
    /// ���� ��¿� UI
    /// </summary>
    TextMeshProUGUI log;

    private void Awake()
    {
        log = GetComponentInChildren<TextMeshProUGUI>();    // ������Ʈ ã�ƿ���

        logLines = new List<string>(maxLineCount + 5);      // ������ ����ؼ� 5���� ������ �߰�
        builder = new StringBuilder(logLines.Capacity);     // �ƹ��� Ŀ���� logLines ũ�⸦ �Ѿ�� �ʱ� ������ logLines.Capacity ũ��� ����
    }

    private void Start()
    {
        Clear();    // ������ �� ��� ����
    }

    /// <summary>
    /// �ΰſ� ������ �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="logStr">�߰��� ����</param>
    public void Log(string logStr)
    {


        logLines.Add(logStr);               // ����Ʈ�� ���� �߰��ϰ�
        if (logLines.Count > maxLineCount)   // �ִ� �� ���� �Ѿ��
        {
            logLines.RemoveAt(0);           // ù��° �� �����ϱ�
        }

        builder.Clear();                    // ���� Ŭ����
        foreach (var line in logLines)       // ������ ����Ʈ�� ����ִ� ��� ���� �߰�
        {
            builder.AppendLine(line);
        }

        log.text = builder.ToString();      // ������ �ִ� ������ �ϳ��� ���ڿ��� ��ġ��

    }

    /// <summary>
    /// ������ Ŭ����� �Լ�
    /// </summary>
    public void Clear()
    {
        log.text = "";
        logLines.Clear();
        builder.Clear();
    }

    public IEnumerator FadeTextToZero()  // ���İ� 1���� 0���� ��ȯ
    {
        log.color = new Color(log.color.r, log.color.g, log.color.b, 1);
        while (log.color.a > 0.0f)
        {
            log.color = new Color(log.color.r, log.color.g, log.color.b, log.color.a - (Time.deltaTime / 1.2f));
            yield return null;
        }
    }
}
