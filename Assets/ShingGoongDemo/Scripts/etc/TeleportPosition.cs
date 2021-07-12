using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 텔레포트 화살을 쏘고 텔레포트 하는 곳의 position
/// </summary>
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
