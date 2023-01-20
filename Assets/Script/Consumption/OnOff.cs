using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOff : MonoBehaviour
{
    public GameObject Inventory;

    private int checknum = 0;

    private void Awake()
    {
        Inventory.SetActive(true);
    }

    private void Start()
    {
        Inventory.SetActive(false);
    }
    private void Update()
    {
        if(checknum == 0 && Input.GetKeyDown(KeyCode.I))
        {
            Inventory.SetActive(true);
            checknum = 1;
            StartCoroutine(TimeSleep());
        }
        else if(checknum == 1 && Input.GetKeyDown(KeyCode.I))
        {
            Inventory.SetActive(false);
            checknum = 0;
            StartCoroutine(TimeSleep());
        }
    }

    IEnumerator TimeSleep()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
