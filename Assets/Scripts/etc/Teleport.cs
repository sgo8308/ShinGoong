using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public string sceneName;

    private bool _isLoadingNextScene;
    private bool isPlayerInTrigger;

    private void Start()
    {
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

            SoundManager.instance.PlayNonPlayerSound(NonPlayerSounds.TELEPORT);

            SceneManager.instance.GoTo(sceneName);
        }
    }
}
