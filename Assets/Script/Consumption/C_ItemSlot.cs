using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �κ��丮 ��ĭ�� ������ ��Ÿ���� Ŭ����
public class C_ItemSlot
{
    /// <summary>
    /// �� ������ �ε���(�κ��丮�� ���° �����ΰ�?)
    /// </summary>
    uint slotIndex;

    /// <summary>
    /// �� ���Կ� ����ִ� ������
    /// </summary>
    ItemData slotItemData = null;

    /// <summary>
    /// �� ���Կ� ����ִ� ������ ����
    /// </summary>
    uint itemCount = 0;

    // ������Ƽ ------------------------------------------------------------------------------------

    /// <summary>
    /// �� ���Կ� ����ִ� ������ ������
    /// </summary>
    public ItemData ItemData
    {
        get => slotItemData;        // �б�� ������ ����
        private set                 // ����� �ڽŸ� ����
        {
            if (slotItemData != value)     // ���Կ� �������� ������ �־��� ����
            {
                slotItemData = value;       // ���� �����ϰ�
                onSlotItemChange?.Invoke(); // ��������Ʈ�� ����� �Լ��� ����(�ַ� UI ���ſ�)
            }
        }
    }


    /// <summary>
    /// �� ���Կ� ����ִ� ������ ����
    /// </summary>
    public uint ItemCount
    {
        get => itemCount;       // �б�� ������ ����
        private set             // ����� �ڽŸ� ����
        {
            if (itemCount != value)              // ������ ������ ������ �Ͼ�� ����
            {
                itemCount = value;              // ���� �����ϰ�
                onSlotItemChange?.Invoke();     // ��������Ʈ�� ����� �Լ��� ����(�ַ� UI ���ſ�)
            }
        }
    }


    // ������Ƽ(�б�����) --------------------------------------------------------------------------

    /// <summary>
    /// �� ������ ������� ����(true�� �����,false �����ΰ� ����ִ�.)
    /// </summary>
    public bool IsEmpty => (slotItemData == null);

    /// <summary>
    /// �� ������ �ε���
    /// </summary>
    public uint Index => slotIndex;


    // ��������Ʈ ----------------------------------------------------------------------------------

    public Action onSlotItemChange;


    // �Լ��� --------------------------------------------------------------------------------------

    public C_ItemSlot(uint index)
    {
        slotIndex = index;
    }

    /// <summary>
    /// �� ���Կ� ������ �������� ������ ������ �ִ� �Լ�
    /// </summary>
    /// <param name="data">�߰��� ������</param>
    /// <param name="count">������ ����</param>
    public void AssignSlotItem(ItemData data, uint count = 1)
    {
        if (data != null)
        {
            // data�� null�� �ƴϸ� �Ķ���ʹ�� ����
            ItemCount = count;
            ItemData = data;
            Debug.Log($"�κ��丮 {slotIndex}�� ���Կ� \"{ItemData.itemName}\" ������ {ItemCount}�� ����");
        }
        else
        {
            // data�� null�̸� ���� �Լ� ����
            ClearSlotItem();
        }
    }

    /// <summary>
    /// �� ���Կ� ������ ������ ������Ű�� �Լ�
    /// </summary>
    /// <param name="overCount">��ģ ����</param>
    /// <param name="increaseCount">������ų ������ ����</param>
    /// <returns>���� ���� ����. �� �ִ� �Ϳ� �����ϸ� true, ��ġ�� false</returns>
    public bool IncreaseSlotItem(out uint overCount, uint increaseCount = 1)
    {
        bool result;        // �� �ִ� �Ϳ� �����ϸ� true, ��ġ�� false
        int over = 0;       // �������� �߰��Ϸ��� �ϴµ� ��ģ ����

        uint newCount = ItemCount + increaseCount;              // �켱 ������ ����
        over = (int)(newCount) - (int)ItemData.maxStackCount;   // ��ģ ���� ���
        if (over > 0)
        {
            // ������ �ִ� ������ ���ƴ�.
            ItemCount = ItemData.maxStackCount; // �ִ�ġ������ ����
            overCount = (uint)over;             // ��ģ ���� ����
            result = false;                     // �������� ����� false
            Debug.Log($"�κ��丮 {slotIndex}�� ���Կ� \"{ItemData.itemName}\" ������ �ִ�ġ���� ����. ���� {ItemCount}��. {over}�� ��ħ.");
        }
        else
        {
            // ����� �߰��� �� �ִ�.
            ItemCount = newCount;   // ������ ���� ����
            overCount = 0;          // underflow������. ��ģ ���� 0���� ����
            result = true;          // �� �߰������� ����� true
            Debug.Log($"�κ��丮 {slotIndex}�� ���Կ� \"{ItemData.itemName}\" ������ {increaseCount}����ŭ ����. ���� {ItemCount}��");
        }

        return result;
    }

    /// <summary>
    /// �� ���Կ� ������ ������ ���ҽ�Ű�� �Լ�
    /// </summary>
    /// <param name="count">���ҽ�ų ������ ����</param>
    public void DecreaseSlotItem(uint count = 1)
    {
        int newCount = (int)ItemCount - (int)count; // underflow�� ����ؼ� ��ȣ�ִ� ��Ƽ���� ó��

        if (newCount < 1)
        {
            // ���ο� ������ 0���ϸ� ������ ����
            ClearSlotItem();
        }
        else
        {
            // ������ ���������� �ش� ������ ����
            ItemCount = (uint)newCount;
            Debug.Log($"�κ��丮 {slotIndex}�� ���Կ� \"{ItemData.itemName}\" ������ {count}����ŭ ����. ���� {ItemCount}��");
        }
    }

    /// <summary>
    /// �� ���Կ� �ִ� �������� ����ϴ� �Լ�
    /// </summary>
    /// <param name="target">�������� ȿ���� ���� ���</param>
    public void UseSlotItem(GameObject target = null)
    {
        /*IUsable usable = ItemData as IUsable;   // ��밡���� ���������� Ȯ��
        if (usable != null)
        {
            if (usable.Use(target))            // �������� ����ϰ� ���������� ����ߴ��� Ȯ��
            {
                DecreaseSlotItem();             // ���������� ���Ǿ����� ������ ���� 1�� ����
            }
        }*/
    }

    /// <summary>
    /// �� ���Կ��� �������� �����ϴ� �Լ�
    /// </summary>
    public void ClearSlotItem()
    {
        ItemData = null;
        ItemCount = 0;
        Debug.Log($"�κ��丮 {slotIndex}�� ������ ���ϴ�.");
    }
}