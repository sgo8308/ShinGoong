using UnityEngine;

/// <summary>
/// 마우스 커서는 조준점으로 바꿔주는 스크립트
/// </summary>
public class MouseCursor : MonoBehaviour
{
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
            Cursor.SetCursor(cursorTexture, _hotSpot, CursorMode.Auto);
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


