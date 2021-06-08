using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public static bool isAimCursorNeeded = false;
    //마우스 포인터로 사용할 텍스처를 입력받습니다.
    public Texture2D cursorTexture;

    //텍스처의 중심을 마우스 좌표로 할 것인지 체크박스로 입력받습니다.
    public bool hotSpotIsCenter = false;

    //텍스처의 어느부분을 마우스의 좌표로 할 것인지 텍스처의 좌표를 입력받습니다.
    
    public Vector2 adjustHotSpot = Vector2.zero;

    //내부에서 사용할 필드를 선업합니다.
    private Vector2 hotSpot;

    public void Start()
    {
        if (hotSpotIsCenter)
        {
            hotSpot.x = cursorTexture.width / 2;
            hotSpot.y = cursorTexture.height / 2;
        }
        else
        {
            //중심좌표로 사용하지 않을 경우 Adjust Hot Spot으로 입력 받은 것을 사용합니다.
            hotSpot = adjustHotSpot;
        }
    }

    //private void Update()
    //{
    //    if (isAimCursorNeeded)
    //        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);  //마우스 커서를 화면에 표시
    //    else
    //        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    //}
}


