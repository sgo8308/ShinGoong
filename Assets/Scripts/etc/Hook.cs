using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    private PlayerMove playerMove;
    private GameObject player = null;

    public LineRenderer line;
    public GameObject hook;
    public GameObject ropeArrow;

    Vector2 mousedir;
    Vector2 mousePosition;
    Vector2 direction;


    public bool isHookActive;
    public static bool isHookMoving;

    public bool isRopeStart;

    public bool isLineMax;
    public bool isAttach;


    private Animator animator;
    Vector2 hook_position;
    Vector2 lineStart_position;

    float playerRopeDegree;
    private Camera mainCamera;

    float ropeArrowAngle;
    int currnetAngleType;

    void Start()
    {
        player = GameObject.Find("Player");
        playerMove = player.GetComponent<PlayerMove>();
        animator = GetComponent<Animator>();

        lineStart_position = new Vector2(transform.position.x, transform.position.y - 0.5f);

        hook.SetActive(false);

       //hook의 위치를 플레이어 위치로 잡아준다.
        hook_position = new Vector2(transform.position.x, transform.position.y - 0.5f);
        hook.transform.position = hook_position;
      



        print(hook_position.y);

        line.positionCount = 2;
        line.endWidth = line.startWidth = 0.05f;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.transform.position);
        line.useWorldSpace = true;
        isAttach = false;

    }


    void Update()
    {
        lineStart_position = new Vector2(transform.position.x, transform.position.y - 0.5f);

      


        if (!HookCollider.ropePull)  //로프가 플랫폼에 충돌하기 전
        {
            

            line.SetPosition(0, lineStart_position);
            line.SetPosition(1, hook.transform.position);


            if (Input.GetKey(KeyCode.E) && !isHookActive && Input.GetMouseButtonDown(0) && !animator.GetBool("isJumpingUp") && !animator.GetBool("isJumpingDown"))
            {
                playerMove.FlipPlayer();

                isHookMoving = true;

                hook_position = new Vector2(transform.position.x, transform.position.y - 0.5f);
                hook.transform.position = hook_position;

                animator.SetBool("isReady", true);
                Invoke("ReadyToAngle", 1.1f);

                SetRopeArrowDirection();           //RopeArrow의 벡터를 마우스 방향으로 잡아준다.      

                CalculateBowAngle();



                hook.SetActive(true);

                

                line.SetPosition(0, lineStart_position);
                line.SetPosition(1, hook.transform.position);

                mousedir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - hook.transform.position;


                isHookActive = true;
                isLineMax = false;

                hook.gameObject.SetActive(true);

                Invoke("RopeStart", 1.2f);
            }

            
            RopeAction();
        }

       


        if (HookCollider.ropePull)  //로프가 충돌했을때
        {

            isHookActive = false;

            hook.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
     
            GetComponent<Rigidbody2D>().gravityScale = 0;

            //플레이어가 hook으로 날아간다.
            transform.position = Vector2.MoveTowards(transform.position, hook.transform.position, Time.deltaTime * 20);

            playerDirection();

            line.SetPosition(0, transform.position);  // 라인 스타트 지점을 플레이어의 위치로 항상 잡아준다. 


            animator.SetBool("isRopeMoving", true);


            if (Vector2.Distance(transform.position, hook.transform.position) < 1.0f)  //로프 이동이 끝남
            {
                
                animator.SetBool("isRopeMoving", false);
                
                animator.SetBool("isReady", false);

                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);

                isHookActive = false;
                isHookMoving = false;
                isRopeStart = false;

                isLineMax = false;
                hook.gameObject.SetActive(false);
                hook.transform.position = transform.position;       

                HookCollider.ropePull = false;
                GetComponent<Rigidbody2D>().gravityScale = 3;

                hook.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

                hook.SetActive(false);     
     
            }
        }

    }

    private void playerDirection()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, playerRopeDegree);
    }






    private void RopeStart()
    {
        isRopeStart = true;

        animator.SetBool("isRopeAiming20", false);
        animator.SetBool("isRopeAiming30", false);
        animator.SetBool("isRopeAiming40", false);
        animator.SetBool("isRopeAiming50", false);
        animator.SetBool("isRopeAiming60", false);
        animator.SetBool("isRopeAiming70", false);
        animator.SetBool("isRopeAiming80", false);
        animator.SetBool("isRopeAiming90", false);
        animator.SetBool("isRopeAiming100", false);
        animator.SetBool("isRopeAiming110", false);
        animator.SetBool("isRopeAiming120", false);
        animator.SetBool("isRopeAiming130", false);


    }

    private void ReadyToAngle()
    {
      //  animator.SetBool("isReady", false);

        if (ropeArrowAngle >= 0 && ropeArrowAngle < 20) //마우스 각도가 0~20도 일때 Aiming20 애니메이션 시작
        {
            animator.SetBool("isRopeAiming20", true);
            currnetAngleType = 20;
        }
        if (ropeArrowAngle >= 20 && ropeArrowAngle < 30)
        {
            animator.SetBool("isRopeAiming30", true);
            currnetAngleType = 30;

        }
        if (ropeArrowAngle >= 30 && ropeArrowAngle < 40)
        {
            animator.SetBool("isRopeAiming40", true);
            currnetAngleType = 40;

        }
        if (ropeArrowAngle >= 40 && ropeArrowAngle < 50)
        {
            animator.SetBool("isRopeAiming50", true);
            currnetAngleType = 50;
        }
        if (ropeArrowAngle >= 50 && ropeArrowAngle < 60)
        {
            animator.SetBool("isRopeAiming60", true);
            currnetAngleType = 60;
        }
        if (ropeArrowAngle >= 60 && ropeArrowAngle < 70)
        {
            animator.SetBool("isRopeAiming70", true);
            currnetAngleType = 70;
        }
        if (ropeArrowAngle >= 70 && ropeArrowAngle < 80)
        {
            animator.SetBool("isRopeAiming80", true);
            currnetAngleType = 80;
        }
        if (ropeArrowAngle >= 80 && ropeArrowAngle < 90)
        {
            animator.SetBool("isRopeAiming90", true);
            currnetAngleType = 90;
        }
        if (ropeArrowAngle >= 90 && ropeArrowAngle < 100)
        {
            animator.SetBool("isRopeAiming100", true);
            currnetAngleType = 100;
        }
        if (ropeArrowAngle >= 100 && ropeArrowAngle < 110)
        {
            animator.SetBool("isRopeAiming110", true);
            currnetAngleType = 110;
        }
        if (ropeArrowAngle >= 110 && ropeArrowAngle < 120)
        {
            animator.SetBool("isRopeAiming120", true);
            currnetAngleType = 120;
        }
        if (ropeArrowAngle >= 120)
        {
            animator.SetBool("isRopeAiming130", true);
            currnetAngleType = 130;
        }

    }

    private void RopeAction()
    {
        if (isRopeStart)
        {
            if (isHookActive && !isLineMax && !isAttach)
            {
                // hook이 발사된다.
                hook.transform.Translate(mousedir.normalized * Time.deltaTime * 15);


                if (Vector2.Distance(lineStart_position, hook.transform.position) > 25)
                {

                    isLineMax = true;
                }

            }
            else if (isHookActive && isLineMax && !isAttach)
            {
                //hook이 되돌아온다.
                hook.transform.position = Vector2.MoveTowards(hook.transform.position, lineStart_position, Time.deltaTime * 12);

                if (Vector2.Distance(lineStart_position, hook.transform.position) < 0.01f)
                {
                    isHookActive = false;
                    isHookMoving = false;
                    isLineMax = false;
                    isRopeStart = false;
                    animator.SetBool("isReady", false);

                    hook_position = new Vector2(transform.position.x, transform.position.y - 0.5f);
                    hook.transform.position = hook_position;
                    hook.SetActive(false);

                }
            }
            else if (isAttach)
            {

            }
        }
    }

    private void SetRopeArrowDirection()
    {
    

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //스크린상의 마우스좌표 -> 게임상의 2d 좌표로 치환
        direction = new Vector2(mousePosition.x - lineStart_position.x,
                                          mousePosition.y - lineStart_position.y );   //마우스 좌표 - 화살 좌표 = 바라볼 방향

        ropeArrow.transform.right = direction;  //화살의 x축 방향을 '바라볼 방향'으로 정한다

        playerRopeDegree = -(90 - Mathf.Atan2(direction.y , direction.x) * Mathf.Rad2Deg);
        print(playerRopeDegree);

    }

    private void CalculateBowAngle()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //스크린상의 마우스좌표 -> 게임상의 2d 좌표로 치환
        Vector2 t_direction = new Vector2(mousePosition.x - lineStart_position.x,
                                          mousePosition.y - lineStart_position.y);   //마우스 좌표 - 화살 좌표 = 바라볼 방향



        ropeArrow.transform.right = t_direction;  //화살의 x축 방향을 '바라볼 방향'으로 정한다
        playerRopeDegree = -(90 - Mathf.Atan2(t_direction.y, t_direction.x) * Mathf.Rad2Deg);


        ropeArrowAngle = Mathf.Atan2(t_direction.y, t_direction.x) * Mathf.Rad2Deg;   //조준하고 있는 각도 세타 구하기


        if (ropeArrowAngle >= 0)
        {
            ropeArrowAngle = Mathf.Abs(90 - ropeArrowAngle);
        }
        else if (ropeArrowAngle < 0 && ropeArrowAngle >= -90)
        {
            ropeArrowAngle = Mathf.Abs(ropeArrowAngle - 90);

        }
        else if (ropeArrowAngle < -90 && ropeArrowAngle >= -180)
        {
            ropeArrowAngle = 270 - Mathf.Abs(ropeArrowAngle);
        }


        print(ropeArrowAngle);
    }

}
