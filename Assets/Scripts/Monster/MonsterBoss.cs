using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterBoss : Monster
{
    public List<Transform> platformListToGo;
    public Transform nowPlatform;
    public Transform platformToGo;
    /// <summary>
    /// Position over the platform that monster wants to go.
    /// </summary>
    public float flySpeed;
    public float floatSpeed;
    private bool isFlying = false;
    private bool isFloating = false;
    private bool isHit = false;
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
        if (isFloating || isFlying)
            return;

        //Move
        rigid.velocity = new Vector2(direction * speed, rigid.velocity.y);

        CheckIfWall();
    }

    private void CheckIfWall()
    {
        Vector2 frontVecHorizontal = new Vector2(rigid.position.x + 2 * direction, rigid.position.y);
        Debug.DrawRay(frontVecHorizontal, 2 * Vector3.left, new Color(0, 1, 0), 10.0f, false);

        RaycastHit2D rayHit2 = Physics2D.Raycast(frontVecHorizontal,
                                2 * Vector3.left, 1, LayerMask.GetMask("Platform"));

        if (rayHit2.collider != null)
        {
            CancelInvoke();
            ThinkAndMove();
        }
    }

    /// <summary>
    /// 왼쪽으로 움직이기
    /// 오른쪽으로 움직이기
    /// 멈춰서 쉬기
    /// 다른 플랫폼으로 날아가기
    /// 날아가다가 방향 바꿔서 플레이어가 있는 플랫폼으로 날아가기
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
                RunFlyRoutine(true);
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

    private void RunFlyRoutine(bool isOnTheGround)
    {
        ChoosePlatformToGo();
        
        if (isOnTheGround)
        {
            isFloating = true;
            Invoke("Float", 0.7f);
            Invoke("SetIsFloatingFalse", 1.5f);
            return;
        }

        SetIsFloatingFalse();
    }

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

        GetComponent<BoxCollider2D>().isTrigger = true;
        transform.Find("Body").GetComponent<PolygonCollider2D>().isTrigger = true;
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

    #region When Monster get hit by arrow
    protected override void OnHit(float damage)
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

    public override void Dead()
    {
        base.Dead();
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Instantiate(coin, this.transform.position, transform.rotation);
        CancelInvoke();
        Invoke("Destroy", 1);

    }
    #endregion

    private void SetIsFloatingFalse()
    {
        isFloating = false;
    }
}
