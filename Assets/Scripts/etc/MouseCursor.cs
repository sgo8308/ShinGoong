using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    //마우스 포인터로 사용할 텍스처를 입력받습니다.
    public Texture2D cursorTexture;

    Vector2 _hotSpot;

    void Start()
    {
        _hotSpot.x = cursorTexture.width / 2;
        _hotSpot.y = cursorTexture.height / 2;
    }

    private void Update()
    {
        if (IsAimCursorNeeded())
            Cursor.SetCursor(cursorTexture, _hotSpot, CursorMode.Auto);  //마우스 커서를 화면에 표시
        else
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public GameObject inventoryPanel;

    public GameObject gameOverPanel;

    public GameObject mainMenuPanel;

    private bool IsAimCursorNeeded()
    {
        bool isAimCursorNeeded = true;

        if (inventoryPanel.activeSelf || gameOverPanel.activeSelf || mainMenuPanel.activeSelf)
            isAimCursorNeeded = false;

        return isAimCursorNeeded;
    }
}


