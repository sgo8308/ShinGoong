using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    bool _activeInventory = false;
    bool _canOpen;
    SceneManager sceneManager;
    StageManager stageManager;
    void Start()
    {
        inventoryPanel.SetActive(_activeInventory);

        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    void Update()
    {
        if (sceneManager.sceneType == SceneType.STAGE &&
                stageManager.stageState == StageState.UNCLEAR) 
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
}
