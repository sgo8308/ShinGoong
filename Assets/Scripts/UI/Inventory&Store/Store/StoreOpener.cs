using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreOpener : MonoBehaviour
{
    public GameObject storePanel;
    public InventoryOpener inventoryOpener;

    public delegate void OnStoreOpened();
    public OnStoreOpened onStoreOpened;

    bool isPlayerInTrigger;

    //Interact with store
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;

        isPlayerInTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerInTrigger = false;
    }

    private void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.F))
        {
            OpenStore();
        }

        if (Input.GetKeyUp(KeyCode.Escape) && storePanel.activeSelf)
        {
            CloseStore();
        }
    }

    private void OpenStore()
    {
        storePanel.SetActive(true);
        inventoryOpener.OpenInventory();
    }

    private void CloseStore()
    {
        storePanel.SetActive(false);
        inventoryOpener.CloseInventory();
    }
}
