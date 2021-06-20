using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : MonoBehaviour
{
    public GameObject StorePanel;
    bool _isActive = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            StorePanel.SetActive(true);
        }
    }
}
