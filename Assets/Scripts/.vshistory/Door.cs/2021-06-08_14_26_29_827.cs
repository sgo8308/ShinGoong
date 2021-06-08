using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    SceneManager _sceneManager;
    bool isLoadingNextScene;
    public string sceneName;
    private void Start()
    {
        if (GameObject.Find("SceneManager") != null)
            _sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

        isLoadingNextScene = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.F) && !isLoadingNextScene) 
        {
            isLoadingNextScene = true;
            _sceneManager.GoTo(sceneName);
        }
    }
}
