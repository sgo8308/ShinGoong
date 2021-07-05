using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryOpener : UIOpener
{
    public GameObject inventoryPanel;

    public GameObject storePanel;

    public ItemToolTipOpener itemToolTipOpener;

    public GameObject mainMenuPanel;

    protected override void Start()
    {
        base.Start();

        StoreOpener storeOpener = GameObject.Find("StoreNpc").GetComponent<StoreOpener>();
        storeOpener.onStoreOpened += Open;
        storeOpener.onStoreClosed += Close;

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += RegisterToStoreEvent;

        inventoryPanel.SetActive(false);
    }

    private void Update()
    {
        if (mainMenuPanel.activeSelf)
            return;

        if (Input.GetKeyDown(KeyCode.V) && CanOpen())
        {
            if (inventoryPanel.activeSelf)
                Close();
            else
                Open();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && CanClose())
        {
            Close();
        }
    }

    protected override void Open()
    {
        if (!playerMove.isJumping)
        {
            if (isOpened)
                return;

            isOpened = true;
            playerMove.animator.SetBool("isRunning", false);
            playerMove.SetCanMove(false);
            playerAttack.SetCanShoot(false);
            SoundManager.instance.MutePlayerSound();
            SoundManager.instance.MutePlayerRunningSound();
            inventoryPanel.SetActive(true);
            SoundManager.instance.PlayNonPlayerSound(NonPlayerSounds.OPEN_INVENTORY);
        }
    }

    protected override void Close()
    {
        base.Close();
        inventoryPanel.SetActive(false);
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

    private void RegisterToStoreEvent(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ShelterScene")
        {
            StoreOpener storeOpener = GameObject.Find("StoreNpc").GetComponent<StoreOpener>();
            storeOpener.onStoreOpened += Open;
            storeOpener.onStoreClosed += Close;
        }
    }
}
