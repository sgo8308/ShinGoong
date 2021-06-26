using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterOctopus : Monster
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
        if (rigid.bodyType == RigidbodyType2D.Static)
            return;

        rigid.velocity = new Vector2(nextMove * speed, rigid.velocity.y);

        CheckIfFloor();
    }
    private void CheckIfFloor()
    {
        Vector2 frontVecVertical = new Vector2(rigid.position.x + nextMove * 2, rigid.position.y);

        RaycastHit2D rayHit = Physics2D.Raycast(frontVecVertical,
                                Vector3.down, 2, LayerMask.GetMask("Platform"));

        if (rayHit.collider == null)
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

    protected override void ThinkAndMove()
    {
        nextMove = Random.Range(-1, 2);

        anim.SetInteger("WalkSpeed", nextMove);

        FlipSprite();

        FindOriginalPosition();

        //Recursive
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("ThinkAndMove", nextThinkTime);
    }

    void FindOriginalPosition()
    {
        if (transform.position.x <= _position.x + 1 &&
                transform.position.x >= _position.x - 1)
        {
            transform.rotation = _rotation;
            nextMove = 0;
            anim.SetInteger("WalkSpeed", nextMove);
            CancelInvoke("ThinkAndMove");
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

        CancelInvoke("ThinkAndMove");
        Invoke("ThinkAndMove", 3);
    }

    protected override void OnDetectPlayer()
    {
        GetAngry();
        CancelInvoke("GetPeaceful");
        Invoke("GetPeaceful", 5);
    }

    public override void OnHit(float damage)
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

        CancelInvoke("ThinkAndMove");
        Invoke("ThinkAndMove", 5);
    }

    public override void GetPeaceful()
    {
        radar.GetComponent<Image>().color = new Color(1, 1, 0, 0.5f);
        speed = 2;
        anim.speed = 1;
    }
}
