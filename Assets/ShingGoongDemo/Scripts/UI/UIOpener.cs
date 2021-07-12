using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI들을 열고 닫을 때 쓰는 모든 Opener 스크립트의 부모 스크립트
/// </summary>
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
        playerMove.animator.SetBool("isRunning", false);
        playerMove.SetCanMove(false);
        playerAttack.SetCanShoot(false);

        SoundManager.instance.MutePlayerSound();
        SoundManager.instance.MutePlayerRunningSound();
    }


    protected virtual void Close()
    {
        Invoke("SetIsOpenedFalse", 0.5f);
        if (!Player.isDead)
        {
            playerMove.SetCanMove(true);
            playerAttack.SetCanShoot(true);
            SoundManager.instance.UnMutePlayerSound();
            SoundManager.instance.UnMutePlayerRunningSound();
        }
        playerMove.InitJumpValues();
        playerMove.animator.enabled = true;
        playerMove.animator.SetBool("isIdle", true);
        playerMove.animator.SetBool("isReady", false);
    }

    private void SetIsOpenedFalse()
    {
        isOpened = false;
    }
}
