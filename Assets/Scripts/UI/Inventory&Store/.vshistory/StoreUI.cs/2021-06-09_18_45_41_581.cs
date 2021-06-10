using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : MonoBehaviour
{
    public GameObject StorePanel;
    public GameObject InventoryPanel;

    bool _isActive = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;
        
        if (Input.GetKeyUp(KeyCode.F) && !InventoryPanel.activeSelf)
        {
            StorePanel.SetActive(true);
            InventoryPanel.SetActive(true);
        }
    }


}
