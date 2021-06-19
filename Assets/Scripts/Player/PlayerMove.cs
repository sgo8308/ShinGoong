using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMove : MonoBehaviour
{
    public bool canMove { get; private set; }
    public bool isRopeMoving { get; private set; }
    public bool isJumping { get; private set; }

    public int stopSoundCount;

    private PlayerInfo playerInfo;
    private Animator animator;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    private Vector2 mousePosition;
    private Vector2 zeroVector;

    private void Start()
    {
        playerInfo = GetComponent<PlayerInfo>();
        animator = GetComponent <Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;

        canMove = true;
        
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += 
            (Scene scene, LoadSceneMode mode) => canMove = true;

        zeroVector = new Vector2(0, 0);
    }

    private void FixedUpdate()
    {
        if (!canMove)
            return;

        //GetAxisRaw 함수를 이용해 Horizontal 값을 가져옴(-1,0,1) [Edit] -> [Project Settings] -> Input
        if (Input.GetAxisRaw("Horizontal") == 1)
            MoveRight();
        if (Input.GetAxisRaw("Horizontal") == -1)
            MoveLeft();

        limitSpeed();

        CheckIfJumping();
    }

    private void Update()
    {
        if (isRopeMoving)
            RopeMove();

        if (!canMove)
        {
            StopPlayer();
            return;
        }

        if (Input.GetButtonDown("Jump") && !animator.GetBool("isJumping"))
        {
            Jump();
        }

        if (Input.GetButtonUp("Horizontal")) //버튼을 계속 누르고 있다가 땔때 
            StopPlayer();

        if (Mathf.Abs(rigid.velocity.x) < 0.4 || isJumping)
            animator.SetBool("isRunning", false);
        else
            animator.SetBool("isRunning", true);

        if (animator.GetBool("isRunning"))
        {
            if (!SoundManager.instance.playerSound.isPlaying)
            {
                SoundManager.instance.PlayPlayerSound(PlayerSounds.PLAYER_RUN);
                stopSoundCount = 0;
            }
        }
        else
        {
            if (stopSoundCount == 0 && !isJumping)
            {
                SoundManager.instance.StopPlayerSound();
                stopSoundCount = 1;
            }
        }
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    public void SetIsRopeMoving(bool value)
    {
        isRopeMoving = value;
    }

    private void MoveRight()
    {
        spriteRenderer.flipX = false;                         
        rigid.AddForce(Vector2.right, ForceMode2D.Impulse);  
    }

    private void MoveLeft()
    {
        spriteRenderer.flipX = true;                          
        rigid.AddForce(Vector2.left, ForceMode2D.Impulse);
    }

    private void limitSpeed()
    {
        if (rigid.velocity.x > playerInfo.maxSpeed)  //Right Max Speed
            rigid.velocity = new Vector2(playerInfo.maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < playerInfo.maxSpeed * (-1))  //Left Max Speed
            rigid.velocity = new Vector2(playerInfo.maxSpeed * (-1), rigid.velocity.y);
    }

    private void Jump()
    {
        rigid.AddForce(Vector2.up * playerInfo.jumpPower, ForceMode2D.Impulse);
        animator.SetBool("isRunning", false);
        animator.SetBool("isJumping", true);
        isJumping = true;

        SoundManager.instance.PlayPlayerSound(PlayerSounds.PLAYER_JUMP);
    }

    private void CheckIfJumping()
    {
        if (rigid.velocity.y < 0)  //플레이어가 아래로 떨어질때 Down Ray를 사용한다.
        {
            Debug.DrawRay(rigid.position, 3 * Vector3.down, new Color(0, 1, 0));

            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 3, LayerMask.GetMask("Platform"));  //Ray가 맞은 오브젝트 (UI레이어만 해당됨)

            if (rayHit.collider != null)  //레이와 충돌한 오브젝트가 있다면
            {
                if (rayHit.distance < 2.2f)  //플레이어의 발바닥 바로 아래에서 무언가가 감지된다면 
                {
                    animator.SetBool("isJumping", false);

                    if (isJumping)
                        SoundManager.instance.PlayPlayerSound(PlayerSounds.PLAYER_LAND);

                    isJumping = false;

                }
            }
        }

        if (rigid.velocity.y == 0)
        {
            animator.SetBool("isJumping", false);
            isJumping = false;
        }
    }


    public float ropeMoveSpeed;

    private void RopeMove()
    {
        if (isRopeMoving)
        {
            Vector2 ropeArrow_Position = RopeArrow.currentRopeArrowPositionList[0]; //스크린상의 마우스좌표 -> 게임상의 2d 좌표로 치환

            this.GetComponent<Rigidbody2D>().gravityScale = 0; //플레이어의 중력을 0으로 한다.

            transform.position = Vector2.MoveTowards(transform.position, 
                                        ropeArrow_Position, ropeMoveSpeed);  //로프화살 좌표까지 이동한다.

            Vector2 p_Position = transform.position;

            if (p_Position == ropeArrow_Position)
            {
                isRopeMoving = false;
                this.GetComponent<Rigidbody2D>().gravityScale = 3;
            }
        }
    }

    public void FlipPlayer()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x > transform.position.x) //마우스가 플레이어보다 오른쪽에 있을때
        {
            spriteRenderer.flipX = false;
        }
        else if (mousePosition.x <= transform.position.x) //마우스가 플레이어보다 왼쪽에 있을때
        {
            spriteRenderer.flipX = true;
        }
    }

    public void StopPlayer()
    {
        rigid.velocity = zeroVector;
        animator.SetBool("isRunning", false);
    }
}
