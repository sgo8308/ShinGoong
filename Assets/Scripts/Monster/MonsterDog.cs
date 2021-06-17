using UnityEngine;
using UnityEngine.UI;

public class MonsterDog : Monster
{
    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    void FixedUpdate()
    {
        //Move
        rigid.velocity = new Vector2(nextMove * speed, rigid.velocity.y);

        CheckIfFloor();

        CheckIfWall();
    }

    private void CheckIfFloor()
    {
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0), 10.0f, false);

        RaycastHit2D rayHit = Physics2D.Raycast(frontVec,
                                Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider == null)
            Turn();
    }

    private void CheckIfWall()
    {
        Vector2 frontVecHorizontal = new Vector2(rigid.position.x + 2 * nextMove, rigid.position.y);
        Debug.DrawRay(frontVecHorizontal, 2 * Vector3.left, new Color(0, 1, 0), 10.0f, false);

        RaycastHit2D rayHit2 = Physics2D.Raycast(frontVecHorizontal,
                                2 * Vector3.left, 1, LayerMask.GetMask("Platform"));

        if (rayHit2.collider != null)
        {
            CancelInvoke();
            ThinkAndMove();
        }
    }

    protected override void Initialize()
    {
        speed = 1;
        hp = 100;
        defensivePower = 0;
        expPoint = 10.0f;
        Invoke("ThinkAndMove", 5);
    }
    protected override void OnDetectPlayer()
    {
        GetAngry();
        CancelInvoke("GetPeaceful");
        Invoke("GetPeaceful", 5);
    }

    #region Move
    protected override void ThinkAndMove()
    {
        nextMove = Random.Range(-1, 2);

        anim.SetInteger("WalkSpeed", nextMove);

        FlipSprite();

        //Recursive
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("ThinkAndMove", nextThinkTime);
    }

    void Turn()
    {
        nextMove = nextMove * -1;
        FlipSprite();

        CancelInvoke("ThinkAndMove");
        Invoke("ThinkAndMove", 3);
    } 
    #endregion

    #region When Monster get hit by arrow
    protected override void OnHit(float damage)
    {
        ReduceHp(damage);

        hpBarFrame.SetActive(true);
        CancelInvoke("HideHpBarFrame");
        Invoke("HideHpBarFrame", 3);

        if (hp <= 30)
            GetAngry();

        CheckIfDead();

        CancelInvoke("GetPeaceful");
        Invoke("GetPeaceful", 5);
    }

    public override void GetAngry()
    {
        radar.GetComponent<Image>().color = new Color(1, 0, 0, 0.75f);
        speed = 3;
        if (nextMove == 0 && this.transform.rotation == Quaternion.Euler(0, 0, 0))
        {
            nextMove = 1;
        }
        else if (nextMove == 0 && this.transform.rotation == Quaternion.Euler(0, 180, 0))
        {
            nextMove = -1;
        }

        anim.SetInteger("WalkSpeed", nextMove);
        anim.speed = 1.3f;

        CancelInvoke("ThinkAndMove");
        Invoke("ThinkAndMove", 3);
    }

    public override void GetPeaceful()
    {
        radar.GetComponent<Image>().color = new Color(1, 1, 0, 0.5f);
        speed = 1;
        anim.speed = 1;
    }
    public override void Dead()
    {
        base.Dead();
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Instantiate(coin, this.transform.position, transform.rotation);
        CancelInvoke();
        Invoke("Destroy", 0.8f);

    }
    #endregion


}
