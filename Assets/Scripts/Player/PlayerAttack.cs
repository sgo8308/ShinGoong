﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static float gaugePower = 0.0f;
    public static float nowPowerOfArrow = 0.0f;

    public GameObject arrowPrefab = null;
    public GameObject teleportArrowPrefab = null;
    public GameObject teleportArrowImage = null;
    public GameObject teleportEffect = null;
    private GameObject player = null;
    private PlayerMove playerMove;
    private PlayerSkill playerSkill;

    public float arrowSpeed = 50f;    //화살 속도
    public float arrowMaxPower = 1f;    //화살 Max Power


    private Animator animator;
    private Camera mainCamera;
    private Transform arrowDirection = null;
    private Vector2 mousePosition;

    private bool canShoot = true;

    float aimAngle;
    float ReAimAngle;

    bool aiming;
    bool angleChange;
    int currnetAngleType;
    int ReAimAngleType;


    Sprite[] sprites2;
    Sprite[] sprites3;
    SpriteRenderer spriteReAim;

    bool isAttacking;
    public bool canGuageBarFill;
    public bool isTeleportArrowOn = false;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerMove = player.GetComponent<PlayerMove>();
        playerSkill = player.GetComponent<PlayerSkill>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        arrowDirection = transform.Find("ArrowDirection");

        ImageSet();
        spriteReAim = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!canShoot || playerMove.isJumping || Inventory.instance.GetArrowCount() <= 0 || UIOpener.isOpened)
            return;

        SetArrowDirection();

        AttackReady();

        if (Input.GetMouseButtonDown(1))
        {
            isAttacking = false;
            AimCancel();
        }

        if (Input.GetKeyDown(KeyCode.R) && !playerSkill.IsSkillOn())
        {
            if (!isTeleportArrowOn)
            {
                isTeleportArrowOn = true;
                teleportArrowImage.SetActive(true);
            }
            else
            {
                isTeleportArrowOn = false;
                teleportArrowImage.SetActive(false);
            }

            SoundManager.instance.PlayClickSound();
        }

        if (teleportPosition != null && isTeleportArrowOn && teleportArrow.isStop && !Player.isDead)
        {
            Teleport();
        }
    }

    public void SetCanShoot(bool value)
    {
        this.canShoot = value;
    }

    public float timeG;
    private void AttackReady()
    {
        if (Input.GetMouseButtonDown(0))  //down -> ready애니메이션 시작
        {
            animator.enabled = true;
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", false);

            CancelInvoke("SetTrueCanGuageBarFill");
            canGuageBarFill = false;
            isAttacking = true;

            playerMove.StopPlayer();
            playerMove.FlipPlayer();
            playerMove.SetCanMove(false);

            CalculateBowAngle();
            animator.SetBool("isReady", true);
            AimCancel2();
            CancelInvoke("AimCancel");
            CancelInvoke("ReadyToAim");  
            Invoke("ReadyToAim", 0.7f);  //0.4초 후에 준비자세에서 조준자세로 바꿔준다.

            SoundManager.instance.PlayPlayerSound(PlayerSounds.PLAYER_READY_ARROW);

            gaugePower = 0.0f;
        }

        if (Input.GetMouseButton(0) && !animator.GetBool("isRunning") && isAttacking)
        {
            if (!canGuageBarFill)
                Invoke("SetTrueCanGuageBarFill", timeG);

            playerMove.StopPlayer();
            playerMove.FlipPlayer();
            playerMove.SetCanMove(false);

            if (canGuageBarFill)
                ControlPower();

            ReAiming();
        }

        if (Input.GetMouseButtonUp(0) && !animator.GetBool("isRunning") && isAttacking)  //up -> 0.2초 뒤에 angle애니메이션 취소
        {

            isAttacking = false;

            if (canGuageBarFill)
            {
                nowPowerOfArrow = gaugePower;

                if(isTeleportArrowOn)
                    Invoke("ShootTeleportArrow", 0.1f);
                else
                    Invoke("ShootArrow", 0.1f);

                FireFinish();

                SoundManager.instance.PlayPlayerSound(PlayerSounds.PLAYER_SHOOT_ARROW);

                Invoke("AimCancel", 0.5f);
            }
            else
            {
                AimCancel();
            }
        }
    }

    private void CalculateBowAngle()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); //스크린상의 마우스좌표 -> 게임상의 2d 좌표로 치환
        Vector2 t_direction = new Vector2(mousePosition.x - arrowDirection.position.x,
                                          mousePosition.y - arrowDirection.position.y);   //마우스 좌표 - 화살 좌표 = 바라볼 방향

        aimAngle = Mathf.Atan2(t_direction.y, t_direction.x) * Mathf.Rad2Deg;   //조준하고 있는 각도 세타 구하기


        if (aimAngle >= 0)
        {
            aimAngle = Mathf.Abs(90 - aimAngle);
        }
        else if (aimAngle < 0 && aimAngle >= -90)
        {
            aimAngle = Mathf.Abs(aimAngle - 90);

        }
        else if (aimAngle < -90 && aimAngle >= -180)
        {
            aimAngle = 270 - Mathf.Abs(aimAngle);
        }
    }

    private void CalculateBowAngle_ReAim()
    {
        Vector2 t_mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition); //스크린상의 마우스좌표 -> 게임상의 2d 좌표로 치환
        Vector2 t_direction = new Vector2(t_mousePos.x - arrowDirection.position.x,
                                          t_mousePos.y - arrowDirection.position.y);   //마우스 좌표 - 화살 좌표 = 바라볼 방향

        ReAimAngle = Mathf.Atan2(t_direction.y, t_direction.x) * Mathf.Rad2Deg;   //조준하고 있는 각도 세타 구하기
                                                                                  // print("재조준각도1: " + ReAimAngle);
        if (ReAimAngle >= 0)
        {
            ReAimAngle = Mathf.Abs(90 - ReAimAngle);
        }
        else if (ReAimAngle < 0 && ReAimAngle >= -90)
        {
            ReAimAngle = Mathf.Abs(ReAimAngle - 90);

        }
        else if (ReAimAngle < -90 && ReAimAngle >= -180)
        {
            ReAimAngle = 270 - Mathf.Abs(ReAimAngle);
        }

        if (ReAimAngle < 20)  //20도 미만 일때 재조준각 타입 정하기
        {
            ReAimAngleType = 20;
        }
        if (ReAimAngle >= 20)   //20도 이상 일때 재조준각 타입 정하기
        {
            ReAimAngleType = (int)(Mathf.Floor(ReAimAngle / 10) + 0.5) * 10;
        }

        if (currnetAngleType != ReAimAngleType)   //재조준 해서 각도타입이 달라졌을 때
        {
            angleChange = true;
        }

        if (currnetAngleType == ReAimAngleType)  //재조준 하였지만 다시 원래 각도 타입로 돌아왔을 때
        {
            angleChange = false;
        }
    }

    private void ReadyToAim()  //Ready애니메이션 끝나자 마자 Aiming애니메이션 시작
    {
        animator.SetBool("isReady", false);

        if (aimAngle >= 0 && aimAngle < 25) //마우스 각도가 0~25도 일때 Aiming20 애니메이션 시작
        {
            animator.SetBool("isAiming20", true);
            currnetAngleType = 20;
        }
        if (aimAngle >= 25 && aimAngle < 35)
        {
            animator.SetBool("isAiming30", true);
            currnetAngleType = 30;

        }
        if (aimAngle >= 35 && aimAngle < 45)
        {
            animator.SetBool("isAiming40", true);
            currnetAngleType = 40;

        }
        if (aimAngle >= 45 && aimAngle < 55)
        {
            animator.SetBool("isAiming50", true);
            currnetAngleType = 50;
        }
        if (aimAngle >= 55 && aimAngle < 65)
        {
            animator.SetBool("isAiming60", true);
            currnetAngleType = 60;
        }
        if (aimAngle >= 65 && aimAngle < 75)
        {
            animator.SetBool("isAiming70", true);
            currnetAngleType = 70;
        }
        if (aimAngle >= 75 && aimAngle < 85)
        {
            animator.SetBool("isAiming80", true);
            currnetAngleType = 80;
        }
        if (aimAngle >= 85 && aimAngle < 95)
        {
            animator.SetBool("isAiming90", true);
            currnetAngleType = 90;
        }
        if (aimAngle >= 95 && aimAngle < 105)
        {
            animator.SetBool("isAiming100", true);
            currnetAngleType = 100;
        }
        if (aimAngle >= 105 && aimAngle < 115)
        {
            animator.SetBool("isAiming110", true);
            currnetAngleType = 110;
        }
        if (aimAngle >= 115 && aimAngle < 125)
        {
            animator.SetBool("isAiming120", true);
            currnetAngleType = 120;
        }
        if (aimAngle >= 125)
        {
            animator.SetBool("isAiming130", true);
            currnetAngleType = 130;
        }
        aiming = true;
    }

    private void FireFinish()
    {
        animator.enabled = false;

        //각도별 조건 달기
        if (currnetAngleType == 20)
        {
            spriteReAim.sprite = sprites3[0];
        }
        if (currnetAngleType == 30)
        {
            spriteReAim.sprite = sprites3[1];
        }
        if (currnetAngleType == 40)
        {
            spriteReAim.sprite = sprites3[2];
        }
        if (currnetAngleType == 50)
        {
            spriteReAim.sprite = sprites3[3];
        }
        if (currnetAngleType == 60)
        {
            spriteReAim.sprite = sprites3[4];
        }
        if (currnetAngleType == 70)
        {
            spriteReAim.sprite = sprites3[5];
        }
        if (currnetAngleType == 80)
        {
            spriteReAim.sprite = sprites3[6];
        }
        if (currnetAngleType == 90)
        {
            spriteReAim.sprite = sprites3[7];
        }
        if (currnetAngleType == 100)
        {
            spriteReAim.sprite = sprites3[8];
        }
        if (currnetAngleType == 110)
        {
            spriteReAim.sprite = sprites3[9];
        }
        if (currnetAngleType == 120)
        {
            spriteReAim.sprite = sprites3[10];
        }
        if (currnetAngleType == 130)
        {
            spriteReAim.sprite = sprites3[11];
        }


        aiming = false;
        angleChange = false;
    }


    void AimCancel()
    {
        animator.enabled = true;
        animator.SetBool("isIdle", true);

        animator.SetBool("isAiming20", false);
        animator.SetBool("isAiming30", false);
        animator.SetBool("isAiming40", false);
        animator.SetBool("isAiming50", false);
        animator.SetBool("isAiming60", false);
        animator.SetBool("isAiming70", false);
        animator.SetBool("isAiming80", false);
        animator.SetBool("isAiming90", false);
        animator.SetBool("isAiming100", false);
        animator.SetBool("isAiming110", false);
        animator.SetBool("isAiming120", false);
        animator.SetBool("isAiming130", false);

        animator.SetBool("isReady", false);

        playerMove.SetCanMove(true);

        aiming = false;
        angleChange = false;

        SoundManager.instance.StopPlayerSound();
    }

    void AimCancel2()
    {
        animator.SetBool("isAiming20", false);
        animator.SetBool("isAiming30", false);
        animator.SetBool("isAiming40", false);
        animator.SetBool("isAiming50", false);
        animator.SetBool("isAiming60", false);
        animator.SetBool("isAiming70", false);
        animator.SetBool("isAiming80", false);
        animator.SetBool("isAiming90", false);
        animator.SetBool("isAiming100", false);
        animator.SetBool("isAiming110", false);
        animator.SetBool("isAiming120", false);
        animator.SetBool("isAiming130", false);
    }

    private void ReAiming()  //마우스를 누르고 있을 때
    {
        if (aiming)
        {
            CalculateBowAngle_ReAim(); //재조준 활 각도계산

            if (angleChange)
            {
                if (ReAimAngleType == 20)
                {
                    currnetAngleType = 20;

                    animator.enabled = false;

                    spriteReAim.sprite = sprites2[0];
                }


                if (ReAimAngleType == 30)
                {
                    currnetAngleType = 30;

                    animator.enabled = false;

                    spriteReAim.sprite = sprites2[1];

                }

                if (ReAimAngleType == 40)
                {
                    currnetAngleType = 40;

                    animator.enabled = false;

                    spriteReAim.sprite = sprites2[2];
                }

                if (ReAimAngleType == 50)
                {
                    currnetAngleType = 50;

                    animator.enabled = false;

                    spriteReAim.sprite = sprites2[3];
                }
                if (ReAimAngleType == 60)
                {
                    currnetAngleType = 60;

                    animator.enabled = false;

                    spriteReAim.sprite = sprites2[4];
                }
                if (ReAimAngleType == 70)
                {
                    currnetAngleType = 70;

                    animator.enabled = false;

                    spriteReAim.sprite = sprites2[5];
                }
                if (ReAimAngleType == 80)
                {
                    currnetAngleType = 80;

                    animator.enabled = false;

                    spriteReAim.sprite = sprites2[6];
                }
                if (ReAimAngleType == 90)
                {
                    currnetAngleType = 90;

                    animator.enabled = false;

                    spriteReAim.sprite = sprites2[7];
                }
                if (ReAimAngleType == 100)
                {
                    currnetAngleType = 100;

                    animator.enabled = false;

                    spriteReAim.sprite = sprites2[8];
                }
                if (ReAimAngleType == 110)
                {
                    currnetAngleType = 110;

                    animator.enabled = false;

                    spriteReAim.sprite = sprites2[9];
                }
                if (ReAimAngleType == 120)
                {
                    currnetAngleType = 120;

                    animator.enabled = false;

                    spriteReAim.sprite = sprites2[10];
                }
                if (ReAimAngleType == 130)
                {
                    currnetAngleType = 130;

                    animator.enabled = false;

                    spriteReAim.sprite = sprites2[11];
                }
            }


        }
    }


    private void SetArrowDirection()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); //스크린상의 마우스좌표 -> 게임상의 2d 좌표로 치환
        Vector2 direction = new Vector2(mousePosition.x - arrowDirection.transform.position.x,
                                          mousePosition.y - arrowDirection.transform.position.y);   //마우스 좌표 - 화살 좌표 = 바라볼 방향

        arrowDirection.transform.right = direction;  //화살의 x축 방향을 '바라볼 방향'으로 정한다
    }

    private void ControlPower()
    {
        gaugePower += Time.deltaTime * arrowSpeed;

        if (gaugePower > arrowMaxPower)
            gaugePower = arrowMaxPower;

        MainUI.instance.UpdateGaugeBarUI(gaugePower, arrowMaxPower);
    }

    private void ShootArrow()
    {
        GameObject t_arrow = Instantiate(arrowPrefab, arrowDirection.transform.position, arrowDirection.transform.rotation); //화살 생성
        t_arrow.GetComponent<Rigidbody2D>().velocity = t_arrow.transform.right * nowPowerOfArrow;  //화살 발사 속도 = x축 방향 * 파워 * 속도값

        if (gaugePower >= arrowMaxPower)
        {
            t_arrow.GetComponent<Rigidbody2D>().gravityScale = 0; //Max Power일때 직사로 발사된다. 중력 0
            t_arrow.GetComponent<Rigidbody2D>().velocity = t_arrow.transform.right * nowPowerOfArrow;  //화살 발사 속도 = x축 방향 * 파워 * 속도값
        }

        Inventory.instance.UseArrow();
        MainUI.instance.UpdateArrowCountUI();
    }

    public GameObject nowFlyingTeleportArrow;
    public Transform teleportPosition;
    public TeleportArrow teleportArrow;
    private void ShootTeleportArrow()
    {
        nowFlyingTeleportArrow = Instantiate(teleportArrowPrefab, arrowDirection.transform.position, arrowDirection.transform.rotation); //화살 생성
        nowFlyingTeleportArrow.GetComponent<Rigidbody2D>().velocity = nowFlyingTeleportArrow.transform.right * nowPowerOfArrow * 4 / 5;  //화살 발사 속도 = x축 방향 * 파워 * 속도값

        nowFlyingTeleportArrow.GetComponent<Rigidbody2D>().gravityScale = 2; //Max Power일때 직사로 발사된다. 중력 0

        if (gaugePower >= arrowMaxPower)
        {
            nowFlyingTeleportArrow.GetComponent<Rigidbody2D>().gravityScale = 0; //Max Power일때 직사로 발사된다. 중력 0
            nowFlyingTeleportArrow.GetComponent<Rigidbody2D>().velocity = nowFlyingTeleportArrow.transform.right * nowPowerOfArrow * 1 / 3;  //화살 발사 속도 = x축 방향 * 파워 * 속도값
        }

        Inventory.instance.UseArrow();
        MainUI.instance.UpdateArrowCountUI();

        teleportArrow = nowFlyingTeleportArrow.GetComponent<TeleportArrow>();
        teleportPosition = nowFlyingTeleportArrow.transform.Find("TeleportPosition");
    }

    private void Teleport()
    {
        Vector3 teleportPos;
        if (teleportPosition.rotation.z >= 0)
        {
            teleportPos = new Vector3(teleportPosition.position.x, teleportPosition.position.y, 0);
        }
        else
        {
            teleportPos = new Vector3(teleportPosition.position.x, teleportPosition.position.y + 2.0f, 0);
        }

        transform.position = teleportPos;
        isTeleportArrowOn = false;
        teleportArrowImage.SetActive(false);
        teleportEffect.SetActive(true);
        Invoke("HideTeleportEffect", 0.5f);

        SoundManager.instance.PlayNonPlayerSound(NonPlayerSounds.ARROW_TELEPORT);

        Destroy(teleportPosition.gameObject);
    }

    
    private void ImageSet()
    {
        sprites2 = Resources.LoadAll<Sprite>("Sprites/Player/FireAngle_anim/Angle2");

        sprites3 = Resources.LoadAll<Sprite>("Sprites/Player/FireAngle_anim/Angle3");
    }

    private void SetTrueCanGuageBarFill()
    {
        canGuageBarFill = true;
    }

    private void HideTeleportEffect()
    {
        teleportEffect.SetActive(false);
    }

}
