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
        if (Input.GetMouseButtonDown(0))
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
        if (Input.GetButtonDown("Jump")) //&& !anim.GetBool(isJumping")추가하기
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

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
        }else 
        {
            anim.SetBool("isRunning", true);
        }
    }

    private void FixedUpdate()
    {
        //Move By Key Control
        float h = Input.GetAxisRaw("Horizontal");  //GetAxisRaw 함수를 이용해 Horizontal 값을 가져옴(-1,0,1) [Edit] -> [Project Settings] -> Input
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed)  //Right Mas Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1))  //Left Mas Speed
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
    }


    void DistanceFromArrow()
    {

    }
}

