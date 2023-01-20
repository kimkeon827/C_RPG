using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

// �κ��丮�� ������ ������ Ŭ����
public class C_Inventory
{
    // ��� ---------------------------------------------------------------------------------------

    /// <summary>
    /// �⺻ �κ��丮 ĭ �� 
    /// </summary>
    public const int Default_Inventory_Size = 6;
    public const uint TempSlotIndex = 99999;        // � ���ڵ� �������. slots�� �ε����� �� �� �ִ� ���� �ƴϸ� �ȴ�.
    public const uint EquipItemIndex = 99998;
    public const uint HpIndex = 99997;
    public const uint Mpindex = 99996;
    // ���� ---------------------------------------------------------------------------------------

    /// <summary>
    /// �� �κ��丮�� ������ �ִ� ������ ������ �迭
    /// </summary>
    C_ItemSlot[] slots = null;

    /// <summary>
    /// �巡�� ���� �������� �ӽ� �����ϴ� ����
    /// </summary>
    C_ItemSlot tempSlot = null;
    C_ItemSlot equipSlot = null;
    C_ItemSlot hpSlot = null;
    C_ItemSlot mpSlot = null;

    /// <summary>
    /// ���� �޴����� ������ ������ ������ �Ŵ��� ĳ�̿�
    /// </summary>
    ItemDataManager dataManager;

    /// <summary>
    /// �� �κ��丮�� ������ �ִ� �÷��̾�
    /// </summary>
    Player owner;

    // ������Ƽ ------------------------------------------------------------------------------------

    public int SlotCount => slots.Length;

    public C_ItemSlot TempSlot => tempSlot;
    public C_ItemSlot EquipSlot => equipSlot;
    public C_ItemSlot HpSlot => hpSlot;
    public C_ItemSlot MpSlot => mpSlot;

    /// <summary>
    /// Ư�� ��°�� ItemSlot�� �����ִ� �ε���
    /// </summary>
    /// <param name="index">������ ������ ��ġ</param>
    /// <returns>index��°�� �ִ� ItemSlot</returns>
    public C_ItemSlot this[uint index] => slots[index];

    /// <summary>
    /// �� �κ��丮�� ������ �ִ� �÷��̾ Ȯ���ϴ� ������Ƽ
    /// </summary>
    public Player Owner => owner;

    // �Լ��� --------------------------------------------------------------------------------------       
    public C_Inventory(Player owner, int size = Default_Inventory_Size)
    {
        Debug.Log($"{size}ĭ¥�� �κ��丮 ����");
        slots = new C_ItemSlot[size];
        for (int i = 0; i < size; i++)
        {
            slots[i] = new C_ItemSlot((uint)i);
        }
        tempSlot = new C_ItemSlot(TempSlotIndex);
        equipSlot = new C_ItemSlot(EquipItemIndex);
        hpSlot = new C_ItemSlot(HpIndex);
        mpSlot = new C_ItemSlot(Mpindex);

        dataManager = GameManager.Inst.ItemData;

        this.owner = owner;
    }

    /// <summary>
    /// �������� �κ��丮�� 1�� �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="code">�߰��� �������� �ڵ�</param>
    /// <returns>��������(true�� ����, false�� ����)</returns>
    public bool AddItem(ItemIDCode code)
    {
        return AddItem(dataManager[code]);
    }

    /// <summary>
    /// �������� �κ��丮�� 1�� �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="data">�߰��� ������ ������</param>
    /// <returns>��������(true�� ����, false�� ����)</returns>
    public bool AddItem(ItemData data)
    {
        GameManager.Inst.Logger.Log($"<#26FF00>{data.itemName}�� 1�� ������ϴ�. </color>");
        // ���� ������ �������� ��ġ���� ��� �ؾ� �ϴ°�?

        bool result = false;

        // ���� ������ �������� �ִ°�?
        // ������ -> ���� ����
        // ������ -> �� ���Կ� ������ �ֱ�

        C_ItemSlot targetSlot = FindSameItem(data);
        if (targetSlot != null)
        {
            // ���� ������ �������� �ִ�.                        
            result = targetSlot.IncreaseSlotItem(out uint _);    // ���� ���� �õ�. ����� ���� result ����
        }
        else
        {
            // �κ��丮�� ���� ������ �������� ����.
            C_ItemSlot emptySlot = FindEmptySlot();
            if (emptySlot != null)
            {
                // ����ִ� ������ ã�Ҵ�.
                emptySlot.AssignSlotItem(data);
                result = true;
            }
            else
            {
                // �κ��丮�� ���� á��.
                Debug.Log($"���� : �κ��丮�� ���� á���ϴ�.");
            }
        }

        return result;
    }

    /// <summary>
    /// �������� �κ��丮�� Ư�� ���Կ� 1�� �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="code">�߰��� ������ �ڵ�</param>
    /// <param name="index">�������� �߰��� ������ �ε���</param>
    /// <returns>true�� ����, false�� ����</returns>
    public bool AddItem(ItemIDCode code, uint index)
    {
        return AddItem(dataManager[code], index);
    }

    /// <summary>
    /// �������� �κ��丮�� Ư�� ���Կ� 1�� �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="data">�߰��� ������ ������</param>
    /// <param name="index">�������� �߰��� ������ �ε���</param>
    /// <returns>true�� ����, false�� ����</returns>
    public bool AddItem(ItemData data, uint index)
    {
        bool result = false;

        if (IsValidSlotIndex(index))         // �ε����� �����Ѱ�?
        {
            C_ItemSlot slot = slots[index];   // �ش� �ε����� ���� ��������

            if (slot.IsEmpty)                // �ش� ������ ����ִ°�?
            {
                // ��������� �׳� �������� �ִ´�.
                slot.AssignSlotItem(data);
                result = true;
            }
            else
            {
                // ������ ������� �ʴ�.
                if (slot.ItemData == data)   //���� ������ �������� �ִ°�?
                {
                    // ���� ������ �������� ��������� ������ �߰�                    
                    result = slot.IncreaseSlotItem(out uint _);    // ���� ���� �õ�. ����� ���� result ����
                }
                else
                {
                    // �ٸ� ������ �������� ��������� �׳� ����
                    Debug.Log($"���� : �κ��丮 {index}�� ���Կ� �ٸ� �������� ����ֽ��ϴ�.");
                }
            }
        }
        else
        {
            Debug.Log($"���� : {index}�� �߸��� �ε����Դϴ�.");
        }

        return result;
    }

    /// <summary>
    /// �������� Ư�� �κ��丮 ���Կ��� Ư�� ������ŭ �����ϴ� �Լ�
    /// </summary>
    /// <param name="slotIndex">�������� ������ ������ �ε���</param>
    /// <param name="decreaseCount">������ ����(�⺻������ 1)</param>
    /// <returns>�����̸� true, ���и� false</returns>
    public bool RemoveItem(uint slotIndex, uint decreaseCount = 1)
    {
        bool result = false;
        if (IsValidSlotIndex(slotIndex))   // ������ �ε����� ��
        {
            C_ItemSlot slot = slots[slotIndex];       // �ش� ���Կ�
            slot.DecreaseSlotItem(decreaseCount);   // decreaseCount��ŭ ������ ���� ����
            result = true;
        }
        else
        {
            Debug.Log($"���� : {slotIndex}�� �߸��� �ε����Դϴ�.");
        }

        return result;
    }

    /// <summary>
    /// Ư�� ���Կ��� �������� �����ϴ� �Լ�
    /// </summary>
    /// <param name="slotIndex">�������� ������ �Լ�</param>
    /// <returns>true�� ����, false�� ����</returns>
    public bool ClearItem(uint slotIndex)
    {
        bool result = false;

        if (IsValidSlotIndex(slotIndex))
        {
            C_ItemSlot slot = slots[slotIndex];
            slot.ClearSlotItem();
            return true;
        }
        else
        {
            Debug.Log($"���� : {slotIndex}�� �߸��� �ε����Դϴ�.");
        }

        return result;
    }

    /// <summary>
    /// �κ��丮�� ��� �������� ���� �Լ�
    /// </summary>
    public void ClearInventory()
    {
        foreach (var slot in slots)
        {
            slot.ClearSlotItem();
        }
    }

    /// <summary>
    /// �������� �̵���Ű�� �Լ�
    /// </summary>
    /// <param name="from">���� �ε���</param>
    /// <param name="to">��Ģ �ε���</param>
    public void MoveItem(uint from, uint to)
    {
        // from�� ������ �ε����̰� �������� ����ִ�. �׸��� to�� ������ �ε����̴�.
        if (IsValidAndNotEmptySlotIndex(from) && IsValidSlotIndex(to))
        {
            // ���� ��������
            C_ItemSlot fromSlot = (from == C_Inventory.TempSlotIndex) ? TempSlot : slots[from];
            C_ItemSlot toSlot = (to == C_Inventory.TempSlotIndex) ? TempSlot : slots[to];

            if (fromSlot.ItemData == toSlot.ItemData)
            {
                // from�� to�� ���� �������� ������ ������ to���� ������ ��ġ��
                toSlot.IncreaseSlotItem(out uint overCount, fromSlot.ItemCount);    // ������ ���� �õ��� �� ��ģ ���� �޾ƿ���
                fromSlot.DecreaseSlotItem(fromSlot.ItemCount - overCount);          // from���� to�� ������ �з� ��ŭ�� ���ҽ�Ű��
                Debug.Log($"�κ��丮�� {from}���Կ��� {to}�������� ������ ��ġ�� ����");
            }
            else
            {
                // from�� to�� ���� �ٸ� �������� ������ ������ ���� ����ó��
                ItemData tempData = fromSlot.ItemData;
                uint tempCount = fromSlot.ItemCount;
                fromSlot.AssignSlotItem(toSlot.ItemData, toSlot.ItemCount);
                toSlot.AssignSlotItem(tempData, tempCount);
                Debug.Log($"�κ��丮�� {from}���԰� {to}������ ������ ��ü ����");
            }
        }
    }

    public void MoveEquipItem(uint from, uint to)
    {
        C_ItemSlot fromSlot = (from == C_Inventory.EquipItemIndex) ? EquipSlot : slots[from];
        C_ItemSlot toSlot = (to == C_Inventory.EquipItemIndex) ? EquipSlot : slots[to];

        ItemData tempData = fromSlot.ItemData;
        uint tempCount = fromSlot.ItemCount;
        fromSlot.AssignSlotItem(toSlot.ItemData, toSlot.ItemCount);
        toSlot.AssignSlotItem(tempData, tempCount);
    }

    public void MoveHpItem(uint from, uint to)
    {
        C_ItemSlot fromSlot = (from == C_Inventory.HpIndex) ? HpSlot : slots[from];
        C_ItemSlot toSlot = (to == C_Inventory.HpIndex) ? HpSlot : slots[to];

        ItemData tempData = fromSlot.ItemData;
        uint tempCount = fromSlot.ItemCount;
        fromSlot.AssignSlotItem(toSlot.ItemData, toSlot.ItemCount);
        toSlot.AssignSlotItem(tempData, tempCount);
    }

    public void MoveMpItem(uint from, uint to)
    {
        C_ItemSlot fromSlot = (from == C_Inventory.Mpindex) ? MpSlot : slots[from];
        C_ItemSlot toSlot = (to == C_Inventory.Mpindex) ? MpSlot : slots[to];

        ItemData tempData = fromSlot.ItemData;
        uint tempCount = fromSlot.ItemCount;
        fromSlot.AssignSlotItem(toSlot.ItemData, toSlot.ItemCount);
        toSlot.AssignSlotItem(tempData, tempCount);
    }

    public void SortItemnum(uint from, uint to)
    {
        C_ItemSlot fromSlot = (from == C_Inventory.TempSlotIndex) ? TempSlot : slots[from];
        C_ItemSlot toSlot = (to == C_Inventory.TempSlotIndex) ? TempSlot : slots[to];

        ItemData tempData = fromSlot.ItemData;
        uint tempCount = fromSlot.ItemCount;
        fromSlot.AssignSlotItem(toSlot.ItemData, toSlot.ItemCount);
        toSlot.AssignSlotItem(tempData, tempCount);
    }

    /// <summary>
    /// Ư�� ���Կ� �ִ� �������� �ӽ� �������� �ű�� �Լ�
    /// </summary>
    /// <param name="slotID">�������� ���ҽ�ų ����</param>
    /// <param name="count">���ҽ�Ű�� ����</param>
    public void MoveItemToTempSlot(uint slotID, uint count)
    {
        if (IsValidAndNotEmptySlotIndex(slotID))    // ������ �����϶�(�ε����� �����ϰ� �������� ����ִ�.)
        {
            C_ItemSlot fromSlot = slots[slotID];                  // ���� ��������
            fromSlot.DecreaseSlotItem(count);                   // ���� ���Կ� ����ִ� ������ ������ count��ŭ ����
            tempSlot.AssignSlotItem(fromSlot.ItemData, count);  // �ӽ� ���Կ� ���� ���Կ� ����ִ� ������ ������ count��ŭ ����
        }
    }

    // ������ ���

    /// <summary>
    /// ����ִ� ������ ã�� �Լ�
    /// </summary>
    /// <returns>����ִ� �Լ��� ã���� null�� �ƴϰ� ����ִ� �Լ��� ������ null</returns>
    private C_ItemSlot FindEmptySlot()
    {
        C_ItemSlot result = null;
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                result = slot;
                break;
            }
        }
        return result;
    }

    /// <summary>
    /// �κ��丮�� �Ķ���Ϳ� ���� ������ �������� �ִ��� ã�ƺ��� �Լ�(�������� �� ������ �ִ����� Ȯ��)
    /// </summary>
    /// <param name="itemData">ã�� ������</param>
    /// <returns>ã������ null�� �ƴѰ�(ã�� �������� ����ִ� ����), ã�� �������� null</returns>
    private C_ItemSlot FindSameItem(ItemData itemData)
    {
        C_ItemSlot findSlot = null;

        foreach (var slot in slots)
        {
            // ���� ������ �������̰� ��ĭ�� �־�� �Ѵ�.
            if (slot.ItemData == itemData && slot.ItemCount < slot.ItemData.maxStackCount)
            {
                findSlot = slot;
                break;
            }
        }

        return findSlot;
    }

    /// <summary>
    /// �Ķ���ͷ� ���� �ε����� ������ �ε������� �Ǵ��ϴ� �Լ�
    /// </summary>
    /// <param name="index">Ȯ���� �ε���</param>
    /// <returns>true�� ��밡���� �ε���, false�� ���Ұ����� �ε���</returns>
    private bool IsValidSlotIndex(uint index) => (index < SlotCount) || (index == TempSlotIndex) || (index == EquipItemIndex);

    /// <summary>
    /// �Ķ���ͷ� ���� �ε����� ������ �ε����̸鼭 ������� ���� ���� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="index">Ȯ���� �ε���</param>
    /// <returns>true�� ������ �ε����̸鼭 �������� ����ִ� �Լ�, false�� ������ �ε����� �ƴϰų� ����ִ�.</returns>
    private bool IsValidAndNotEmptySlotIndex(uint index)
    {
        if (IsValidSlotIndex(index))
        {
                C_ItemSlot testSlot = (index == TempSlotIndex) ? TempSlot : slots[index];

                return !testSlot.IsEmpty;
        }

        return false;
    }

    public void PrintInventory()
    {
        // ��� ���� : [ ���(1), �����̾�(1), ���޶���(2), (��ĭ), (��ĭ), (��ĭ) ]
        string printText = "[ ";

        // ItemsSlot�� ������ ������ ���� ���� Ȯ���ؼ� �̸��� ������ printText�� �߰�
        for (int i = 0; i < SlotCount - 1; i++)
        {
            if (!slots[i].IsEmpty)
            {
                printText += $"{slots[i].ItemData.itemName}({slots[i].ItemCount})";
            }
            else
            {
                printText += "(��ĭ)";
            }
            printText += ", ";
        }

        // ������ ���Ը� ���� ó��
        C_ItemSlot lastSlot = slots[SlotCount - 1];
        if (!lastSlot.IsEmpty)
        {
            printText += $"{lastSlot.ItemData.itemName}({lastSlot.ItemCount}) ]";
        }
        else
        {
            printText += "(��ĭ) ]";
        }

        Debug.Log(printText);
    }
}