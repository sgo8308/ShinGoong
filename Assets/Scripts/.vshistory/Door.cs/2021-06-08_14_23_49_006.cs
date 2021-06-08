using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    SceneManager sceneManager;
    bool isLoadingNextScene;
    public string sceneName;
    private void Start()
    {
        if (GameObject.Find("SceneManager") != null)
            sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

        isLoadingNextScene = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.F) && !isLoadingNextScene) 
        {
            isLoadingNextScene = true;
            sceneManager.GoTo(sceneName);
        }
    }
}
