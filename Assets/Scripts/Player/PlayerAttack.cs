using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static bool isRopeArrowMoving = false;

    public static float power = 0.0f;

    public GameObject arrowPrefab = null;
    public GameObject ropeArrowPrefab = null;

    private GameObject player = null;
    private PlayerMove playerMove;

    public float arrowSpeed = 50f;    //화살 속도
    public float arrowMaxPower = 1f;    //화살 Max Power
    public float ropeArrowSpeed = 15f;    //화살 속도


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

    Sprite[] sprites;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerMove = player.GetComponent<PlayerMove>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        arrowDirection = transform.Find("ArrowDirection");

        ImageSet();
    }

    private void Update()
    {
        if (!canShoot || playerMove.isJumping ||
                playerMove.isRopeMoving || Inventory.instance.GetArrowCount() <= 0 ||
                isRopeArrowMoving || UIOpener.isOpened)
            return;

        SetArrowDirection();

        if (Input.GetMouseButton(0))
        {
        //   playerMove.StopPlayer();
        //   playerMove.FlipPlayer();
        //   playerMove.SetCanMove(false);
        //   ControlPower();
        }

        if (Input.GetMouseButtonUp(0) && !Input.GetKey(KeyCode.R))
        {
         //   playerMove.SetCanMove(true);
         //   Invoke("ShootArrow", 0.1f);
        }

        if (Input.GetKey(KeyCode.R))
        {
            if (Input.GetMouseButtonDown(0))
            {
                ShootRopeArrow();
            }
        }

        AttackReady();
    }

    public void SetCanShoot(bool value)
    {
        this.canShoot = value;
    }

    private void AttackReady()
    {
        if (Input.GetMouseButtonDown(0) && !animator.GetBool("isRunning") && !animator.GetBool("isJumping") &&!Input.GetKey(KeyCode.E))  //down -> ready애니메이션 시작
        {
            playerMove.StopPlayer();
            playerMove.FlipPlayer();
            playerMove.SetCanMove(false);


            CalculateBowAngle();
            animator.SetBool("isReady", true);
            Invoke("ReadyToAim", 0.7f);  //0.7초 후에 준비자세에서 조준자세로 바꿔준다.

        }

        if (Input.GetMouseButton(0) && !animator.GetBool("isRunning") && !animator.GetBool("isJumping" ) && !Input.GetKey(KeyCode.E))
        {
            playerMove.StopPlayer();
            playerMove.FlipPlayer();
            playerMove.SetCanMove(false);
            ControlPower();


            ReAiming();

        }

        if (Input.GetMouseButtonUp(0) && !Input.GetKey(KeyCode.R) && !animator.GetBool("isRunning") && !animator.GetBool("isJumping") && !Input.GetKey(KeyCode.E))  //up -> 0.2초 뒤에 angle애니메이션 취소
        {
            print("마우스 up");

            playerMove.SetCanMove(true);
            Invoke("ShootArrow", 0.1f);

            FireFinish();
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


        print(aimAngle);
    }

    void CalculateBowAngle_ReAim()
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
            ReAimAngle = Mathf.Abs(  ReAimAngle - 90 );

        }else if (ReAimAngle < -90 && ReAimAngle >= -180)
        {
            ReAimAngle = 270 - Mathf.Abs(ReAimAngle );
        }



        if (ReAimAngle < 20)  //20도 미만 일때 재조준각 타입 정하기
        {
            ReAimAngleType = 20;
        }
        if (ReAimAngle >= 20 )   //20도 이상 일때 재조준각 타입 정하기
        {
            ReAimAngleType = (int)(Mathf.Floor(ReAimAngle / 10) + 1 ) * 10 ;
        }

        if (currnetAngleType != ReAimAngleType)   //재조준 해서 각도타입이 달라졌을 때
        {
          //  print("aim 각도type : " + currnetAngleType);
          //  print("재조준 각도type : " + ReAimAngleType);
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

        if (aimAngle >= 0 && aimAngle < 20) //마우스 각도가 0~20도 일때 Aiming20 애니메이션 시작
        {
            animator.SetBool("isAiming20", true);
            currnetAngleType = 20;
        }
        if (aimAngle >= 20 && aimAngle < 30)
        {
            animator.SetBool("isAiming30", true);
            currnetAngleType = 30;
        }
        if (aimAngle >= 30 && aimAngle < 40)
        {
            animator.SetBool("isAiming40", true);
            currnetAngleType = 40;
        }
        if (aimAngle >= 40 && aimAngle < 50)
        {
            animator.SetBool("isAiming50", true);
            currnetAngleType = 50;
        }
        if (aimAngle >= 50 && aimAngle < 60)
        {
            animator.SetBool("isAiming60", true);
            currnetAngleType = 60;
        }
        if (aimAngle >= 60 && aimAngle < 70)
        {
            animator.SetBool("isAiming70", true);
            currnetAngleType = 70;
        }
        if (aimAngle >= 70 && aimAngle < 80)
        {
            animator.SetBool("isAiming80", true);
            currnetAngleType = 80;
        }
        if (aimAngle >= 80 && aimAngle < 90)
        {
            animator.SetBool("isAiming90", true);
            currnetAngleType = 90;
        }
        if (aimAngle >= 90 && aimAngle < 100)
        {
            animator.SetBool("isAiming100", true);
            currnetAngleType = 100;
        }
        if (aimAngle >= 100 && aimAngle < 110)
        {
            animator.SetBool("isAiming110", true);
            currnetAngleType = 110;
        }
        if (aimAngle >= 110 && aimAngle < 120)
        {
            animator.SetBool("isAiming120", true);
            currnetAngleType = 120;
        }
        if (aimAngle >= 120 )
        {
            animator.SetBool("isAiming130", true);
            currnetAngleType = 130;
        }
       
        aiming = true;

    }

    private void FireFinish()
    {
        //각도별 조건 달기
        if (currnetAngleType == 20)
        {
            animator.SetBool("isAiming20", false);
            ReAimCancel();
            animator.SetBool("isFireFinish20", true);  // FireFinish 애니메이션 시작 = 발사!!
        }
        if (currnetAngleType == 30)
        {
            animator.SetBool("isAiming30", false);
            ReAimCancel();
            animator.SetBool("isFireFinish30", true);  // FireFinish 애니메이션 시작 = 발사!!
        }
        if (currnetAngleType == 40)
        {
            animator.SetBool("isAiming40", false);
            ReAimCancel();
            animator.SetBool("isFireFinish40", true);  // FireFinish 애니메이션 시작 = 발사!!
        }
        if (currnetAngleType == 50)
        {
            animator.SetBool("isAiming50", false);
            ReAimCancel();
            animator.SetBool("isFireFinish50", true);  // FireFinish 애니메이션 시작 = 발사!!
        }
        if (currnetAngleType == 60)
        {
            animator.SetBool("isAiming60", false);
            ReAimCancel();
            animator.SetBool("isFireFinish60", true);  // FireFinish 애니메이션 시작 = 발사!!
        }
        if (currnetAngleType == 70)
        {
            animator.SetBool("isAiming70", false);
            ReAimCancel();
            animator.SetBool("isFireFinish70", true);  // FireFinish 애니메이션 시작 = 발사!!
        }
        if (currnetAngleType == 80)
        {
            animator.SetBool("isAimin80", false);
            ReAimCancel();
            animator.SetBool("isFireFinish80", true);  // FireFinish 애니메이션 시작 = 발사!!
        }
        if (currnetAngleType == 90)
        {
            animator.SetBool("isAiming90", false);
            ReAimCancel();
            animator.SetBool("isFireFinish90", true);  // FireFinish 애니메이션 시작 = 발사!!
        }
        if (currnetAngleType == 100)
        {
            animator.SetBool("isAiming100", false);
            ReAimCancel();
            animator.SetBool("isFireFinish100", true);  // FireFinish 애니메이션 시작 = 발사!!
        }
        if (currnetAngleType == 110)
        {
            animator.SetBool("isAiming110", false);
            ReAimCancel();
            animator.SetBool("isFireFinish110", true);  // FireFinish 애니메이션 시작 = 발사!!
        }
        if (currnetAngleType == 120)
        {
            animator.SetBool("isAiming120", false);
            ReAimCancel();
            animator.SetBool("isFireFinish120", true);  // FireFinish 애니메이션 시작 = 발사!!
        }
        if (currnetAngleType == 130)
        {
            animator.SetBool("isAiming130", false);
            ReAimCancel();
            animator.SetBool("isFireFinish130", true);  // FireFinish 애니메이션 시작 = 발사!!
        }





        Invoke("AttackToIdle", 0.2f);


        aiming = false;
        angleChange = false;

    }

    void ReAimCancel()
    {
        animator.SetBool("20to30", false);
        animator.SetBool("30to20", false);
        animator.SetBool("30to40", false);
        animator.SetBool("40to30", false);
        animator.SetBool("40to50", false);
        animator.SetBool("50to40", false);
        animator.SetBool("50to60", false);
        animator.SetBool("60to50", false);
        animator.SetBool("60to70", false);
        animator.SetBool("70to60", false);
        animator.SetBool("70to80", false);
        animator.SetBool("80to70", false);
        animator.SetBool("80to90", false);
        animator.SetBool("90to80", false);
        animator.SetBool("90to100", false);
        animator.SetBool("100to90", false);
        animator.SetBool("100to110", false);
        animator.SetBool("110to100", false);
        animator.SetBool("110to120", false);
        animator.SetBool("120to110", false);
        animator.SetBool("120to130", false);
        animator.SetBool("130to120", false);   

    }

    private void AttackToIdle()
    {
        animator.SetBool("isFireFinish20", false);
        animator.SetBool("isFireFinish30", false);
        animator.SetBool("isFireFinish40", false);
        animator.SetBool("isFireFinish50", false);
        animator.SetBool("isFireFinish60", false);
        animator.SetBool("isFireFinish70", false); 
        animator.SetBool("isFireFinish80", false);
        animator.SetBool("isFireFinish90", false);
        animator.SetBool("isFireFinish100", false);
        animator.SetBool("isFireFinish110", false);
        animator.SetBool("isFireFinish120", false);
        animator.SetBool("isFireFinish130", false);
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
                    print("여기 들어와? 20");
                    animator.SetBool("isAiming30", false);

                    animator.SetBool("20to30", false);

                    animator.SetBool("30to20", true);

                    currnetAngleType = 20;
                    angleChange = false;

                }


                if (ReAimAngleType == 30)
                {
                    print("여기 들어와? 30");
                    animator.SetBool("isAiming20", false);
                    animator.SetBool("isAiming40", false);

                    animator.SetBool("30to20", false);
                    animator.SetBool("30to40", false);

                    animator.SetBool("20to30", true);
                    animator.SetBool("40to30", true);


                    currnetAngleType = 30;
                    angleChange = false;

                }

                if (ReAimAngleType == 40)
                {
                    print("여기 들어와? 40");
                    animator.SetBool("isAiming30", false);
                    animator.SetBool("isAiming50", false);

                    animator.SetBool("40to30", false);
                    animator.SetBool("40to50", false);

                    animator.SetBool("30to40", true);
                    animator.SetBool("50to40", true);

                    currnetAngleType = 40;
                    angleChange = false;

                }
                if (ReAimAngleType == 50)
                {
                    print("여기 들어와? 50");
                    animator.SetBool("isAiming40", false);
                    animator.SetBool("isAiming60", false);

                    animator.SetBool("50to40", false);
                    animator.SetBool("50to60", false);

                    animator.SetBool("40to50", true);
                    animator.SetBool("60to50", true);

                    currnetAngleType = 50;
                    angleChange = false;

                }
                if (ReAimAngleType == 60)
                {
                    print("여기 들어와? 60");
                    animator.SetBool("isAiming50", false);
                    animator.SetBool("isAiming70", false);

                    animator.SetBool("60to50", false);
                    animator.SetBool("60to70", false);

                    animator.SetBool("50to60", true);
                    animator.SetBool("70to60", true);

                    currnetAngleType = 60;
                    angleChange = false;

                }
                if (ReAimAngleType == 70)
                {
                    print("여기 들어와? 70");
                    animator.SetBool("isAiming60", false);
                    animator.SetBool("isAiming80", false);

                    animator.SetBool("70to60", false);
                    animator.SetBool("70to80", false);

                    animator.SetBool("60to70", true);
                    animator.SetBool("80to70", true);

                    currnetAngleType = 70;
                    angleChange = false;

                }
                if (ReAimAngleType == 80)
                {
                    print("여기 들어와? 80");
                    animator.SetBool("isAiming70", false);
                    animator.SetBool("isAiming90", false);

                    animator.SetBool("80to70", false);
                    animator.SetBool("80to90", false);

                    animator.SetBool("70to80", true);
                    animator.SetBool("90to80", true);

                    currnetAngleType = 80;
                    angleChange = false;

                }
                if (ReAimAngleType == 90)
                {
                    print("여기 들어와? 90");
                    animator.SetBool("isAiming80", false);
                    animator.SetBool("isAiming100", false);

                    animator.SetBool("90to80", false);
                    animator.SetBool("90to100", false);

                    animator.SetBool("80to90", true);
                    animator.SetBool("100to90", true);

                    currnetAngleType = 90;
                    angleChange = false;

                }
                if (ReAimAngleType == 100)
                {
                    print("여기 들어와? 100");
                    animator.SetBool("isAiming90", false);
                    animator.SetBool("isAiming110", false);

                    animator.SetBool("100to90", false);
                    animator.SetBool("100to110", false);

                    animator.SetBool("90to100", true);
                    animator.SetBool("110to100", true);

                    currnetAngleType = 100;
                    angleChange = false;

                }
                if (ReAimAngleType == 110)
                {
                    print("여기 들어와? 110");
                    animator.SetBool("isAiming100", false);
                    animator.SetBool("isAiming120", false);

                    animator.SetBool("110to100", false);
                    animator.SetBool("110to120", false);

                    animator.SetBool("100to110", true);
                    animator.SetBool("120to110", true);

                    currnetAngleType = 110;
                    angleChange = false;

                }
                if (ReAimAngleType == 120)
                {
                    print("여기 들어와? 120");
                    animator.SetBool("isAiming110", false);
                    animator.SetBool("isAiming130", false);

                    animator.SetBool("120to110", false);
                    animator.SetBool("120to130", false);

                    animator.SetBool("110to120", true);
                    animator.SetBool("130to120", true);

                    currnetAngleType = 120;
                    angleChange = false;

                }
                if (ReAimAngleType == 130)
                {
                    print("여기 들어와? 130");
                    animator.SetBool("isAiming120", false);

                    animator.SetBool("130to120", false);
                    animator.SetBool("120to130", true);

                    currnetAngleType = 130;
                    angleChange = false;

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
        arrowDirection.transform.position = new Vector2(player.transform.position.x, player.transform.position.y ); // 플레이어 목 근처에서 화살이 나가게  y값 조정
    }

    private void ControlPower()
    {
        power += Time.deltaTime * arrowSpeed;

        MainUI.instance.UpdateGaugeBarUI(power, arrowMaxPower);

        if (power > arrowMaxPower)
            power = arrowMaxPower;
    }

    private void ShootArrow()
    {
        GameObject t_arrow = Instantiate(arrowPrefab, arrowDirection.transform.position, arrowDirection.transform.rotation); //화살 생성
        t_arrow.GetComponent<Rigidbody2D>().velocity = t_arrow.transform.right * power * 1 / 2;  //화살 발사 속도 = x축 방향 * 파워 * 속도값

        if (power >= arrowMaxPower)
        {
            t_arrow.GetComponent<Rigidbody2D>().gravityScale = 0; //Max Power일때 직사로 발사된다. 중력 0
            t_arrow.GetComponent<Rigidbody2D>().velocity = t_arrow.transform.right * power * 1 / 3;  //화살 발사 속도 = x축 방향 * 파워 * 속도값
        }

        power = 0.0f;

        Inventory.instance.UseArrow();
        MainUI.instance.UpdateArrowCountUI();
    }

    private void ShootRopeArrow()
    {
        GameObject RopeArrow = Instantiate(ropeArrowPrefab, arrowDirection.transform.position, arrowDirection.transform.rotation); //화살 생성
        RopeArrow.GetComponent<Rigidbody2D>().gravityScale = 0; //Max Power일때 직사로 발사된다. 중력 0
        RopeArrow.GetComponent<Rigidbody2D>().velocity = RopeArrow.transform.right * ropeArrowSpeed * 1 / 3;  //화살 발사 속도 = x축 방향 * 파워 * 속도값

        isRopeArrowMoving = true;
    }

    private void ImageSet()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites/FireAngle_anim");
    }

}
