using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    float _aimAngle;
    float _ReAimAngle;

    private PlayerMove playerMove;
    private PlayerAttack playerAttack;
    private PlayerSkill playerSkill;
    private Animator animator;

    public delegate void OnPlayerDead();
    public OnPlayerDead onPlayerDead;

    bool _aiming;
    bool _angleChange;
    int _currnetAngleType;
    int _ReAimAngleType;

    Sprite[] sprites;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();  //초기화
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

    }

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        playerAttack = GetComponent<PlayerAttack>();
        playerSkill = GetComponent<PlayerSkill>();
        animator = GetComponent<Animator>();
        Cursor.visible = true;

        ImageSet();  //재조준할때 바꾸어줄 이미지를 세팅해 놓는다.

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += Revive;
    }

    public void Revive(Scene scene, LoadSceneMode mode)
    {
        animator.SetBool("isHit", false);

        playerMove.SetCanMove(true);

        playerAttack.SetCanShoot(true);
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
        animator.SetBool("isHit", true);
        
        if(onPlayerDead != null)
            onPlayerDead.Invoke();
        
        playerMove.SetCanMove(false);
        playerAttack.SetCanShoot(false);
    }
    #endregion
    
    public void AcquireCoin()
    {
        Inventory.instance.AddCoin(1);

        MainUI.instance.UpdateCoinUI();

        Inventory.instance.UpdateCoin();
    }
    
    private void AttackReady()
    {
        

        if (Input.GetMouseButtonDown(0))  //down -> ready애니메이션 시작
        {
            CalculateBowAngle();
            _animator.SetBool("isReady", true);
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

            _animator.SetBool("isReady", false);
            _animator.SetBool("isAiming20", false);
            _animator.SetBool("isFireFinish20", true);  // FireFinish 애니메이션 시작 = 발사!!

            _animator.SetBool("isAiming30", false);
            _animator.SetBool("isFireFinish30", true);

            print("마우스 up2");

            Invoke("FireFinish", 0.2f);
      
        }
    }

    #region Item
    public void Sell(InventorySlot slot)
    {
        Inventory.instance.AddCoin(slot.GetItem().priceInInventory);
        MainUI.instance.UpdateCoinUI();
        Inventory.instance.UpdateCoin();

        Inventory.instance.RemoveItem(slot.GetSlotNum());
    }

    public void Buy(StoreSlot storeSlot)
    {
        Inventory.instance.SubtractCoin(storeSlot.item.priceInStore);
        MainUI.instance.UpdateCoinUI();
        Inventory.instance.UpdateCoin();

        Inventory.instance.AddItem(storeSlot.item);
    }
    
    public void Equip(InventorySlot inventorySlot, InventoryEquipSlot equipSlot)
    {
        Item tempItem = inventorySlot.GetItem();

        if (equipSlot.IsItemSet())
        {
            inventorySlot.SetItem(equipSlot.GetItem());

            equipSlot.SetItem(tempItem);

            Inventory.instance.
                ChangeItem(inventorySlot.GetSlotNum(), inventorySlot.GetItem());
        }
        else
        {
            equipSlot.SetItem(tempItem);

            Inventory.instance.RemoveItem(inventorySlot.GetSlotNum());
        }

        playerSkill.SetSkill(equipSlot.GetItem());
    }

    public void UnEquip(InventoryEquipSlot equipSlot)
    {
        if (Inventory.instance.GetItemCount() < Inventory.instance.GetSlotCount())
        {
            Inventory.instance.AddItem(equipSlot.GetItem());
            equipSlot.RemoveItem();
        }
    }

    public void Use(InventorySlotInfo slotInfo)
    {
        Inventory.instance.RemoveItem(slotInfo.slotNum);
    } 
    #endregion
    
    void CalculateBowAngle_ReAim()
    {
        Vector2 t_mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition); //스크린상의 마우스좌표 -> 게임상의 2d 좌표로 치환
        Vector2 t_direction = new Vector2(t_mousePos.x - arrowDirection.position.x,
                                          t_mousePos.y - arrowDirection.position.y);   //마우스 좌표 - 화살 좌표 = 바라볼 방향

        _ReAimAngle = Mathf.Atan2(t_direction.y, t_direction.x) * Mathf.Rad2Deg;   //조준하고 있는 각도 세타 구하기
        _ReAimAngle = Mathf.Abs(90 - _ReAimAngle);
        print(_ReAimAngle);
        
        if(_ReAimAngle < 20)
        {
            _ReAimAngleType = 20;
        }
        if (_ReAimAngle >= 20 &&_ReAimAngle < 30)
        {
            _ReAimAngleType = 30;
        }

        if (_currnetAngleType != _ReAimAngleType)
        {
            print("aim 각도type : " + _currnetAngleType);
            print("재조준 각도type : "+ _ReAimAngleType);
            _angleChange = true;
        }
    }

    private void ReadyToAim()  //Ready애니메이션 끝나자 마자 Aiming애니메이션 시작
    {
        _animator.SetBool("isReady", false);

        if (_aimAngle >= 0 && _aimAngle < 20) //마우스 각도가 0~20도 일때 Aiming20 애니메이션 시작
        {
            _animator.SetBool("isAiming20", true);

            _currnetAngleType = 20;
            _aiming = true;
            //현재 조준중인 상태이므로 여기서 마우스 이동시  스프라이트가 '각도별angle2'로 계속 업데이트 되야 함

        }
        if (_aimAngle >= 20 && _aimAngle < 30)
        {
            _animator.SetBool("isAiming30", true);

            _currnetAngleType = 30;

            _aiming = true;

        }
    }

    private void FireFinish()
    {
        _animator.SetBool("isFireFinish20", false);

  
        _animator.SetBool("isFireFinish30", false);

        _aiming = false;
        _angleChange = false;
    }

    private void ReAiming()
    {
        if (_aiming )
        {
            CalculateBowAngle_ReAim(); //재조준 활 각도계산

            if (_angleChange)
            {
                if (_ReAimAngleType == 20)
                {
                    print("여기 들어와? 20");

                //    gameObject.GetComponent<Animator>().speed = 0;


                    gameObject.GetComponent<Animator>().enabled = false;
                    _animator.SetBool("isAiming20", false);
                    gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
                }


                if (_ReAimAngleType == 30)
                {
                    print("여기 들어와? 30");

               //     gameObject.GetComponent<Animator>().speed = 0;

                    gameObject.GetComponent<Animator>().enabled = false;

                    _animator.SetBool("isAiming30", false);
                    gameObject.GetComponent<SpriteRenderer>().sprite = sprites[4];

                     
                }
            }
        }
    }

    private void ImageSet()
    {
        sprites  =  Resources.LoadAll<Sprite>("Sprites/FireAngle_anim");
      //  print(sprites.Length);
   // 
   //    for (int i = 0; i < sprites.Length; i++)
   //    {
   //        //들어간 배열 수 만큼 반복하여 이름 콘솔 창에 띄움.
   //        Debug.Log(sprites[i].name);
   //    }
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

