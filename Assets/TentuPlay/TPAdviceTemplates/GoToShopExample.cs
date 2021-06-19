using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToShopExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToShop()
    {
        string itemId = gameObject.GetComponent<itemIdViewer>().itemId;
        Debug.Log("itemId: " + itemId);
    }

}
