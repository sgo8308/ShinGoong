using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Image itemToolTip;
    
    public void ShowToolTip()
    {
        itemToolTip.gameObject.SetActive(true);
    }
}
