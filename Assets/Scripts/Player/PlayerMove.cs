using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMove : MonoBehaviour
{
    public bool canMove { get; private set; }
    public bool isJumping { get; private set; }

    private bool isLanded;

    public int stopSoundCount;

    private PlayerInfo playerInfo;
    public Animator animator;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    private Vector2 mousePosition;
    private Vector2 zeroVector;

    private void Start()
    {
        playerInfo = GetComponent<PlayerInfo>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;

        canMove = true;

        UnityEngine.SceneManagement.SceneManager.sceneLoaded +=
            (Scene scene, LoadSceneMode mode) => canMove = true;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += InitJumpValues;
            
        zeroVector = new Vector2(0, 0);
    }

    private void FixedUpdate()
    {
        if (!canMove)
            return;

        //GetAxisRaw 함수를 이용해 Horizontal 값을 가져옴(-1,0,1) [Edit] -> [Project Settings] -> Input
        if (Input.GetAxisRaw("Horizontal") == 1 && !animator.GetBool("isReady") )
            MoveRight();

        if (Input.GetAxisRaw("Horizontal") == -1 && !animator.GetBool("isReady"))
            MoveLeft();

        limitSpeed();

        if (Input.GetButtonUp("Horizontal")) //버튼을 계속 누르고 있다가 땔때 
            StopPlayer();
    }

    private void Update()
    {
        if (!canMove)
            return;

        CheckIfJumping();

        CheckIfOnGround();

        if (!canMove)
        {
            StopPlayer();
            return;
        }

        if (Input.GetButtonDown("Jump") && !animator.GetBool("isReady") && !isJumping)
        {
            Jump();
        }


        if (Mathf.Abs(rigid.velocity.x) < 0.4)
            animator.SetBool("isRunning", false);
        else if (!isJumping && !animator.GetBool("isJumpingDown"))
            animator.SetBool("isRunning", true);

        if (animator.GetBool("isRunning"))
        {
            if (!SoundManager.instance.playerRunningSound.isPlaying)
            {
                SoundManager.instance.PlayPlayerRunningSound();
                stopSoundCount = 0;
            }
        }
        else
        {
            if (stopSoundCount == 0 && !isJumping)
            {
                SoundManager.instance.StopPlayerRunningSound();
                stopSoundCount = 1;
            }
        }
    }



    public void SetCanMove(bool value)
    {
        canMove = value;
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

        animator.SetBool("isJumpingUp", true);

        isJumping = true;

        isLanded = false;

        SoundManager.instance.PlayPlayerSound(PlayerSounds.PLAYER_JUMP);
    }

    private void CheckIfJumping()
    {
        if (rigid.velocity.y < -0.1f)  //플레이어가 아래로 떨어질때 Down Ray를 사용한다.
        {
            //isJumping = true;


            animator.SetBool("isRunning", false);
            animator.SetBool("isJumpingUp", false);
            animator.SetBool("isJumpingDown", true);

            Vector2 rightVec = new Vector2(rigid.position.x + 0.4f, rigid.position.y);
            Vector2 leftVec = new Vector2(rigid.position.x - 0.45f, rigid.position.y);

            Debug.DrawRay(rigid.position, 3 * Vector3.down, new Color(0, 1, 0), 10.0f, false);
            Debug.DrawRay(rightVec, 3 * Vector3.down, new Color(0, 1, 0), 10.0f, false);
            Debug.DrawRay(leftVec, 3 * Vector3.down, new Color(0, 1, 0), 10.0f, false);

            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 3, LayerMask.GetMask("Platform"));  //Ray가 맞은 오브젝트 (UI레이어만 해당됨)
            RaycastHit2D rayHit2 = Physics2D.Raycast(rightVec, Vector3.down, 3, LayerMask.GetMask("Platform"));  //Ray가 맞은 오브젝트 (UI레이어만 해당됨)
            RaycastHit2D rayHit3 = Physics2D.Raycast(leftVec, Vector3.down, 3, LayerMask.GetMask("Platform"));  //Ray가 맞은 오브젝트 (UI레이어만 해당됨)
            if (rayHit.collider != null || rayHit2.collider != null || rayHit3.collider != null)  //레이와 충돌한 오브젝트가 있다면
            {
                if (rayHit.distance < 1.8f || rayHit2.distance < 1.8f || rayHit3.distance < 1.8f)  //플레이어의 발바닥 바로 아래에서 무언가가 감지된다면 
                {
                    if (!isLanded)
                        SoundManager.instance.PlayPlayerSound(PlayerSounds.PLAYER_LAND);

                    Debug.Log("발바닥아래 감지됐음");

                    isJumping = false;
                    isLanded = true;

                    animator.SetBool("isJumpingUp", false);
                    animator.SetBool("isJumpingDown", false);
                    animator.SetBool("isJumpingFinal", true);
                    Invoke("JumpFinalTime", 0.3f);
                    rigid.gravityScale = 3.0f;

                }
            }
        }
    }

    public void CheckIfOnGround()
    {
        Vector2 rightVec = new Vector2(rigid.position.x + 0.3f, rigid.position.y);
        Vector2 leftVec = new Vector2(rigid.position.x - 0.3f, rigid.position.y);

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 3, LayerMask.GetMask("Platform"));  //Ray가 맞은 오브젝트 (UI레이어만 해당됨)
        RaycastHit2D rayHit2 = Physics2D.Raycast(rightVec, Vector3.down, 3, LayerMask.GetMask("Platform"));  //Ray가 맞은 오브젝트 (UI레이어만 해당됨)
        RaycastHit2D rayHit3 = Physics2D.Raycast(leftVec, Vector3.down, 3, LayerMask.GetMask("Platform"));  //Ray가 맞은 오브젝트 (UI레이어만 해당됨)
        if (rayHit.collider != null || rayHit2.collider != null || rayHit3.collider != null)  //레이와 충돌한 오브젝트가 있다면
        {
            if (rayHit.distance < 1.8f || rayHit2.distance < 1.8f || rayHit3.distance < 1.8f)  //플레이어의 발바닥 바로 아래에서 무언가가 감지된다면 
            {
                Debug.Log("발바닥 감지");
                isLanded = true;
                isJumping = false;
                animator.SetBool("isJumpingUp", false);
                animator.SetBool("isJumpingDown", false);
            }
        }
    }

    public void InitJumpValues(Scene scene, LoadSceneMode mode)
    {
        isJumping = false;
        isLanded = true;
        animator.SetBool("isJumpingDown", false);
    }

    public void InitJumpValues()
    {
        isJumping = false;
        isLanded = true;
        animator.SetBool("isJumpingUp", false);
        animator.SetBool("isJumpingDown", false);
        animator.SetBool("isJumpingFinal", false);
    }

    void JumpFinalTime()
    {
        animator.SetBool("isJumpingDown", false);

        animator.SetBool("isJumpingFinal", false);

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
