using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    SceneManager _sceneManager;
    bool _isLoadingNextScene;
    public string sceneName;

    private void Start()
    {
        if (GameObject.Find("SceneManager") != null)
            _sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

        _isLoadingNextScene = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.F) && !_isLoadingNextScene) 
        {
            _isLoadingNextScene = true;
            _sceneManager.GoTo(sceneName);
        }
    }
}
