using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameObject inventoryPanel;

    bool _activeInventory = false;

    SceneManager _sceneManager;
    StageManager _stageManager;


    void Start()
    {
        inventoryPanel.SetActive(_activeInventory);

        _sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        _stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

    }

    void Update()
    {
        //Inventory
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



    public Image itemToolTip;
    public void ShowToolTip()
    {
        itemToolTip.gameObject.SetActive(true);
    }
}
