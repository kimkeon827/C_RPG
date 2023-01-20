using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class total_InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanme;
    bool activeInventory = false;

    private void Start()
    {
        inventoryPanme.SetActive(activeInventory);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            activeInventory = !activeInventory;
            inventoryPanme.SetActive(activeInventory);
        }
    }
}
