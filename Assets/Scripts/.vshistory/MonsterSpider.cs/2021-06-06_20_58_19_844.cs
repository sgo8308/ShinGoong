using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSpider : Monster
{
    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    void FixedUpdate()
    {
        //Move
        _rigid.velocity = new Vector2(_nextMove * _speed, _rigid.velocity.y);

        //Platform Check
        Vector2 frontVec = new Vector2(_rigid.position.x + _nextMove, _rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0), 10.0f, false);

        RaycastHit2D rayHit = Physics2D.Raycast(frontVec,
                                Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider == null)
            Turn();
    }

    protected override void Initialize()
    {
        _speed = 0;
        _hp = 100;
        _defensivePower = 30;

        Invoke("Think", 5);
    }

    protected override void Think()
    {
        //_nextMove = Random.Range(-1, 2);

        _nextMove = 0;

        _anim.SetInteger("WalkSpeed", _nextMove);

        FlipSprite();

        //Recursive
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        _nextMove = _nextMove * -1;
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
        _radar.GetComponent<Image>().color = new Color(1, 0, 0, 0.75f);
        _speed = 4;
        if (_nextMove == 0 && this.transform.rotation == Quaternion.Euler(0, 0, 0))
        {
            _nextMove = 1;
        }
        else if (_nextMove == 0 && this.transform.rotation == Quaternion.Euler(0, 180, 0))
        {
            _nextMove = -1;
        }

        _anim.SetInteger("WalkSpeed", _nextMove);
        _anim.speed = 1.3f;

        CancelInvoke("Think");
        Invoke("Think", 3);
    }

    public override void GetPeaceful()
    {
        _radar.GetComponent<Image>().color = new Color(1, 1, 0, 0.5f);
        _speed = 0;
        _anim.speed = 1;
    }

    protected override void Dead()
    {
        base.Dead();
        Instantiate(coin, this.transform.position, transform.rotation);
    }
}
