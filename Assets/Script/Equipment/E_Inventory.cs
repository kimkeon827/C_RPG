using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Inventory : MonoBehaviour
{
    DroppableUI[] slots;

    private void Awake()
    {
        slots = GetComponentsInChildren<DroppableUI>();
        
    }

    private void Start()
    {
        Debug.Log(slots.Length);
    }

    public void Swap_item(DraggableUI a, DraggableUI b)
    {
        
    }
}
