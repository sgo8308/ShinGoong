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

    private float aimAngle;

    private bool canShoot = true;

    float _aimAngle;
    float _ReAimAngle;

    bool _aiming;
    bool _angleChange;
    int _currnetAngleType;
    int _ReAimAngleType;

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
            playerMove.StopPlayer();
            playerMove.FlipPlayer();
            playerMove.SetCanMove(false);
            ControlPower();
        }

        if (Input.GetMouseButtonUp(0) && !Input.GetKey(KeyCode.R))
        {
            playerMove.SetCanMove(true);
            Invoke("ShootArrow", 0.1f);
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
        if (Input.GetMouseButtonDown(0))  //down -> ready애니메이션 시작
        {
            CalculateBowAngle();
            animator.SetBool("isReady", true);
            Invoke("ReadyToAim", 0.7f);  //0.7초 후에 준비자세에서 조준자세로 바꿔준다.

        }

        if (Input.GetMouseButton(0))
        {
            ReAiming();

        }

        if (Input.GetMouseButtonUp(0))  //up -> 0.2초 뒤에 angle애니메이션 취소
        {
            print("마우스 up");
            this.gameObject.GetComponent<Animator>().enabled = true;

            animator.SetBool("isReady", false);
            animator.SetBool("isAiming20", false);
            animator.SetBool("isFireFinish20", true);  // FireFinish 애니메이션 시작 = 발사!!

            animator.SetBool("isAiming30", false);
            animator.SetBool("isFireFinish30", true);

            print("마우스 up2");

            Invoke("FireFinish", 0.2f);
        }
    }

    private void CalculateBowAngle()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); //스크린상의 마우스좌표 -> 게임상의 2d 좌표로 치환
        Vector2 t_direction = new Vector2(mousePosition.x - arrowDirection.position.x,
                                          mousePosition.y - arrowDirection.position.y);   //마우스 좌표 - 화살 좌표 = 바라볼 방향

        aimAngle = Mathf.Atan2(t_direction.y, t_direction.x) * Mathf.Rad2Deg;   //조준하고 있는 각도 세타 구하기
        aimAngle = Mathf.Abs(90 - aimAngle);
        print(aimAngle);
    }

    void CalculateBowAngle_ReAim()
    {
        Vector2 t_mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition); //스크린상의 마우스좌표 -> 게임상의 2d 좌표로 치환
        Vector2 t_direction = new Vector2(t_mousePos.x - arrowDirection.position.x,
                                          t_mousePos.y - arrowDirection.position.y);   //마우스 좌표 - 화살 좌표 = 바라볼 방향

        _ReAimAngle = Mathf.Atan2(t_direction.y, t_direction.x) * Mathf.Rad2Deg;   //조준하고 있는 각도 세타 구하기
        _ReAimAngle = Mathf.Abs(90 - _ReAimAngle);
        print(_ReAimAngle);

        if (_ReAimAngle < 20)
        {
            _ReAimAngleType = 20;
        }
        if (_ReAimAngle >= 20 && _ReAimAngle < 30)
        {
            _ReAimAngleType = 30;
        }

        if (_currnetAngleType != _ReAimAngleType)
        {
            print("aim 각도type : " + _currnetAngleType);
            print("재조준 각도type : " + _ReAimAngleType);
            _angleChange = true;
        }
    }

    private void ReadyToAim()  //Ready애니메이션 끝나자 마자 Aiming애니메이션 시작
    {
        animator.SetBool("isReady", false);

        if (_aimAngle >= 0 && _aimAngle < 20) //마우스 각도가 0~20도 일때 Aiming20 애니메이션 시작
        {
            animator.SetBool("isAiming20", true);

            _currnetAngleType = 20;
            _aiming = true;
            //현재 조준중인 상태이므로 여기서 마우스 이동시  스프라이트가 '각도별angle2'로 계속 업데이트 되야 함

        }
        if (_aimAngle >= 20 && _aimAngle < 30)
        {
            animator.SetBool("isAiming30", true);

            _currnetAngleType = 30;

            _aiming = true;

        }
    }

    private void FireFinish()
    {
        animator.SetBool("isFireFinish20", false);


        animator.SetBool("isFireFinish30", false);

        _aiming = false;
        _angleChange = false;
    }

    private void ReAiming()
    {
        if (_aiming)
        {
            CalculateBowAngle_ReAim(); //재조준 활 각도계산

            if (_angleChange)
            {
                if (_ReAimAngleType == 20)
                {
                    print("여기 들어와? 20");

                    //    gameObject.GetComponent<Animator>().speed = 0;

                    gameObject.GetComponent<Animator>().enabled = false;
                    animator.SetBool("isAiming20", false);
                    gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
                }


                if (_ReAimAngleType == 30)
                {
                    print("여기 들어와? 30");

                    //     gameObject.GetComponent<Animator>().speed = 0;

                    gameObject.GetComponent<Animator>().enabled = false;

                    animator.SetBool("isAiming30", false);
                    gameObject.GetComponent<SpriteRenderer>().sprite = sprites[4];
                }
            }
        }
    }

    private void ReadyCancel()  //Ready애니메이션 끝나자 마자 Aiming애니메이션 시작
    {
        animator.SetBool("isReady", false);

        if (aimAngle >= 0 && aimAngle < 20) //마우스 각도가 0~20도 일때 Aiming20 애니메이션 시작
        {
            animator.SetBool("isAiming20", true);
        }
        if (aimAngle >= 20 && aimAngle < 30)
        {
            animator.SetBool("isAiming30", true);
        }
    }

    private void SetArrowDirection()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); //스크린상의 마우스좌표 -> 게임상의 2d 좌표로 치환
        Vector2 direction = new Vector2(mousePosition.x - arrowDirection.transform.position.x,
                                          mousePosition.y - arrowDirection.transform.position.y);   //마우스 좌표 - 화살 좌표 = 바라볼 방향

        arrowDirection.transform.right = direction;  //화살의 x축 방향을 '바라볼 방향'으로 정한다
        arrowDirection.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 0.3f); // 플레이어 목 근처에서 화살이 나가게  y값 조정
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
