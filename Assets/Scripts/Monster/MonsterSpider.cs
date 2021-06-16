using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSpider : Monster
{
    Vector3 _position;
    Quaternion _rotation;
    public float rayLength;
    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    void FixedUpdate()
    {
        //Move
        rigid.velocity = new Vector2(nextMove * speed, rigid.velocity.y);

        //Check - floor
        Vector2 frontVecVertical = new Vector2(rigid.position.x + nextMove * 2, rigid.position.y);
        Debug.DrawRay(frontVecVertical, Vector3.down, new Color(0, 1, 0), 0.1f, false);

        RaycastHit2D rayHit = Physics2D.Raycast(frontVecVertical,
                                Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider == null)
            Turn();

        //Check - Wall
        Vector2 frontVecHorizontal = new Vector2(rigid.position.x + 2 * nextMove, rigid.position.y);
        Debug.DrawRay(frontVecHorizontal, 2 * Vector3.left, new Color(0, 1, 0), 10.0f, false);

        RaycastHit2D rayHit2 = Physics2D.Raycast(frontVecHorizontal,
                                2 * Vector3.left, 1, LayerMask.GetMask("Platform"));

        if (rayHit2.collider != null)
            Turn();
    }

    protected override void Initialize()
    {
        _position = transform.position;
        _rotation = transform.rotation;

        speed = 2;
        hp = 100;
        expPoint = 30.0f;
        defensivePower = 30;
    }

    protected override void ThinkAndWalkAround()
    {
        nextMove = Random.Range(-1, 2);

        anim.SetInteger("WalkSpeed", nextMove);

        FlipSprite();

        FindOriginalPosition();

        //Recursive
        float nextThinkAndWalkAroundTime = Random.Range(2f, 5f);
        Invoke("ThinkAndWalkAround", nextThinkAndWalkAroundTime);
    }

    void FindOriginalPosition()
    {
        if (transform.position.x <= _position.x + 1 &&
                transform.position.x >= _position.x - 1)
        {
            transform.rotation = _rotation;
            nextMove = 0;
            anim.SetInteger("WalkSpeed", nextMove);
            CancelInvoke("ThinkAndWalkAround");
        }

        if (nextMove != 0) 
        {
            CancelInvoke("FindOriginalPosition");
            Invoke("FindOriginalPosition", 0.5f);
        }
    }

    void Turn()
    {
        nextMove = nextMove * -1;
        FlipSprite();

        CancelInvoke("ThinkAndWalkAround");
        Invoke("ThinkAndWalkAround", 3);
    }

    protected override void OnDetectPlayer()
    {
        GetAngry();
        CancelInvoke("GetPeaceful");
        Invoke("GetPeaceful", 5);
    }

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
        speed = 4;
        if (nextMove == 0 && _rotation == Quaternion.Euler(0, 0, 0))
            nextMove = 1;
        else if (nextMove == 0 && _rotation == Quaternion.Euler(0, 180, 0))
            nextMove = -1;

        anim.SetInteger("WalkSpeed", nextMove);
        anim.speed = 1.5f;

        CancelInvoke("ThinkAndWalkAround");
        Invoke("ThinkAndWalkAround", 5);
    }

    public override void GetPeaceful()
    {
        radar.GetComponent<Image>().color = new Color(1, 1, 0, 0.5f);
        speed = 2;
        anim.speed = 1;
    }

    public override void Dead()
    {
        base.Dead();
        Instantiate(coin, this.transform.position, transform.rotation);
    }
}
