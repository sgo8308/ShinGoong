using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject camera;
    float yAxis;
    private void Start()
    {
        yAxis = camera.transform.position.y;
    }
    void Update()
    {
        camera.transform.position.y = yAxis
    }
}
