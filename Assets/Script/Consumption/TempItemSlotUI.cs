using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.InputSystem;
using static UnityEngine.UI.GridLayoutGroup;

public class TempItemSlotUI : C_ItemSlotUI
{
    /// <summary>
    /// �� �ӽý����� ���Ե� �κ��丮
    /// </summary>
    private C_InventoryUI invenUI;

    /// <summary>
    /// �� �ӽ� ������ ���Ե� �κ��丮�� ������ �ִ� �÷��̾�
    /// </summary>
    private Player owner;

    /// <summary>
    /// �ӽ� ������ ������ ������ �˸��� ��������Ʈ. true�� ���ȴ�. false�� ������.
    /// </summary>
    public Action<bool> onTempSlotOpenClose;

    private void Update()
    {
        transform.position = Mouse.current.position.ReadValue();        // �� �����Ӹ��� ���콺 ��ġ�� �̵�
    }

    /// <summary>
    /// ���� �ʱ�ȭ �Լ�
    /// </summary>
    /// <param name="id">������ ID. 99999</param>
    /// <param name="slot">�� UI�� ������ �ӽ� ItemSlot</param>
    /// <param name="owner">�� UI�� ����ϴ� �÷��̾�</param>
    public override void InitializeSlot(uint id, C_ItemSlot slot)
    {
        onTempSlotOpenClose = null;     // ��������Ʈ �ʱ�ȭ �߰�

        invenUI = GameManager.Inst.InvenUI; // �κ��丮 UI ã��
        owner = invenUI.Owner;              // owner ����

        base.InitializeSlot(id, slot);
    }

    /// <summary>
    /// TempItemSlotUI�� ���� �Լ�
    /// </summary>
    public void Open()
    {
        if (!ItemSlot.IsEmpty)               // �������� ������� ���� ����
        {
            transform.position = Mouse.current.position.ReadValue();    // ���� �� ���콺 ��ġ�� �̵�
            onTempSlotOpenClose?.Invoke(true);  // �����ٰ� �˸�
            gameObject.SetActive(true);         // Ȱ��ȭ
        }
    }

    /// <summary>
    /// TempItemSlotUI�� �ݴ� �Լ�
    /// </summary>
    public void Close()
    {
        onTempSlotOpenClose?.Invoke(false);     // �����ٰ� �˸�
        gameObject.SetActive(false);            // ��Ȱ��ȭ
    }

    /*public void OnDrop(InputAction.CallbackContext _)
    {
        //Debug.Log("OnDrop");
        Vector2 screenPos = Mouse.current.position.ReadValue();         // ��ũ�� ��ǥ ��������
        if (!invenUI.IsInInventoryArea(screenPos) && !ItemSlot.IsEmpty) // ��ũ����ǥ�� �κ��丮 ���� ���̰� �ӽ� ���Կ� �������� ���� ��
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);          // ��ũ�� ��ǥ�� ���� ����
            //Debug.Log($"Ray : {ray}");
            if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f, LayerMask.GetMask("Ground"))) // ���̿� ���� �浹 ���� Ȯ��
            {
                // ���̿� ���� �浹������
                Vector3 dropDir = hit.point - owner.transform.position; // ��ŷ�� ������ �÷��̾��� ��ġ�� ����ؼ� ���� ���� ���ϱ�
                Vector3 dropPos = hit.point;    // ��ŷ�� ���� ���� ����

                if (dropDir.sqrMagnitude > owner.itemPickupRange * owner.itemPickupRange)
                {
                    // ��ŷ�� ������ �ʹ� �ָ� ������ ������
                    // �÷��̾� ��ġ���� ���� ����(owner.itemPickupRange)�� ����� �ʵ��� ó��
                    dropPos = owner.transform.position + dropDir.normalized * owner.itemPickupRange;
                }

                // ������ ����
                ItemFactory.MakeItem((int)ItemSlot.ItemData.id, (int)ItemSlot.ItemCount, dropPos, true);
                ItemSlot.ClearSlotItem();   // �ӽ� ���� ����
                Close();                    // �ӽ� ���� �ݱ�
            }
        }
    }*/
}
