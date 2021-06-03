using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMite : MonoBehaviour
{
    Animator _anim;
    Rigidbody2D _rigid;
    SpriteRenderer _spriteRenderer;
    RectTransform _radarRectTranform;
    int _nextMove; // -1 , 0 , 1 -> left, stop, right

    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();  
        _radarRectTranform = this.transform
                                .Find("MonsterCanvas").transform
                                .Find("RadarImage").GetComponent<RectTransform>();

        Invoke("Think", 5); // think per 5 seconds
    }

    void FixedUpdate()
    {
        //Move
        _rigid.velocity = new Vector2(_nextMove, _rigid.velocity.y);

        //Platform Check
        Vector2 frontVec = new Vector2(_rigid.position.x + _nextMove, _rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0), 10.0f, false);

        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        
        if (rayHit.collider == null)
        {
            Turn();
        }
    }

    void Think()
    {
        //Set Next Move
        _nextMove = Random.Range(-1, 2);

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

        CancelInvoke();
        Invoke("Think", 3);
    }

    void FlipSprite()
    {
        // Flip Monster Sprite and Radar image 
        if (_nextMove == 1)
        {
            _spriteRenderer.flipX = true;
            _radarRectTranform.rotation =
                Quaternion.Euler(_radarRectTranform.rotation.x,
                                 0, _radarRectTranform.rotation.z);
        }
        else
        {
            _spriteRenderer.flipX = false;
            _radarRectTranform.rotation =
                Quaternion.Euler(_radarRectTranform.rotation.x,
                                 180, _radarRectTranform.rotation.z);
        }
    }
}
