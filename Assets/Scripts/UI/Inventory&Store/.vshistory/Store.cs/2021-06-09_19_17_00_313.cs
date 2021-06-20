using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public GameObject StorePanel;
    public InventoryUI InventoryPanel;

    //Interact with store
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;
        
        if (Input.GetKeyUp(KeyCode.F) && !InventoryPanel.isActive)
        {
            StorePanel.SetActive(true);
            InventoryPanel.InventorySwitch();
        }
    }

    //Close Store and Inventory
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && StorePanel.activeSelf)
        {
            StorePanel.SetActive(false);
            InventoryPanel.InventorySwitch();
        }
    }


}
