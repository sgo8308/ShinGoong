﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIOpener : MonoBehaviour
{
    protected PlayerMove playerMove;
    protected PlayerAttack playerAttack;
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
        playerMove.animator.SetBool("isIdle", true);
        playerMove.SetCanMove(false);
        playerAttack.SetCanShoot(false);
        SoundManager.instance.MutePlayerSound();
    }


    protected virtual void Close()
    {
        Invoke("SetIsOpenedFalse", 0.5f);
        playerMove.animator.SetBool("isIdle", false);
        playerMove.SetCanMove(true);
        playerAttack.SetCanShoot(true);
        SoundManager.instance.UnMutePlayerSound();
    }

    private void SetIsOpenedFalse()
    {
        isOpened = false;
    }
}
