﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoreSlot : MonoBehaviour , IPointerClickHandler
{
    public Item item;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            //Item item = this.GetComponent<> // 인덱스 호출해서 인벤토리 목록에서 그 인덱스 가져온다.
            ////만약 타입이 활이면 장착
            //switch (switch_on)
            //{
            //    default:
            //}
        }

    }
}
