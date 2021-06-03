using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestPa : MonoBehaviour
{
    UnityEvent event1;

    private void Start()
    {
        event1 = new UnityEvent(); 
    }

    void hello()
    {
        Debug.Log("hello");
    }
}
