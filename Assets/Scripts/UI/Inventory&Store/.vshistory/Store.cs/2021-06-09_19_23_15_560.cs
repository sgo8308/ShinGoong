using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public GameObject StorePanel;
    public InventoryUI InventoryPanel;
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
        if (Input.GetKeyDown(KeyCode.F) && !InventoryPanel.isActive)
        {
            StorePanel.SetActive(true);
            InventoryPanel.InventorySwitch();
        }

        if (Input.GetKeyUp(KeyCode.Escape) && StorePanel.activeSelf)
        {
            StorePanel.SetActive(false);
            InventoryPanel.InventorySwitch();
        }
    }


}
