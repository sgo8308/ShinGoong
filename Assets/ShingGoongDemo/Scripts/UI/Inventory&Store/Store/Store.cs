using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public GameObject storePanel;
    public GameObject inventoryPanel;
    bool isPlayerInTrigger;

    public List<Item> items = new List<Item>();
    public delegate void OnInitialize(List<Item> items);
    public OnInitialize onInitialize;

    public delegate void OnCreated();
    public OnCreated onCreated;

    private void Start()
    {
        StageManager.instance.InitializeStore();   
    }

    ////Interact with store
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag != "Player")
    //        return;

    //    isPlayerInTrigger = true;
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    isPlayerInTrigger = false;
    //}

    ////Close Store and Inventory
    //private void Update()
    //{
    //    if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.F) && !inventoryPanel.activeSelf)
    //    {
    //        storePanel.SetActive(true);
    //        inventoryPanel.SetActive(true);
    //    }

    //    if (Input.GetKeyUp(KeyCode.Escape) && storePanel.activeSelf)
    //    {
    //        storePanel.SetActive(false);
    //        inventoryPanel.SetActive(false);
    //    }
    //}
}
