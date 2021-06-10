using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public GameObject storePanel;
    public GameObject InventoryPanel;
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

    //Close Store and Inventory
    private void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.F) && !InventoryPanel.isActive)
        {
            storePanel.SetActive(true);

            if (inventoryPanel.activeSelf)
                inventoryPanel.SetActive(false);
            else
                inventoryPanel.SetActive(true);

        }

        if (Input.GetKeyUp(KeyCode.Escape) && storePanel.activeSelf)
        {
            storePanel.SetActive(false);
            InventoryPanel.InventorySwitch();
        }
    }


}
