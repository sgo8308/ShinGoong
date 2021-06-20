using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    bool _activeInventory = false;
    bool _canOpen;
    SceneManager sceneManager;

    void Start()
    {
        inventoryPanel.SetActive(_activeInventory);

        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        
        Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
    }

    void Update()
    {
        if (sceneManager.sceneType == SceneManager.SceneType.STAGE) 
        {
            
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            _activeInventory = !_activeInventory;
            inventoryPanel.SetActive(_activeInventory);
        }

        if (inventoryPanel.activeSelf)
        {
            MouseCursor.isAimCursorNeeded = false;
            Player.canMove = false;
        }
        else
        { 
            MouseCursor.isAimCursorNeeded = true;
            Player.canMove = true;
        }
    }
}
