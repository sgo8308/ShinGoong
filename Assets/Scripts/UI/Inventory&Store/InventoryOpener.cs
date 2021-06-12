using UnityEngine;

public class InventoryOpener : MonoBehaviour
{
    public GameObject inventoryPanel;

    public GameObject storePanel;

    public PlayerMove playerMove;

    public ItemToolTipOpener itemToolTipOpener;

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
        playerMove.SetCanMove(false);
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        playerMove.SetCanMove(true);
        itemToolTipOpener.CloseToolTip();
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
