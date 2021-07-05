using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 보스 몬스터에 붙어 있는 스크립트 
/// </summary>
public class MonsterBoss : Monster
{
    public List<Transform> platformListToGo;
    public Transform nowPlatform;
    public Transform platformToGo;

    public float flySpeed;
    public float floatSpeed;

    public bool isFlying = false;
    public bool isFloating = false;
    public bool isHit = false;

    private int direction;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    protected override void Initialize()
    {
        speed = 2;
        flySpeed = 0.15f;
        floatSpeed = 0.05f;
        hp = 100;
        defensivePower = 50;
        expPoint = 70.0f;
    }

    private void Start()
    {
        Invoke("ThinkAndMove", 5);
    }

    void FixedUpdate()
    {
        if (Player.isDead)
        {
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            CancelInvoke();
        }

        if (isFloating || isFlying)
            return;

        if (rigid.bodyType == RigidbodyType2D.Static)
            return;

        rigid.velocity = new Vector2(direction * speed, rigid.velocity.y);
    }

    /// <summary>
    /// 왼쪽으로 움직이기
    /// 오른쪽으로 움직이기
    /// 멈춰서 쉬기
    /// 다른 플랫폼으로 날아가기
    /// 날아가다가 맞으면 방향 바꿔서 플레이어가 있는 플랫폼으로 날아가기
    /// 중에 하나를 랜덤으로 선택해서 행동한다.
    /// </summary>

    protected override void ThinkAndMove()
    {
        CancelInvoke("ThinkAndMove");

        nextMove = ChooseBehaviour();

        switch (nextMove)
        {
            case -1:
                MoveLeft();
                break;
            case 0:
                Stop();
                break;
            case 1:
                MoveRight();
                break;
            case 2:
                if (!CheckIfFalling()) // Don't Try Flying When Falling
                    RunFlyRoutine(true);
                else
                    Invoke("ThinkAndMove", 0.5f);
                return;
            case 3:
                RunFlyRoutine(false);
                return;
            default:
                break;
        }

        FlipSprite();

        float nextThinkTime = Random.Range(3f, 6f);
        Invoke("ThinkAndMove", nextThinkTime);
    }

    private int ChooseBehaviour()
    {
        if (isHit)
        {
            if (isFloating || isFlying)
                return 3;

            switch (FindPlayer())
            {
                case "left":
                    return -1;
                case "right":
                    return 1;
                case "other platform":
                    return 2;
                default:
                    break;
            }
        }
        
        return Random.Range(-1, 3);
    }

    private void MoveLeft()
    {
        direction = -1;
        anim.enabled = true;
        anim.SetBool("isWalking", true);
    }

    private void MoveRight()
    {
        direction = 1;
        anim.enabled = true;
        anim.SetBool("isWalking", true);
    }

    private void Stop()
    {
        direction = 0;
        anim.enabled = true;
        anim.SetBool("isWalking", false);
    }

    /// <summary>
    /// 위로 뜨기, 날기, 착지하기의 비행루틴
    /// </summary>
    /// <param name="isOnTheGround"></param>
    private void RunFlyRoutine(bool isOnTheGround)
    {
        ChoosePlatformToGo();

        ShowBubble();

        if (isOnTheGround)
        {
            isFloating = true;
            Invoke("Float", 0.7f);
            Invoke("SetIsFloatingFalse", 1.5f);
            return;
        }

        SetIsFloatingFalse();
    }

    /// <summary>
    /// 날기 시작할 때 이동할 플랫폼을 선택하는 메소드 
    /// </summary>
    private void ChoosePlatformToGo()
    {
        if (isHit)
        {
            platformToGo = StageManager.instance.platformPlayerSteppingOn;
            return;
        }

        while (true)
        {
            int platFormNumToGo = Random.Range(0, 8);

            if (nowPlatform == null)
                nowPlatform = platformListToGo[0];

            if (nowPlatform.name != platformListToGo[platFormNumToGo].name)
            {
                platformToGo = platformListToGo[platFormNumToGo];
                break;
            }
        }
    }

    private void Float()
    {
        if (!isFloating)
        {
            isFlying = true;
            Invoke("Fly", 1.0f);
            return;
        }

        direction = 0;

        GetComponent<Rigidbody2D>().gravityScale = 0;

        Vector3 upSide = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position,
                                    upSide, floatSpeed);

        anim.SetBool("isWalking", false);
        anim.enabled = false;

        Invoke("Float", 0.02f);
    }

    private void Fly()
    {
        Vector3 targetPosToFly = new Vector3(platformToGo.position.x, platformToGo.position.y + 3,
                                transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position,
                                    targetPosToFly, flySpeed);

        if (targetPosToFly.x > transform.position.x)
            FlipSpriteToRight();
        else if (targetPosToFly.x < transform.position.x)
            FlipSpriteToLeft();

        GetComponent<BoxCollider2D>().isTrigger = true;
        transform.Find("Body").GetComponent<PolygonCollider2D>().isTrigger = true;

        anim.enabled = true;
        anim.SetBool("isFlying", true);

        if (transform.position == targetPosToFly)
        {
            isFlying = false;
            Invoke("Land", 0.5f);
            return;
        }

        Invoke("Fly", 0.02f);
    }

    private void Land()
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
        transform.Find("Body").GetComponent<PolygonCollider2D>().isTrigger = false;
        GetComponent<Rigidbody2D>().gravityScale = 1;

        anim.SetBool("isFlying", false);
        anim.enabled = false;

        Invoke("ThinkAndMove", 1.0f);
    }

    protected override void OnDetectPlayer()
    {
        GetAngry();
        CancelInvoke("GetPeaceful");
        Invoke("GetPeaceful", 7);
    }

    private bool CheckIfFalling()
    {
        if (isFloating || isFlying)
            return false;

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 3, LayerMask.GetMask("Platform"));

        if (rayHit.collider == null)
            return true;

        return false;
    }

    #region When Monster gets hit
    public override void OnHit(float damage)
    {
        isHit = true;

        ReduceHp(damage);

        hpBarFrame.SetActive(true);
        CancelInvoke("HideHpBarFrame");
        Invoke("HideHpBarFrame", 3);

        CheckIfDead();

        GetAngry();

        CancelInvoke("ThinkAndMove");
        ThinkAndMove();

        CancelInvoke("GetPeaceful");
        Invoke("GetPeaceful", 7);
    }

    private string FindPlayer()
    {
        if (StageManager.instance.platformPlayerSteppingOn.name == nowPlatform.name)
        {
            if (StageManager.instance.player.transform.position.x >= transform.position.x)
                return "right";

            if (StageManager.instance.player.transform.position.x < transform.position.x)
                return "left";
        }

        return "other platform";
    }

    public override void GetAngry()
    {
        radar.GetComponent<Image>().color = new Color(1, 0, 0, 0.75f);
        speed = 3;
        flySpeed = 0.13f;
    }

    public override void GetPeaceful()
    {
        isHit = false;
        radar.GetComponent<Image>().color = new Color(1, 1, 0, 0.5f);
        speed = 2;
    }
    #endregion

    /// <summary>
    /// Show bubble on platform to go
    /// </summary>
    private void ShowBubble()
    {
        if (platformToGo == null)
            return;

        platformToGo.GetComponent<Platform>().ShowBubble();
    }

    private void SetIsFloatingFalse()
    {
        isFloating = false;
    }
}
