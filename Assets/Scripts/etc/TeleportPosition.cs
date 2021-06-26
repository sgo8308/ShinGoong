using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPosition : MonoBehaviour
{
    bool isInvokeStart;
    void Update()
    {
        if (!transform.parent && !isInvokeStart)
        {
            Invoke("DestroyThis", 0.1f);
            isInvokeStart = true;
        }
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
