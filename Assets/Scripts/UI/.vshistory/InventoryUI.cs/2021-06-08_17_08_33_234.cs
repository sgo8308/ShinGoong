using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;

    bool _activeInventory = false;

    Inventory inventory;

    SceneManager _sceneManager;
    StageManager _stageManager;

    List<InventorySlot> slots;

    void Start()
    {
        inventoryPanel.SetActive(_activeInventory);

        _sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        _stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

        inventory.onChangeItem += RedrawSlotUI;
        slots = new List<InventorySlot>();
    }

    void Update()
    {
        if (_sceneManager.sceneType == SceneType.STAGE &&
                _stageManager.stageState == StageState.UNCLEAR) 
            return;

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

    void RedrawSlotUI()
    {

    }
}
