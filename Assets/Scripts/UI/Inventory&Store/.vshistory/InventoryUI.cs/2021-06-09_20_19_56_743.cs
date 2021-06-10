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
    }

    void Update()
    {
        if (_sceneManager.sceneType == SceneType.STAGE &&
                _stageManager.stageState == StageState.UNCLEAR)
            return;

        if (Input.GetKeyDown(KeyCode.I))
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

    #region ToolTip
    public GameObject itemToolTip;

    public void SetToolTipInfo(Item item)
    {
        itemToolTip.transform.Find("Name")
                             .GetComponent<TextMeshProUGUI>().text = item.itemName;

        itemToolTip.transform.Find("ItemImage&Ability")
                             .Find("ItemImage")
                             .GetComponent<Image>().sprite = item.itemImage;

        itemToolTip.transform.Find("ItemImage&Ability")
                             .Find("Ability")
                             .GetComponent<TextMeshProUGUI>().text = item.itemAbilityInfo;

        itemToolTip.transform.Find("ItemInfo")
                             .GetComponent<TextMeshProUGUI>().text = item.itemInfo;

        itemToolTip.transform.Find("Skill")
                             .Find("SkillImage")
                             .GetComponent<Image>().sprite = item.skillImage;

        itemToolTip.transform.Find("Skill")
                             .Find("Name&Info")
                             .Find("Name").GetComponent<TextMeshProUGUI>().text = item.skillName;

        itemToolTip.transform.Find("Skill")
                             .Find("Name&Info")
                             .Find("Info").GetComponent<TextMeshProUGUI>().text = item.skillInfo;

        itemToolTip.transform.Find("Price")
                             .Find("CoinCount")
                             .GetComponent<TextMeshProUGUI>().text = item.price.ToString();
    }
    public void ShowToolTip(Vector3 position, bool isEquippedSlot)
    {
        if (isEquippedSlot)
            itemToolTip.gameObject.GetComponent<RectTransform>().pivot = new Vector2(1, 1);
        else
            itemToolTip.gameObject.GetComponent<RectTransform>().pivot = new Vector2(1, 0);

        itemToolTip.gameObject.SetActive(true);
        itemToolTip.transform.position = position;
    }

    public void HideToolTip()
    {
        itemToolTip.gameObject.SetActive(false);
    } 
    #endregion
}
