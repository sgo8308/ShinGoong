using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemToolTip : MonoBehaviour
{
    private ItemToolTipUI ui;
    private ItemToolTipOpener opener;

    void Start()
    {
        ui = gameObject.GetComponent<ItemToolTipUI>();
        opener = gameObject.GetComponent<ItemToolTipOpener>();
    }

}
