using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMove : MonoBehaviour
{
    public InventoryOpener inventoryOpener;

    public Player player;

    public PlayerInfo playerInfo;

    public bool canMove = true;

    private void Start()
    {
        inventoryOpener.onInventoryOpened += () => canMove = false;
        player.onPlayerDead += () => canMove = false;
        
        inventoryOpener.onInventoryClosed += () => canMove = true;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += 
            (Scene scene, LoadSceneMode mode) => canMove = true;
    }
}
