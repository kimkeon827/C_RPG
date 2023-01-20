using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InvenUI : MonoBehaviour
{
    public GameObject Inventory;
    public C_InventoryUI inventoryUI;
    private void Awake()
    {
        Button CloseButton = transform.GetChild(1).GetComponent<Button>();
        Button Sortbutton = transform.GetChild(2).GetComponent<Button>();
        CloseButton.onClick.AddListener(Close);
        Sortbutton.onClick.AddListener(Sort);
    }


    void Close()
    {
        Inventory.SetActive(false);
    }

    void Sort()
    {
        inventoryUI.SortItem();
    }
}
