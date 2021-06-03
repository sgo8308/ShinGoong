using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    Vector2 MousePosition;
    Camera m_cam = null; //카메라 변수

    Animator anim;

    public static bool jumpingState = false;  //플레이어의 점핑 상태

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();  //초기화
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        m_cam = Camera.main;    //태그가 main인 카메라를 변수에 넣어준다.
        Cursor.visible = true;
    }

    void P_directionSet()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetAxisRaw("Horizontal") == 0)
        {

            MousePosition = Input.mousePosition;
            MousePosition = m_cam.ScreenToWorldPoint(MousePosition);

            if (MousePosition.x > transform.position.x) //마우스가 플레이어보다 오른쪽에 있을때
            {
                spriteRenderer.flipX = false;
            }
            else if (MousePosition.x <= transform.position.x) //마우스가 플레이어보다 왼쪽에 있을때
            {
                spriteRenderer.flipX = true;
            }
        }
    }


    void Update()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))   
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
            jumpingState = true;
        }
        //Stop Speed
        if (Input.GetButtonUp("Horizontal")) //버튼을 계속 누르고 있다가 땔때 
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.1f, rigid.velocity.y);
        }
        if (Input.GetButtonDown("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;  //1과 -1이 같지 않을때 false 출력(체크해제)


        P_directionSet();

        //Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.4)
        {
            anim.SetBool("isRunning", false);
            
        }
        else
        {
            anim.SetBool("isRunning", true);
            
        }


    }

    private void FixedUpdate()
    {
        //GetAxisRaw 함수를 이용해 Horizontal 값을 가져옴(-1,0,1) [Edit] -> [Project Settings] -> Input

        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            spriteRenderer.flipX = false;                         //Turn right
            rigid.AddForce(Vector2.right, ForceMode2D.Impulse);  //Move right
        }
        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            spriteRenderer.flipX = true;                          //Turn left
            rigid.AddForce(Vector2.left, ForceMode2D.Impulse);   //Move left
        }

        //Max Speed
        if (rigid.velocity.x > maxSpeed)  //Right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1))  //Left Max Speed
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        //Landing Platform
        if (rigid.velocity.y < 0)  //플레이어가 아래로 떨어질때 Down Ray를 사용한다.
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 3, LayerMask.GetMask("UI"));  //Ray가 맞은 오브젝트 (UI레이어만 해당됨)

            if (rayHit.collider != null)  //레이와 충돌한 오브젝트가 있다면
            {
                if (rayHit.distance < 1.2f)  //플레이어의 발바닥 바로 아래에서 무언가가 감지된다면 
                {
                    anim.SetBool("isJumping", false);
                    jumpingState = false;
                }
            }
        }

        if (rigid.velocity.y == 0)
        {
            anim.SetBool("isJumping", false);
            jumpingState = false;
        }



    }

    void PlayerDead()
    {
        Debug.Log("플레이어 죽었다");
    }
}

