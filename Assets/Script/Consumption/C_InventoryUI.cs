using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class C_InventoryUI : MonoBehaviour
{
    /// <summary>
    /// ItemSlotUI�� �ִ� ������. �κ��丮 ũ�� ��ȭ�� ����ؼ� ������ �ֱ�.
    /// </summary>
    public GameObject slotPrefab;

    /// <summary>
    /// �� UI�� ������ �κ��丮
    /// </summary>
    C_Inventory inven;

    /// <summary>
    /// �� �κ��丮�� �ִ� ������ ������ UI
    /// </summary>
    C_ItemSlotUI[] slotUIs;

    /// <summary>
    /// ������ �̵� �� �����⸦ ���� �ӽ� ����
    /// </summary>
    TempItemSlotUI tempSlotUI;
    EquipmentSlot equipmentSlotUI;
    HPuseSlot hpuseSlotUI;
    MPuseSlot mpuseSlotUI;

    /// <summary>
    /// ������ �� ������ �����ִ� UIâ
    /// </summary>
    DetailInfoUI detail;

    /// <summary>
    /// ������ ������ UI â
    /// </summary>
    ItemSpliterUI spliter;

    /// <summary>
    /// �Է� ó���� ��ǲ �׼� Ŭ����
    /// </summary>
    PlayerInputActions inputActions;

    /// <summary>
    /// �κ��丮���� �κ��丮�� ������ �÷��̾ ������ Ȯ�� �����ִ� ������Ƽ
    /// </summary>
    public Player Owner => inven.Owner;

    uint Sortchecknum = 0;

    private void Awake()
    {
        // ������Ʈ ã��
        Transform slotParent = transform.GetChild(1);
        slotUIs = new C_ItemSlotUI[slotParent.childCount];
        for (int i = 0; i < slotParent.childCount; i++)
        {
            Transform child = slotParent.GetChild(i);
            slotUIs[i] = child.GetComponent<C_ItemSlotUI>();
        }

        tempSlotUI = GetComponentInChildren<TempItemSlotUI>();
        equipmentSlotUI = FindObjectOfType<EquipmentSlot>();
        hpuseSlotUI = FindObjectOfType<HPuseSlot>();
        mpuseSlotUI = FindObjectOfType<MPuseSlot>();
        detail = GetComponentInChildren<DetailInfoUI>();
        spliter = GetComponentInChildren<ItemSpliterUI>();
        spliter.onOKClick += OnSplitOK;     // ���ø��Ͱ� ������ �ִ� onOKClick ��������Ʈ�� �Լ� ���

        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Click.performed += spliter.OnMouseClick;
        //inputActions.UI.Click.canceled += tempSlotUI.OnDrop;
    }

    private void OnDisable()
    {
        //inputActions.UI.Click.canceled -= tempSlotUI.OnDrop;
        inputActions.UI.Click.performed -= spliter.OnMouseClick;
        inputActions.UI.Disable();
    }

    /// <summary>
    /// �Է¹��� �κ��丮�� �°� ���� �ʱ�ȭ �۾��� �ϴ� �Լ�
    /// </summary>
    /// <param name="playerInven">�� UI�� ǥ���� �κ��丮</param>
    public void InitializeInventory(C_Inventory playerInven)
    {
        inven = playerInven;

        Transform slotParent = transform.GetChild(1);               // �������� �뵵
        GridLayoutGroup grid = slotParent.GetComponent<GridLayoutGroup>();

        if (C_Inventory.Default_Inventory_Size != inven.SlotCount)  // �κ��丮 ũ�Ⱑ �⺻�� �ٸ� ���� ó��
        {
            // �⺻ ������� �ٸ��� ���� ������ ���� �����ϰ� ���� �����            
            //Debug.Log("�κ��丮 ����� �ٸ���.");
            foreach (var slot in slotUIs)
            {
                Destroy(slot.gameObject);   // �⺻������ ������ �ִ� ���� ��� ����
            }

            // �κ��丮 ũ�⿡ ���� ItemSlotUI�� ũ�� ����
            RectTransform rectParent = (RectTransform)slotParent;
            float totalArea = rectParent.rect.width * rectParent.rect.height;   // slotParent�� ��ü ���� ���
            float slotArea = totalArea / inven.SlotCount;                       // slot �ϳ��� ���� ���ϱ�

            float slotSideLength = Mathf.Floor(Mathf.Sqrt(slotArea)) - grid.spacing.x;  // spacing ũ�� ����ؼ� slot �Ѻ��� ���� ���ϱ�
            grid.cellSize = new Vector2(slotSideLength, slotSideLength);                // ��� ��� ����

            // ���� ���Ӱ� ����
            slotUIs = new C_ItemSlotUI[inven.SlotCount];      // ���� �迭�� �� ũ�⿡ �°� ���� ����
            for (uint i = 0; i < inven.SlotCount; i++)
            {
                GameObject obj = Instantiate(slotPrefab, slotParent);   // ������ �ϳ��� ����
                obj.name = $"{slotPrefab.name}_{i}";                    // ���� �̸��� �Ȱ�ġ�� ����
                slotUIs[i] = obj.GetComponent<C_ItemSlotUI>();            // ������ �迭�� ����                
            }
        }

        // ���� ó���κ�
        for (uint i = 0; i < inven.SlotCount; i++)
        {
            slotUIs[i].InitializeSlot((uint)i, inven[i]);           // �� ���� �ʱ�ȭ
            slotUIs[i].Resize(grid.cellSize.x * 0.75f);             // ���� ũ�⿡ �°� ���� ũ�� ��������
            slotUIs[i].onDragStart += OnItemMoveStart;              // ���Կ��� �巡�װ� ���۵� �� ����� �Լ� ����
            slotUIs[i].onDragEnd += OnItemMoveEnd;                  // ���Կ��� �巡�װ� ���� �� ����� �Լ� ����
            slotUIs[i].onDragCancel += OnItemMoveCancel;            // �巡�װ� �������� �� ����� �Լ� ����
            slotUIs[i].onClick += OnClick;                          // Ŭ���� ���� �� ����� �Լ� ����
            slotUIs[i].onShiftClick += OnItemSplit;                 // ����Ʈ Ŭ���� ���� �� ����� �Լ� ����
            slotUIs[i].onPoinerEnter += OnItemDetailOn;             // ���콺�� ���� �� ����� �Լ� ����
            slotUIs[i].onPoinerExit += OnItemDetailOff;             // ���콺�� ������ �� ����� �Լ� ����
            slotUIs[i].onPoinerMove += OnPointerMove;               // ���콺�� ���� �ȿ��� ������ �� ����� �Լ� ����
        }

        equipmentSlotUI.InitializeSlot(C_Inventory.EquipItemIndex, inven.EquipSlot);
        equipmentSlotUI.Resize(grid.cellSize.x * 0.75f);
        equipmentSlotUI.onDragStart += OnItemMoveStart;              // ���Կ��� �巡�װ� ���۵� �� ����� �Լ� ����
        equipmentSlotUI.onDragEnd += OnItemMoveEnd;                  // ���Կ��� �巡�װ� ���� �� ����� �Լ� ����
        equipmentSlotUI.onDragCancel += OnItemMoveCancel;            // �巡�װ� �������� �� ����� �Լ� ����
        equipmentSlotUI.onClick += OnClick;                          // Ŭ���� ���� �� ����� �Լ� ����
        equipmentSlotUI.onShiftClick += OnItemSplit;                 // ����Ʈ Ŭ���� ���� �� ����� �Լ� ����
        equipmentSlotUI.onPoinerEnter += OnItemDetailOn;             //  ���콺�� ���� �� ����� �Լ� ����
        equipmentSlotUI.onPoinerExit += OnItemDetailOff;             // ���콺�� ������ �� ����� �Լ� ����
        equipmentSlotUI.onPoinerMove += OnPointerMove;               // ���콺�� ���� �ȿ��� ������ �� ����� �Լ�


        hpuseSlotUI.InitializeSlot(C_Inventory.HpIndex, inven.HpSlot);
        hpuseSlotUI.Resize(grid.cellSize.x * 0.75f);
        mpuseSlotUI.InitializeSlot(C_Inventory.Mpindex, inven.MpSlot);
        mpuseSlotUI.Resize(grid.cellSize.x * 0.75f);
        // �ӽ� ���� �ʱ�ȭ ó��
        tempSlotUI.InitializeSlot(C_Inventory.TempSlotIndex, inven.TempSlot); // �ӽ� ���� �ʱ�ȭ
        tempSlotUI.onTempSlotOpenClose += OnDetailPause;
        tempSlotUI.Close(); // �⺻������ �ݾ� ����
    }

    /// <summary>
    /// Ȯ���� ��ũ�� ��ǥ�� �κ��丮 ���� ������ Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="screenPos">Ȯ���� ��ũ�� ��ǥ</param>
    /// <returns>�κ��丮 ���� �ȿ� ������ true, �ƴϸ� false</returns>
    public bool IsInInventoryArea(Vector2 screenPos)
    {
        RectTransform rectTransform = (RectTransform)transform;

        Vector2 min = new(rectTransform.position.x - rectTransform.sizeDelta.x, rectTransform.position.y);
        Vector2 max = new(rectTransform.position.x, rectTransform.position.y + rectTransform.sizeDelta.y);

        return (min.x < screenPos.x && screenPos.x < max.x && min.y < screenPos.y && screenPos.y < max.y);  // min, max ���̿� �ִ��� Ȯ��
    }

    /// <summary>
    /// ���Կ� �巡�׸� �������� �� ����� �Լ�
    /// </summary>
    /// <param name="slotID">�巡�װ� ���۵� ������ ID</param>
    private void OnItemMoveStart(uint slotID)
    {
        inven.MoveItem(slotID, C_Inventory.TempSlotIndex);    // ���Կ� �ִ� �����۵��� �ӽ� �������� ��� �ű�
        tempSlotUI.Open();                                  // �ӽ� ������ �����ֱ�
    }

    /// <summary>
    /// �巡�װ� ���Կ��� ������ ��, Ŭ���� �Ǿ��� �� ����� �Լ�
    /// </summary>
    /// <param name="slotID">�巡�װ� ���� ������ ID</param>
    private void OnItemMoveEnd(uint slotID)
    {
        OnItemMoveCancel(slotID);
        detail.Open(inven[slotID].ItemData);
    }

    /// <summary>
    /// ���콺�� ���Կ��� Ŭ���� �Ǿ��� �� ����� �Լ�
    /// </summary>
    /// <param name="slotID">Ŭ���� ������ ID</param>
    private void OnClick(uint slotID)
    {
        if (tempSlotUI.ItemSlot.IsEmpty)
        {
            // �������� ����� �뵵
            if (inven[slotID].ItemData.id == 2)
            {
                GameManager.Inst.Logger.Log($"<#0024FF>{inven[slotID].ItemData.itemName} ��� �����Ǿ����ϴ�.</color>");
                inven.MoveEquipItem(slotID, C_Inventory.EquipItemIndex);
            }
            else if(inven[slotID].ItemData.id == 0)
            {
                GameManager.Inst.Logger.Log($"<#0024FF>{inven[slotID].ItemData.itemName} HP������ �����Ǿ����ϴ�.</color>");
                inven.MoveHpItem(slotID, C_Inventory.HpIndex);
            }   
            else if (inven[slotID].ItemData.id == 1)
            {
                GameManager.Inst.Logger.Log($"<#0024FF>{inven[slotID].ItemData.itemName} MP������ �����Ǿ����ϴ�.</color>");
                inven.MoveMpItem(slotID, C_Inventory.Mpindex);
            }
        }
        else
        {
            // �ӽ� ������ �������� slotID ���Կ� ���� �뵵
            OnItemMoveEnd(slotID);
        }
    }

    /// <summary>
    /// ������ ����Ʈ Ŭ������ �� ����� �Լ�
    /// </summary>
    /// <param name="slotID"></param>
    private void OnItemSplit(uint slotID)
    {
        Debug.Log("����ƮŬ��");
        C_ItemSlotUI targetSlot = slotUIs[slotID];
        spliter.transform.position = targetSlot.transform.position + Vector3.up * 100;
        spliter.Open(targetSlot);
        detail.Close();
        detail.IsPause = true;
    }



    /// <summary>
    /// �巡�װ� �������� �� ����� �Լ�
    /// </summary>
    /// <param name="slotID">�巡�װ� ���� ������ ID</param>
    private void OnItemMoveCancel(uint slotID)
    {
        inven.MoveItem(C_Inventory.TempSlotIndex, slotID);    // �ӽ� ������ �����۵��� ���Կ� ��� �ű�
        if (tempSlotUI.ItemSlot.IsEmpty)
        {
            tempSlotUI.Close();                             // �ӽ� ������ �Ⱥ��̰� �����
        }
    }

    /// <summary>
    /// ���콺�� ���Կ� ���� �� �ش� ���Կ� �ִ� �������� �� ���� â���� �� �� �ֵ��� �����ϰ� ���� �Լ�
    /// </summary>
    /// <param name="slotID">��� ����</param>
    private void OnItemDetailOn(uint slotID)
    {
        detail.Open(slotUIs[slotID].ItemSlot.ItemData); // ��� ������ ������ ������ �Ѱ��ָ� ����
    }

    /// <summary>
    /// ���콺�� ������ ������ �� ������â�� �ݴ� �Լ�
    /// </summary>
    /// <param name="_">��� ����</param>
    private void OnItemDetailOff(uint _)
    {
        detail.Close();
    }

    /// <summary>
    /// ���콺�� ���Ծȿ��� ������ �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="pointerPos">���콺 �������� ��ũ�� ��ǥ</param>
    private void OnPointerMove(Vector2 pointerPos)
    {
        if (detail.IsOpen)  // ������ â�� �������� ����
        {
            detail.MovePosition(pointerPos);
        }
    }

    /// <summary>
    /// TempItemSlotUI�� ������ ���� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="isPause">true�� ������ ����Ǿ��� ��. false�� ������ ����Ǿ��� ��</param>
    private void OnDetailPause(bool isPause)
    {
        detail.IsPause = isPause;   // �ӽ� ������ ������ ������â�� �Ͻ� ����
                                    // �ӽ� ������ ������ ������â �Ͻ� ���� ����
    }

    /// <summary>
    /// ������ �и�â���� OK�� Ŭ���Ǿ��� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="slotID">�������� �и��� ����</param>
    /// <param name="count">�и��� ������ ����</param>
    private void OnSplitOK(uint slotID, uint count)
    {
        inven.MoveItemToTempSlot(slotID, count);    // slotID��° ���Կ��� �������� count��ŭ �и��ؼ� �ӽý��Կ� ���
        tempSlotUI.Open();                          // �ӽý����� ���̰� �����
    }


    private void sortstart(int itemid)
    {
        for (uint i = 1; i < 16; i++)
        {
            if (inven[i].IsEmpty)
            {

            }
            else if (inven[i].ItemData.id == itemid)
            {
                if (i == Sortchecknum)
                {
                    Sortchecknum++;
                }
                else
                {
                    inven.SortItemnum(i, Sortchecknum);
                    Sortchecknum++;
                }
            }
        }
    }

    //������ ���� â
    public void SortItem()
    {
        for (int i = 0; i <= 2; i++)
        {
            sortstart(i);
        }
        GameManager.Inst.Logger.Log($"<#FF0092> �������� �����߽��ϴ�.</color>");
        Sortchecknum = 0;
    }

    public void HPslotuse()
    {
        //inven.RemoveItem(C_Inventory.HpIndex,1);
        GameManager.Inst.Logger.Log($"<#FF0300> HP������ ����߽��ϴ�.</color>");
    }

    public void MPslotuse()
    {
        //inven.RemoveItem(C_Inventory.HpIndex,1);
        GameManager.Inst.Logger.Log($"<#FF0300> MP������ ����߽��ϴ�.</color>");
    }
}