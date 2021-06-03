using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testchild : TestPa
{
    void Start()
    {
        Invoke("hello", 3);
    }

    public void jiwoo()
    {
        Debug.Log("jiwoo");
    }
}
