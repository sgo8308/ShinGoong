using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    SceneManager sceneManager;
    bool isLoadingNextScene;
    private void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        isLoadingNextScene = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.F) && !isLoadingNextScene) 
        {
            isLoadingNextScene = true;
            sceneManager.GoTo("Stage1Scene");
        }
    }
}
