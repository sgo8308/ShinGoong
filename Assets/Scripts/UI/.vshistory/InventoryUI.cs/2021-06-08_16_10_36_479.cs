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

        if(GameObject.Find("StageManager") != null)
            stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    void Update()
    {
        if (sceneManager.sceneType == SceneManager.SceneType.STAGE &&
                stageManager.stageState == StageManager.StageStage.UNCLEAR) 
        {
            //전투 중에는 열 수 없습니다 문구 출력하기
            return;
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
