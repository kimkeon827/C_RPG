using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.InputSystem;

public class C_ItemSlotUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    // ���� ---------------------------------------------------------------------------------------
    private uint id;    // ���° �����ΰ�?

    protected C_ItemSlot itemSlot;    // �� UI�� ����� ItemSlot

    private Image itemImage;
    private TextMeshProUGUI itemCountText;

    // ������Ƽ ------------------------------------------------------------------------------------

    public uint ID => id;
    public C_ItemSlot ItemSlot => itemSlot;

    // ��������Ʈ ----------------------------------------------------------------------------------
    public Action<uint> onDragStart;        // �巡�� �������� ��
    public Action<uint> onDragEnd;          // �巡�װ� ������ ��(�ڽ� �ȿ��� ��)
    public Action<uint> onDragCancel;       // �巡�װ� �������� ��(�ڽ� �ۿ��� ��)
    public Action<uint> onClick;            // Ŭ���� �Ǿ��� ��
    public Action<uint> onShiftClick;       // ����Ʈ Ŭ���� �Ǿ��� ��
    public Action<uint> onPoinerEnter;      // ���콺 �����Ͱ� �ȿ� ������ ��
    public Action<uint> onPoinerExit;       // ���콺 �����Ͱ� ������ ������ ��
    public Action<Vector2> onPoinerMove;    // ���콺 �����Ͱ� �ȿ��� ������ ��

    // �Լ� ---------------------------------------------------------------------------------------

    private void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemCountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// ���� �ʱ�ȭ �Լ�
    /// </summary>
    /// <param name="id">������ ID</param>
    /// <param name="slot">�� UI�� ������ ItemSlot</param>
    public virtual void InitializeSlot(uint id, C_ItemSlot slot)
    {
        this.id = id;
        this.itemSlot = slot;
        this.itemSlot.onSlotItemChange = Refresh;

        onDragStart = null;
        onDragEnd = null;
        onDragCancel = null;
        onClick = null;
        onShiftClick = null;
        onPoinerEnter = null;
        onPoinerExit = null;
        onPoinerMove = null;

        Refresh();
    }

    /// <summary>
    /// �ڽ� ���� ������Ʈ���� ũ�� ����
    /// </summary>
    /// <param name="iconSize">������ �Ѻ��� ũ��</param>
    public void Resize(float iconSize)
    {
        RectTransform rectTransform = (RectTransform)itemImage.gameObject.transform;
        rectTransform.sizeDelta = new Vector2(iconSize, iconSize);
    }

    /// <summary>
    /// ������ ���̴� ��� ���� �뵵�� �Լ�. itemSlot�� ���� �����Ͱ� ����� ������ ����.
    /// </summary>
    private void Refresh()
    {
        if (itemSlot.IsEmpty)
        {
            // ������ ������ �������
            itemImage.sprite = null;        // ��������Ʈ ����
            itemImage.color = Color.clear;  // ����ȭ
            itemCountText.text = null;      // ���� ����
        }
        else
        {
            // ������ ���Կ� �������� ������
            itemImage.sprite = itemSlot.ItemData.itemIcon;  // �ش� ������ �̹��� ǥ��
            itemImage.color = Color.white;                  // ������ȭ
            itemCountText.text = itemSlot.ItemCount.ToString(); // ������ ���� ����
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // OnBeginDrag, OnEndDrag�� �����Ű�� ���� �߰�

        // eventData.position : ���콺 �������� ��ũ����ǥ��
        // eventData.delta : ���콺 �������� ��ġ ��ȭ��
        // eventData.button == PointerEventData.InputButton.Left : ���콺 ���� ��ư�� ������ �ִ�.
        // eventData.button == PointerEventData.InputButton.Right : ���콺 ������ ��ư�� ������ �ִ�.
    }

    /// <summary>
    /// EventSystems���� �巡�� ������ �����ϸ� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="eventData">���� �̺�Ʈ ������</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"�巡�� ���� : {ID}�� ����");
        onDragStart?.Invoke(ID);    // �� ���Կ��� �巡�װ� ���۵Ǿ����� �˸�
    }

    /// <summary>
    /// EventSystems���� �巡�� ���ᰡ �����Ǹ� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="eventData">���� �̺�Ʈ ������</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;    // ���� ���콺 ��ġ�� ��ŷ�� ������Ʈ�� �ִ��� Ȯ��
        if (obj != null)
        {
            // ��ŷ�� ���� �ִ�.(��κ� UI)
            C_ItemSlotUI endSlot = obj.GetComponent<C_ItemSlotUI>();        // ��ŷ�� ������Ʈ���� ItemSlotUI ��������
            Debug.Log(endSlot);
            if (endSlot != null)
            {
                Debug.Log($"�巡�� ���� : {endSlot.ID}�� ����");
                onDragEnd?.Invoke(endSlot.ID);                          // ��ŷ�� ���Կ��� �巡�װ� �������� �˸�
            }
            else
            {
                Debug.Log($"�巡�� ���� : {ID}��° ���Կ��� ����");
                onDragCancel?.Invoke(ID);                               // �巡�װ� ���������� �˸�
            }
        }
    }

    /// <summary>
    /// EventSystems���� Ŭ���� �����Ǹ� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="eventData">���� �̺�Ʈ ������</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Keyboard.current.leftShiftKey.ReadValue() > 0)
        {
            // ����Ʈ Ŭ������ ������ �и�
            if (ItemSlot.ItemCount > 1)     // 1�� �̻��� ���� �и�
            {
                onShiftClick?.Invoke(ID);
            }
        }
        else
        {
            // ��� �ִ� �ӽ� ������ ������ ��ġ�뵵
            onClick?.Invoke(ID);
        }
    }

    /// <summary>
    /// EventSystems���� ���콺 �����Ͱ� �� UI ������ ������ ����Ǵ� �Լ�
    /// </summary>
    /// <param name="eventData">���� �̺�Ʈ ������</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        onPoinerEnter?.Invoke(ID);
    }

    /// <summary>
    /// EventSystems���� ���콺 �����Ͱ� �� UI ������ ������ ����Ǵ� �Լ�
    /// </summary>
    /// <param name="eventData">���� �̺�Ʈ ������</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        onPoinerExit?.Invoke(ID);
    }

    /// <summary>
    /// EventSystems���� ���콺 �����Ͱ� �� UI �����ȿ��� �����̸� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="eventData">���� �̺�Ʈ ������</param>
    public void OnPointerMove(PointerEventData eventData)
    {
        onPoinerMove?.Invoke(eventData.position);   // ��ũ�� ��ǥ�� �Ѱ��ֱ�
    }
}
