using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public static Player instance;
    public static bool canMove;
    public float maxSpeed;
    public float jumpPower;

    public int coinCount;
    public int arrowCount;
    public int level;

    public InventoryUI inventoryUI;
    public Canvas mainUI;

    Rigidbody2D _rigid;
    SpriteRenderer _spriteRenderer;

    Vector2 _mousePosition;
    Camera _mainCamera = null;

    Animator _animator;

    public static bool jumpingState = false;

    public static bool _ropeMove = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        _rigid = GetComponent<Rigidbody2D>();  //초기화
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _mainCamera = Camera.main;    //태그가 main인 카메라를 변수에 넣어준다.
        Cursor.visible = true;
        canMove = true;
        coinCount = 0;
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

        ropeMove();
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
        //canMove = false;
    }
    #endregion
    
    public void AcquireCoin()
    {
        coinCount++;

        UpdateCoinUI();

        inventoryUI.UpdateCoinUI(coinCount);
    }

    void UpdateCoinUI()
    {
        mainUI.transform.Find("CoinCount")
                .GetComponent<TextMeshProUGUI>().text = coinCount.ToString();
    }

    private void Rope()
    {
        if (Input.GetKey(KeyCode.R))
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mousePosition = Input.mousePosition;
                _mousePosition = _mainCamera.ScreenToWorldPoint(_mousePosition);
                print(_mousePosition);
            }
        }
    }

    private void ropeMove()
    {
        if (_ropeMove)
        {
            print("move");
            Vector2 ropeArrow_Position = RopeArrow.currentRopeArrowPositionList[0]; //스크린상의 마우스좌표 -> 게임상의 2d 좌표로 치환
                                                                               
            this.GetComponent<Rigidbody2D>().gravityScale = 0; //플레이어의 중력을 0으로 한다.

            transform.position = Vector2.MoveTowards(gameObject.transform.position, ropeArrow_Position, 0.1f);  //로프화살 좌표까지 이동한다.

            Vector2 p_Position = transform.position;

            if (p_Position == ropeArrow_Position)
            {
                _ropeMove = false;
                this.GetComponent<Rigidbody2D>().gravityScale = 3;
            }
        }
    }
}

