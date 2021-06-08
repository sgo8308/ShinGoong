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
        _rigid.velocity = new Vector2(_nextMove * _speed, _rigid.velocity.y);

        //Check - floor
        Vector2 frontVecVertical = new Vector2(_rigid.position.x + _nextMove * 2, _rigid.position.y);
        Debug.DrawRay(frontVecVertical, Vector3.down, new Color(0, 1, 0), 0.1f, false);

        RaycastHit2D rayHit = Physics2D.Raycast(frontVecVertical,
                                Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider == null)
            Turn();

        //Check - Wall
        Vector2 frontVecHorizontal = new Vector2(_rigid.position.x + 2 * _nextMove, _rigid.position.y);
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

        _speed = 2;
        _hp = 100;
        _defensivePower = 30;
    }

    protected override void Think()
    {
        _nextMove = Random.Range(-1, 2);

        _anim.SetInteger("WalkSpeed", _nextMove);

        FlipSprite();

        FindOriginalPosition();

        //Recursive
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void FindOriginalPosition()
    {
        if (transform.position.x <= _position.x + 1 &&
                transform.position.x >= _position.x - 1)
        {
            transform.rotation = _rotation;
            _nextMove = 0;
            _anim.SetInteger("WalkSpeed", _nextMove);
        }

        if (_nextMove != 0) 
        {
            CancelInvoke("FindOriginalPosition");
            Invoke("FindOriginalPosition", 0.5f);
        }
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
        if (_nextMove == 0 && _rotation == Quaternion.Euler(0, 0, 0))
            _nextMove = 1;
        else if (_nextMove == 0 && _rotation == Quaternion.Euler(0, 180, 0))
            _nextMove = -1;

        _anim.SetInteger("WalkSpeed", _nextMove);
        _anim.speed = 1.5f;

        CancelInvoke("Think");
        Invoke("Think", 5);
    }

    public override void GetPeaceful()
    {
        _radar.GetComponent<Image>().color = new Color(1, 1, 0, 0.5f);
        _speed = 1;
        _anim.speed = 1;
    }

    protected override void Dead()
    {
        base.Dead();
        Instantiate(coin, this.transform.position, transform.rotation);
    }
}
