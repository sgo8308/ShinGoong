

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Player에 붙어 있는 스크립트.
/// Player에 모든 움직임을 담당함.
/// </summary>
public class PlayerMove : MonoBehaviour
{
    public bool canMove { get; private set; }
    public bool isJumping;

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

        if (Input.GetAxisRaw("Horizontal") == 1 && !animator.GetBool("isReady") )
            MoveRight();

        if (Input.GetAxisRaw("Horizontal") == -1 && !animator.GetBool("isReady"))
            MoveLeft();

        limitSpeed();

        if (Input.GetButtonUp("Horizontal")) 
            StopPlayer();
    }

    private void Update()
    {
        if (!canMove)
            return;

        CheckIfJumping();

        CheckIfOnGround();

        if (Input.GetButtonDown("Jump") && !animator.GetBool("isReady") && !isJumping)
            Jump();

        if (!canMove)
        {
            StopPlayer();
            return;
        }

        if (Mathf.Abs(rigid.velocity.x) < 0.1)
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
            if (stopSoundCount == 0)
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
        rigid.velocity = Vector2.up * playerInfo.jumpPower;

        animator.SetBool("isRunning", false);

        animator.SetBool("isJumpingUp", true);

        isJumping = true;

        isLanded = false;

        SoundManager.instance.PlayPlayerSound(PlayerSounds.PLAYER_JUMP);
    }

    private void CheckIfJumping()
    {
        if (rigid.velocity.y < -0.0001f)  //플레이어가 아래로 떨어질때 Down Ray를 사용한다.
        {
            isJumping = true;
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumpingUp", false);
            animator.SetBool("isJumpingDown", true);

            Vector2 rightVec = new Vector2(rigid.position.x + 0.4f, rigid.position.y);
            Vector2 leftVec = new Vector2(rigid.position.x - 0.45f, rigid.position.y);

            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 2, LayerMask.GetMask("Platform"));  
            RaycastHit2D rayHit2 = Physics2D.Raycast(rightVec, Vector3.down, 2, LayerMask.GetMask("Platform"));  
            RaycastHit2D rayHit3 = Physics2D.Raycast(leftVec, Vector3.down, 2, LayerMask.GetMask("Platform"));  
            if (rayHit.collider != null || rayHit2.collider != null || rayHit3.collider != null)  
            {
                if (rayHit.distance < 1.9f || rayHit2.distance < 1.9f || rayHit3.distance < 1.9f)
                {
                    if (!isLanded)
                        SoundManager.instance.PlayPlayerSound(PlayerSounds.PLAYER_LAND);

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
        if ((animator.GetBool("isJumpingUp") || animator.GetBool("isJumpingDown")) && rigid.velocity.y == 0)
        {
            Vector2 rightVec = new Vector2(rigid.position.x + 0.4f, rigid.position.y);
            Vector2 leftVec = new Vector2(rigid.position.x - 0.45f, rigid.position.y);

            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 2, LayerMask.GetMask("Platform"));
            RaycastHit2D rayHit2 = Physics2D.Raycast(rightVec, Vector3.down, 2, LayerMask.GetMask("Platform"));
            RaycastHit2D rayHit3 = Physics2D.Raycast(leftVec, Vector3.down, 2, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null || rayHit2.collider != null || rayHit3.collider != null)
            {
                if (rayHit.distance < 1.8f || rayHit2.distance < 1.8f || rayHit3.distance < 1.8f)
                {
                    if (!isLanded)
                        SoundManager.instance.PlayPlayerSound(PlayerSounds.PLAYER_LAND);

                    isLanded = true;
                    isJumping = false;
                    animator.SetBool("isJumpingUp", false);
                    animator.SetBool("isJumpingDown", false);
                }
            }
        }
    }

    private void SetIsJumpingFalse()
    {
        isJumping = false;
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
