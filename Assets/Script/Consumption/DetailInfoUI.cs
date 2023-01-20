using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class DetailInfoUI : MonoBehaviour
{
    /// <summary>
    /// ���İ��� ����Ǵ� �ӵ�
    /// </summary>
    public float alphaChangeSpeed = 10.0f;

    // ������Ʈ��
    Image itemIcon;
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemValue;
    TextMeshProUGUI itemDesc;
    CanvasGroup canvasGroup;

    /// <summary>
    /// �۵� �Ͻ� ���� Ȯ�ο� ����
    /// </summary>
    bool isPause = false;

    /// <summary>
    /// ��ǥ�� �ϴ� ���İ�
    /// </summary>
    float targetAlpha = 0.0f;


    // ������Ƽ ------------------------------------------------------------------------------------

    /// <summary>
    /// �Ͻ������� Ȯ���ϰ� �����ϴ� ������Ƽ
    /// </summary>
    public bool IsPause
    {
        get => isPause;
        set
        {
            isPause = value;
            if (isPause)     // �Ͻ� ������ �Ǹ� ������ ������ â�� �ݴ´�.
            {
                Close();
            }
        }
    }

    /// <summary>
    /// �����ִ��� Ȯ���ϴ� ������Ƽ
    /// </summary>
    public bool IsOpen => (canvasGroup.alpha > 0.0f);


    // �Լ� ---------------------------------------------------------------------------------------
    private void Awake()
    {
        // ������Ʈ ã��
        itemIcon = transform.GetChild(0).GetComponent<Image>();
        itemName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        itemValue = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        itemDesc = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (targetAlpha > 0)
        {
            // ��ǥ ���İ� 0���� ũ�� => ������ �ִ� ���̴�.
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
        }
        else
        {
            // ��ǥ ���İ� 0���� �۰ų� ����. => ������ �ִ� ���̴�.
            canvasGroup.alpha -= Time.deltaTime * alphaChangeSpeed;
        }
        canvasGroup.alpha = Mathf.Clamp(canvasGroup.alpha, 0, 1);   // �׻� ������ 0~1�� �ǵ��� ����
    }

    /// <summary>
    /// ������ â ����
    /// </summary>
    /// <param name="itemData">������ â�� ������ ������ ������</param>
    public void Open(ItemData itemData)
    {
        if (!isPause && itemData != null)   // �Ͻ� ���� ���°� �ƴϰ� itemData�� ���� ���� ó��
        {
            itemIcon.sprite = itemData.itemIcon;
            itemName.text = itemData.itemName;
            itemValue.text = itemData.value.ToString();
            itemDesc.text = itemData.itemDescription;

            targetAlpha = 1;  // ���İ��� ��� 1�� ���鵵�� ����

            MovePosition(Mouse.current.position.ReadValue());   // ���� �� �׻� ���콺 ��ġ�� �������� ����
        }
    }

    /// <summary>
    /// ������ â �ݱ�
    /// </summary>
    public void Close()
    {
        targetAlpha = 0;    // ���İ��� ��� 0���� ���鵵�� ����
    }

    /// <summary>
    /// ������â�� ��ġ�� �ű�� �Լ�
    /// </summary>
    /// <param name="pos">�� ��ġ</param>
    public void MovePosition(Vector2 pos)
    {
        RectTransform rect = (RectTransform)transform;

        if (pos.x + rect.sizeDelta.x > Screen.width) // ������ â�� ȭ���� ������� Ȯ��
        {
            pos.x -= rect.sizeDelta.x;   // ������â�� ȭ���� �Ѿ�� ������â�� ���� ���̸�ŭ �������� �̵�
        }

        transform.position = pos; // ������ ������â�� �̵� ����
    }
}
