using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : MonoBehaviour
{
    public GameObject StorePanel;
    public InventoryUI InventoryPanel;

    bool _isActive = false;

    
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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && StorePanel.activeSelf)
        {
            StorePanel.SetActive(true);
            InventoryPanel.InventorySwitch();
        }
    }


}
