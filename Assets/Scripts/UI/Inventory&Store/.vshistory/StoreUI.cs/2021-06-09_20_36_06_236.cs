using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : MonoBehaviour
{
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        //ItemList에 있는 Item을 등록해준다.
        for (int i = 0; i < items.Count; i++)
        {
            GameObject slot = Instantiate(storeSlotPrefab, this.game);

        }
        Instantiate
    }
}
