using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPuseSlot : C_ItemSlotUI
{
    C_InventoryUI inven;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            inven.HPslotuse();
        }
    }
}
