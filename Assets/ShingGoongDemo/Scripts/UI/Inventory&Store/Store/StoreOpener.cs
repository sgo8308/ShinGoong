using UnityEngine;

public class StoreOpener : UIOpener
{
    public GameObject storePanel;
    public GameObject mainMenuPanel;

    public delegate void OnStoreOpened();
    public OnStoreOpened onStoreOpened;

    public delegate void OnStoreClosed();
    public OnStoreOpened onStoreClosed;


    bool isPlayerInTrigger;


    protected override void Start()
    {
        base.Start();
        StageManager.instance.InitializeStore();
    }

    //Interact with store
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;

        isPlayerInTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerInTrigger = false;
    }

    private void Update()
    {
        if (mainMenuPanel.activeSelf)
            return;

        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.F) && !storePanel.activeSelf)
        {
            Open();
        }

        if (Input.GetKeyUp(KeyCode.Escape) && storePanel.activeSelf)
        {
            Close();
        }
    }

    protected override void Open()
    {
        if (playerMove.isJumping)
            return;
        
        base.Open();
        storePanel.SetActive(true);
        isOpened = false;
        onStoreOpened.Invoke();
    }

    protected override void Close()
    {
        base.Close();
        storePanel.SetActive(false);
        onStoreClosed.Invoke();
    }
}
