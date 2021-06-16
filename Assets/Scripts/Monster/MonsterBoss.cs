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
    public Vector3 targetPosToFly;
    public float flySpeed;
    public float floatSpeed;
    public bool isFlying = false;
    public bool isFloating = false;
    public int direction;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    public int rayDistance;

    private void Start()
    {
        LoopTest();
    }

    void FixedUpdate()
    {
        if (isFloating || isFlying)
            return;

        //Move
        rigid.velocity = new Vector2(direction * speed, rigid.velocity.y);

        ////Check - Wall
        //Vector2 frontVecHorizontal = new Vector2(rigid.position.x + 2 * nextMove, rigid.position.y);
        //Debug.DrawRay(frontVecHorizontal, 2 * Vector3.left, new Color(0, 1, 0), 10.0f, false);

        //RaycastHit2D rayHit2 = Physics2D.Raycast(frontVecHorizontal,
        //                        2 * Vector3.left, 1, LayerMask.GetMask("Platform"));

        //if (rayHit2.collider != null)
        //{
        //    CancelInvoke();
        //    LoopTest();
        //}
    }

    protected override void Initialize()
    {
        speed = 2;
        flySpeed = 0.15f;
        floatSpeed = 0.05f;
        hp = 100;
        defensivePower = 65;
        expPoint = 70.0f;
        //Invoke("ThinkAndWalkAround", 5);
    }

    protected override void OnDetectPlayer()
    {
        //GetAngry();
        //CancelInvoke("GetPeaceful");
        //Invoke("GetPeaceful", 5);
    }

    protected override void ThinkAndWalkAround()
    {
        //nextMove = Random.Range(2, 3);


        //if (nextMove == 2)
        //{
        //    ChoosePlatformToGo(false);
        //    isFloating = true;
        //    GetComponent<Animator>().enabled = false;
        //    return;
        //}

        //FlipSprite();

        ////Recursive
        //float nextThinkAndWalkAroundTime = Random.Range(3f, 6f);
        //Invoke("ThinkAndWalkAround", nextThinkAndWalkAroundTime);
    }

    private void LoopTest()
    {
        nextMove = Random.Range(2, 3);

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
                ChoosePlatformToGo(false);
                isFloating = true;
                Invoke("Float", 0.7f);
                Invoke("SetIsFloatingFalse", 1.5f);
                return;

            default:
                break;
        }

        FlipSprite();

        float nextThinkAndWalkAroundTime = Random.Range(3f, 6f);
        Invoke("LoopTest", nextThinkAndWalkAroundTime);
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

    private void ChoosePlatformToGo(bool isHit)
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
        if (transform.position == targetPosToFly)
        {
            isFlying = false;
            Invoke("Land", 0.5f);
            return;
        }

        targetPosToFly = new Vector3(platformToGo.position.x, platformToGo.position.y + 3,
                                transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position,
                                    targetPosToFly, flySpeed);

        if (targetPosToFly.x > transform.position.x)
            FlipSpriteToRight();
        else if (targetPosToFly.x < transform.position.x)
            FlipSpriteToLeft();

        anim.enabled = true;
        anim.SetBool("isFlying", true);

        Invoke("Fly", 0.02f);
    }

    private void Land()
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
        transform.Find("Body").GetComponent<PolygonCollider2D>().isTrigger = false;
        GetComponent<Rigidbody2D>().gravityScale = 1;

        anim.SetBool("isFlying", false);
        anim.enabled = false;

        Invoke("LoopTest", 1.0f);
    }

    #region When Monster get hit by arrow
    protected override void OnHit(float damage)
    {
        ReduceHp(damage);

        hpBarFrame.SetActive(true);
        CancelInvoke("HideHpBarFrame");
        Invoke("HideHpBarFrame", 3);

        //---- 여기부터 보스 행동 
        GetAngry();

        if (isFloating || isFlying)
        {
            ChoosePlatformToGo(true);
            SetIsFloatingFalse();
        }

        CheckIfDead();

        CancelInvoke("GetPeaceful");
        Invoke("GetPeaceful", 5);
    }

    public override void GetAngry()
    {
        radar.GetComponent<Image>().color = new Color(1, 0, 0, 0.75f);
        speed = 3;
        flySpeed = 0.17f;

        //anim.SetInteger("WalkSpeed", nextMove);
        //anim.speed = 1.0f;

        //CancelInvoke("ThinkAndWalkAround");
        //Invoke("ThinkAndWalkAround", 3);
    }

    public override void GetPeaceful()
    {
        //radar.GetComponent<Image>().color = new Color(1, 1, 0, 0.5f);
        //speed = 2;
        //anim.speed = 1;
    }

    public override void Dead()
    {
        base.Dead();
        Instantiate(coin, this.transform.position, transform.rotation);
    }
    #endregion

    private void SetIsFloatingFalse()
    {
        isFloating = false;
    }
}
