using UnityEngine;

public class InventoryOpener : MonoBehaviour
{
    public GameObject inventoryPanel;

    public GameObject storePanel;

    public delegate void OnInventoryOpened();
    public OnInventoryOpened onInventoryOpened;

    public delegate void OnInventoryClosed();
    public OnInventoryClosed onInventoryClosed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && CanOpen())
        {
            if (inventoryPanel.activeSelf)
                CloseInventory();
            else
                OpenInventory();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && CanClose())
        {
            CloseInventory();
        }
    }

    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        onInventoryOpened.Invoke();
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        onInventoryClosed.Invoke();

    }

    private bool CanOpen()
    {
        bool canOpen = true;

        if (StageManager.instance.stageState == StageState.UNCLEAR ||
                storePanel.activeSelf)
            canOpen = false;

        return canOpen;
    }

    private bool CanClose()
    {
        bool canClose = true;

        if (storePanel.activeSelf)
            canClose = false;

        return canClose;
    }
    
}
