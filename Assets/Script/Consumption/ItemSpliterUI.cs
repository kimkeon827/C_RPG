using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class ItemSpliterUI : MonoBehaviour, IScrollHandler
{
    /// <summary>
    /// �������� �и��� �ּ� ����
    /// </summary>
    const int itemCountMin = 1;
    /// <summary>
    /// �и��� ����
    /// </summary>
    uint itemSplitCount = itemCountMin;

    /// <summary>
    /// �������� �и��� ����
    /// </summary>
    C_ItemSlot targetSlot;

    /// <summary>
    /// �и��� ���� �Է��� ���� ��ǲ �ʵ�
    /// </summary>
    TMP_InputField inputField;

    /// <summary>
    /// �и��� ���� �Է��� ���� �����̴�
    /// </summary>
    //Slider slider;

    /// <summary>
    /// �и��� ������ ������
    /// </summary>
    Image itemImage;

    /// <summary>
    /// �и��� ���� ���� �� Ȯ���� ���� ������Ƽ
    /// </summary>
    uint ItemSplitCount
    {
        get => itemSplitCount;
        set
        {
            if (itemSplitCount != value)   // �и��� ������ ������ ���� ��
            {
                itemSplitCount = value;
                itemSplitCount = (uint)Mathf.Max(1, itemSplitCount);     // �ּҰ��� 1           

                if (targetSlot != null)
                {
                    itemSplitCount = (uint)Mathf.Min(itemSplitCount, targetSlot.ItemCount - 1); // �ִ밪�� ���Կ� ����ִ� ���� - 1
                }

                // ������ �и� ������ ��ǲ�ʵ�� �����̴��� ǥ��
                inputField.text = itemSplitCount.ToString();
                //slider.value = itemSplitCount;
            }
        }
    }

    /// <summary>
    /// OK ��ư�� ������ �� ����� ��������Ʈ
    /// </summary>
    public Action<uint, uint> onOKClick;

    private void Awake()
    {
        // ���� �ʱ�ȭ

        // ��ǲ�ʵ� ������Ʈ ã��
        inputField = GetComponentInChildren<TMP_InputField>();
        // ��ǲ�ʵ��� ���� ����� �� ����� ���� ItemSplitCount�� ����
        inputField.onValueChanged.AddListener((text) => ItemSplitCount = uint.Parse(text));

        /*// �����̴� ������Ʈ ã��
        slider = GetComponentInChildren<Slider>();
        // �����̴��ǰ��� ����� �� ����� ���� ItemSplitCount�� ����
        //slider.onValueChanged.AddListener(ChangeSliderValue); // �Ϲ� �Լ��� ����ϴ� ���
        slider.onValueChanged.AddListener((value) => ItemSplitCount = (uint)Mathf.RoundToInt(value));   // ���� �Լ��� ����ϴ� ���*/

        // ������ư ������Ʈ ã��
        Button increase = transform.GetChild(1).GetComponent<Button>();
        // ������ư�� ������ �� ���� ItemSplitCount 1�� ����
        increase.onClick.AddListener(() => ItemSplitCount++);
        // ���ҹ�ư ������Ʈ ã��
        Button decrease = transform.GetChild(2).GetComponent<Button>();
        // ���ҹ�ư�� ������ �� ���� ItemSplitCount 1�� ����
        decrease.onClick.AddListener(() => ItemSplitCount--);

        // OK ��ư�� �������� InventoryUI�� �˸��� ������ �и�â �ݱ�
        Button ok = transform.GetChild(3).GetComponent<Button>();
        ok.onClick.AddListener(() =>
        {
            onOKClick?.Invoke(targetSlot.Index, ItemSplitCount);    // � ���Կ��� ��� �������� �ű��� �˷��ֱ�
            Close();                                                // ������ �и�â �ݱ�
        });

        // ĵ�� ��ư�� �������� ������ �и�â �ݱ�
        Button cancel = transform.GetChild(4).GetComponent<Button>();
        cancel.onClick.AddListener(() => Close());

        // ������ �������� ǥ���� �̹��� ������Ʈ ã��
        itemImage = transform.GetChild(5).GetComponent<Image>();
    }

    //private void ChangeSliderValue(float value)   // AddListener�� �Ϲ� �Լ� ����ϴ� ���� ���� ����
    //{
    //    ItemSplitCount = (uint)Mathf.RoundToInt(value);
    //}

    private void Start()
    {
        Close();    // ������ �� �ݰ� �����ϱ�
    }

    /// <summary>
    /// �����ۺи�â�� ���� �Լ�
    /// </summary>
    /// <param name="target">�������� �и��� ����</param>
    public void Open(C_ItemSlotUI target)
    {
        targetSlot = target.ItemSlot;       // ���� ��������        

        ItemSplitCount = 1;                 // ������ �и����� �ʱ�ȭ

        itemImage.sprite = targetSlot.ItemData.itemIcon;    // ������ ����

        //slider.minValue = itemCountMin;     // �ִ� �ּҰ� ����
        //slider.maxValue = targetSlot.ItemCount - 1;

        this.gameObject.SetActive(true);    // ������ Ȱ��ȭ�ؼ� �����ֱ�
    }

    /// <summary>
    /// �����ۺи�â�� �ݴ� �Լ�
    /// </summary>
    public void Close()
    {
        this.gameObject.SetActive(false);   // ��Ȱ��ȭ�ؼ� �Ⱥ��̰� �����
    }

    /// <summary>
    /// Ư�� ��ũ����ǥ�� ������ �и��� �ȿ� �ִ��� �ۿ� �ִ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="screenPos">Ȯ���� ��ũ�� ��ǥ</param>
    /// <returns>true�� ��ũ����ǥ�� ������ �и��� ���̴�. false�� ���̴�.</returns>
    private bool IsAreaInside(Vector2 screenPos)
    {
        RectTransform rectTransform = (RectTransform)transform;
        float halfWidth = rectTransform.rect.width * 0.5f;

        // ������ �и��� ������ ���� �Ʒ�(min)�� ��������(max)�� ����ϱ�. ������ �и����� pivot�� �Ʒ��� ����� �־ �̷��� ���
        Vector2 min = new Vector2(rectTransform.position.x - halfWidth, rectTransform.position.y);
        Vector2 max = new Vector2(rectTransform.position.x + halfWidth, rectTransform.position.y + rectTransform.rect.height);

        // ����� ��ġ�� min���ٴ� ũ�� max���ٴ� ���� ���� true.
        return min.x < screenPos.x && screenPos.x < max.x && min.y < screenPos.y && screenPos.y < max.y;
    }

    /// <summary>
    /// ���콺�� Ŭ���Ǿ��� �� ����� �Լ�
    /// </summary>
    /// <param name="context"></param>
    public void OnMouseClick(InputAction.CallbackContext context)
    {
        // �����ִ� ������ ���� ó��
        if (gameObject.activeSelf)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue(); // ���콺�� ��ġ(��ũ����ǥ)�� ����
            if (!IsAreaInside(screenPos))    // ���콺�� ��ġ�� ������ �и��� �ȿ� �ִ��� �ۿ� �ִ��� Ȯ��
            {
                Close();                    // �ۿ� ������ ������ �и��⸦ �ݴ´�.
            }
        }
    }

    /// <summary>
    /// ���콺 ���� �������� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="eventData"></param>
    public void OnScroll(PointerEventData eventData)
    {
        // eventData.scrollDelta; // ���콺 �� ������ ������ �� �ִ�.        
        if (eventData.scrollDelta.y > 0)
        {
            ItemSplitCount++;   // �������� ���� �ø��� ���� ����
        }
        else
        {
            ItemSplitCount--;   // �Ʒ������� ���� ������ ���� ����
        }
    }
}
