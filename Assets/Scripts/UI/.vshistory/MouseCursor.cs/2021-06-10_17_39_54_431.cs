using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public static bool isAimCursorNeeded = false;

    //마우스 포인터로 사용할 텍스처를 입력받습니다.
    public Texture2D cursorTexture;

    Vector2 _hotSpot;

    void Start()
    {

        //   if (hotSpotIsCenter)
        {
            _hotSpot.x = cursorTexture.width / 2;
            _hotSpot.y = cursorTexture.height / 2;
        }
        //    else
        {
            //중심좌표로 사용하지 않을 경우 Adjust Hot Spot으로 입력 받은 것을 사용합니다.

            //       hotSpot = adjustHotSpot;
        }

        Cursor.SetCursor(cursorTexture, _hotSpot, CursorMode.Auto);  //마우스 커서를 화면에 표시
    }

    private void Update()
    {
        if (isAimCursorNeeded)
            Cursor.SetCursor(cursorTexture, _hotSpot, CursorMode.Auto);  //마우스 커서를 화면에 표시
        else
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}


