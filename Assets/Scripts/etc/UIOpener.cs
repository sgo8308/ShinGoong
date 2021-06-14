using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIOpener : MonoBehaviour
{
    PlayerMove playerMove;
    PlayerAttack playerAttack;
    public static bool isOpened;

    protected virtual void Start()
    {
        playerMove = GameObject.Find("Player").GetComponent<PlayerMove>();
        playerAttack = GameObject.Find("Player").GetComponent<PlayerAttack>();
    }
    protected virtual void Open()
    {
        if (isOpened)
            return;

        isOpened = true;
        playerMove.SetCanMove(false);
        playerAttack.SetCanShoot(false);
    }

    protected virtual void Close()
    {
        isOpened = false;
        playerMove.SetCanMove(true);
        playerAttack.SetCanShoot(true);
    }
}
