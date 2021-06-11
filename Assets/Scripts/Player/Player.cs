using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public static bool canMove;
    public float maxSpeed;
    public float jumpPower;

    public InventoryUI inventoryUI;

    Rigidbody2D _rigid;
    SpriteRenderer _spriteRenderer;

    Vector2 _mousePosition;
    Camera _mainCamera = null;

    Animator _animator;

    public static bool jumpingState = false;

    public static bool ropeMove = false;
    
    public Transform arrowDirection = null;

    float _aimAngle;

    public delegate void OnPlayerDead();
    public OnPlayerDead onPlayerDead;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();  //초기화
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _mainCamera = Camera.main;    //태그가 main인 카메라를 변수에 넣어준다.
        Cursor.visible = true;
        canMove = true;
        
    }

    void P_directionSet()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetAxisRaw("Horizontal") == 0)
        {
            _mousePosition = Input.mousePosition;
            _mousePosition = _mainCamera.ScreenToWorldPoint(_mousePosition);

            if (_mousePosition.x > transform.position.x) //마우스가 플레이어보다 오른쪽에 있을때
            {
                _spriteRenderer.flipX = false;
            }
            else if (_mousePosition.x <= transform.position.x) //마우스가 플레이어보다 왼쪽에 있을때
            {
                _spriteRenderer.flipX = true;
            }
        }
    }

    void Update()
    {
        if (!canMove)
        {
            _animator.SetBool("isRunning", false);
            return;
        }

        //Jump
        if (Input.GetButtonDown("Jump") && !_animator.GetBool("isJumping"))   
        {
            _rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            _animator.SetBool("isJumping", true);
            jumpingState = true;
        }
        //Stop Speed
        if (Input.GetButtonUp("Horizontal")) //버튼을 계속 누르고 있다가 땔때 
            _rigid.velocity = new Vector2(_rigid.velocity.normalized.x * 0.1f, _rigid.velocity.y);

        if (Input.GetButtonDown("Horizontal"))
            _spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;  //1과 -1이 같지 않을때 false 출력(체크해제)

        P_directionSet();

        //Animation
        if (Mathf.Abs(_rigid.velocity.x) < 0.4)
            _animator.SetBool("isRunning", false);
        else
            _animator.SetBool("isRunning", true);

        RopeMove();

        AttackReady();
    }

    private void FixedUpdate()
    {
        if (!canMove)
            return;

        //GetAxisRaw 함수를 이용해 Horizontal 값을 가져옴(-1,0,1) [Edit] -> [Project Settings] -> Input
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            _spriteRenderer.flipX = false;                         //Turn right
            _rigid.AddForce(Vector2.right, ForceMode2D.Impulse);  //Move right
        }
        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            _spriteRenderer.flipX = true;                          //Turn left
            _rigid.AddForce(Vector2.left, ForceMode2D.Impulse);   //Move left
        }

        //Max Speed
        if (_rigid.velocity.x > maxSpeed)  //Right Max Speed
            _rigid.velocity = new Vector2(maxSpeed, _rigid.velocity.y);
        else if (_rigid.velocity.x < maxSpeed * (-1))  //Left Max Speed
            _rigid.velocity = new Vector2(maxSpeed * (-1), _rigid.velocity.y);

        //Landing Platform
        if (_rigid.velocity.y < 0)  //플레이어가 아래로 떨어질때 Down Ray를 사용한다.
        {
            Debug.DrawRay(_rigid.position, Vector3.down, new Color(0, 1, 0));

            RaycastHit2D rayHit = Physics2D.Raycast(_rigid.position, Vector3.down, 3, LayerMask.GetMask("Platform"));  //Ray가 맞은 오브젝트 (UI레이어만 해당됨)

            if (rayHit.collider != null)  //레이와 충돌한 오브젝트가 있다면
            {
                if (rayHit.distance < 1.2f)  //플레이어의 발바닥 바로 아래에서 무언가가 감지된다면 
                {
                    _animator.SetBool("isJumping", false);
                    jumpingState = false;
                }
            }
        }

        if (_rigid.velocity.y == 0)
        {
            _animator.SetBool("isJumping", false);
            jumpingState = false;
        }
    }

    #region Dead
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Radar")
            Invoke("Dead", 0.1f); // dead after 0.1 seconds
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Radar")
            CancelInvoke("Dead");
    }

    void Dead()
    {
        _animator.SetBool("isHit", true);
        canMove = false;
        
        if(onPlayerDead != null)
            onPlayerDead.Invoke();
    }
    #endregion
    
    public void AcquireCoin()
    {
        MainUI.instance. coinCount++;

        MainUI.instance.UpdateCoinUI();

        inventoryUI.UpdateCoinUI();
    }

    private void AttackReady()
    {
        if (Input.GetMouseButtonDown(0))  //down -> ready애니메이션 시작
        {
            CalculateBowAngle();
            _animator.SetBool("isReady", true);
            Invoke("ReadyCancel", 0.8f);
        }

        if (Input.GetMouseButtonUp(0))  //up -> 0.2초 뒤에 angle애니메이션 취소
        {
            _animator.SetBool("isAiming20", false);
            _animator.SetBool("isFireFinish20", true);

            Invoke("AimingCancel", 0.3f);
        }
    }

    void CalculateBowAngle()
    {
        Vector2 t_mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition); //스크린상의 마우스좌표 -> 게임상의 2d 좌표로 치환
        Vector2 t_direction = new Vector2(t_mousePos.x - arrowDirection.position.x,
                                          t_mousePos.y - arrowDirection.position.y);   //마우스 좌표 - 화살 좌표 = 바라볼 방향

        _aimAngle = Mathf.Atan2(t_direction.y, t_direction.x) * Mathf.Rad2Deg;   //조준하고 있는 각도 세타 구하기
        _aimAngle = Mathf.Abs(90 - _aimAngle);
        print(_aimAngle);
    }

    private void ReadyCancel()  //Ready애니메이션 끝나자 마자 Aiming애니메이션 시작
    {
        _animator.SetBool("isReady", false);

        if (_aimAngle >= 0 && _aimAngle < 20) //마우스 각도가 0~20도 일때 Aiming20 애니메이션 시작
        {
            _animator.SetBool("isAiming20", true);
        }
        if (_aimAngle >= 20 && _aimAngle < 30)
        {
            _animator.SetBool("isAiming30", true);
        }
    }

    private void AimingCancel()
    {
        _animator.SetBool("isFireFinish20", false);

    }

    private void RopeMove()
    {
        if (ropeMove)
        {
            print("move");
            Vector2 ropeArrow_Position = RopeArrow.currentRopeArrowPositionList[0]; //스크린상의 마우스좌표 -> 게임상의 2d 좌표로 치환
                                                                               
            this.GetComponent<Rigidbody2D>().gravityScale = 0; //플레이어의 중력을 0으로 한다.

            transform.position = Vector2.MoveTowards(gameObject.transform.position, ropeArrow_Position, 0.1f);  //로프화살 좌표까지 이동한다.

            Vector2 p_Position = transform.position;

            if (p_Position == ropeArrow_Position)
            {
                ropeMove = false;
                this.GetComponent<Rigidbody2D>().gravityScale = 3;
            }
        }
    }
}

