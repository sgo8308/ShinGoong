﻿using UnityEngine;
using UnityEngine.UI;

public class MonsterMite : Monster
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

        //Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0), 10.0f, false);

        RaycastHit2D rayHit = Physics2D.Raycast(frontVec,
                                Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider == null)
            Turn();
    }

    protected override void Initialize()
    {
        speed = 1;
        hp = 100;
        defensivePower = 0;

        Invoke("Think", 5);
    }

    protected override void Think()
    {
        nextMove = Random.Range(-1, 2);

        anim.SetInteger("WalkSpeed", nextMove);

        FlipSprite();

        //Recursive
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove = nextMove * -1;
        FlipSprite();

        CancelInvoke("Think");
        Invoke("Think", 3);
    }

    protected override void OnDetectPlayer()
    {
        GetAngry();
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

        CancelInvoke("Think");
        Invoke("Think", 3);
    }

    public override void GetPeaceful()
    {
        radar.GetComponent<Image>().color = new Color(1, 1, 0, 0.5f);
        speed = 1;
        anim.speed = 1;
    }

    protected override void Dead()
    {
        base.Dead();
        Instantiate(coin, this.transform.position, transform.rotation);
    }
}