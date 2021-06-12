using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemToolTipOpener : MonoBehaviour
{
    public void OpenToolTip(Vector3 position, SlotType slotType)
    {
        switch (slotType)
        {
            case SlotType.Inventory:
                gameObject.GetComponent<RectTransform>().pivot = new Vector2(1, 0);
                break;

            case SlotType.Equipped:
                gameObject.GetComponent<RectTransform>().pivot = new Vector2(1, 1);
                break;

            case SlotType.StoreTopSide:
                gameObject.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
                break;

            case SlotType.StoreBottomSide:
                gameObject.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
                break;

            default:
                break;
        }

        gameObject.SetActive(true);
        transform.position = position;
    }

    public void CloseToolTip()
    {
        gameObject.SetActive(false);
    }
}
