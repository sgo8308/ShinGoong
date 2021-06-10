﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    SceneManager _sceneManager;
    bool _isLoadingNextScene;
    public string sceneName;
    bool isPlayerInTrigger;

    private void Start()
    {
        if (GameObject.Find("SceneManager") != null)
            _sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

        _isLoadingNextScene = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;

        isPlayerInTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerInTrigger = false;
    }

    private void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.F) && !_isLoadingNextScene)
        {
            _isLoadingNextScene = true;
            _sceneManager.GoTo(sceneName);
        }
    }
}
