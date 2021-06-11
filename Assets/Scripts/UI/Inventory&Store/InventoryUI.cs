using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject storePanel;

    public bool isActive = false;

    SceneManager _sceneManager;
    StageManager _stageManager;

    public InventorySlot[] _slots;
    public Transform slotHolder;

    Inventory _inventory;

    void Start()
    {
        inventoryPanel.SetActive(isActive);

        _sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        _stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

        _slots = slotHolder.GetComponentsInChildren<InventorySlot>();

        _inventory = Inventory.instance;
        _inventory.onChangeItem += RedrawSlotUI;

        AllocateSlotNum();
        UpdateCoinUI();
    }

    void Update()
    {
        if (_sceneManager.sceneType == SceneType.STAGE &&
                _stageManager.stageState == StageState.UNCLEAR)
            return;

        if (Input.GetKeyDown(KeyCode.I) && !storePanel.activeSelf)
        {
            if(inventoryPanel.activeSelf)
                inventoryPanel.SetActive(false);
            else
                inventoryPanel.SetActive(true);
        }

        if (!storePanel.activeSelf && inventoryPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            if (inventoryPanel.activeSelf)
                inventoryPanel.SetActive(false);
            else
                inventoryPanel.SetActive(true);
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
            transform.Find("ItemToolTip").gameObject.SetActive(false);
        }
    }



    void RedrawSlotUI()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].RemoveSlot();
        }

        for (int i = 0; i < _inventory.items.Count; i++)
        {
            _slots[i].item = _inventory.items[i];
            _slots[i].UpdateSlotUI();
        }

        AllocateSlotNum();
    }

    void AllocateSlotNum()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].slotNumber = i;
        }
    }

    public void UpdateCoinUI()
    {
        transform.Find("InventoryPanel")
                 .Find("MoneyPanel")
                 .Find("CoinCount")
                 .GetComponent<TextMeshProUGUI>().text = MainUI.instance.coinCount.ToString();
    }
}
