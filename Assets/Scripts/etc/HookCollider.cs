using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCollider : MonoBehaviour
{

    public static bool ropePull = false;

    public GameObject ropeArrow;

    public static HookCollider instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("충돌123");
        ropePull = true;
       
    }
}
