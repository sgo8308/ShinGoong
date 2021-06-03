using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMite : MonoBehaviour
{
    Animator _anim;
    Rigidbody2D _rigid;
    SpriteRenderer _spriteRenderer;
    public int nextMove;

    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("Think", 5);
    }

    void Start()
    {
        // anum.SetBool("isWalking", false);
    }

    void FixedUpdate()
    {
        _rigid.velocity = new Vector2(nextMove, _rigid.velocity.y);
        if (nextMove == 0)
        {
            _anim.SetBool("isWalking", false);
        }
        else
        {
            _anim.SetBool("isWalking", true);

            if (nextMove == 1)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }
        }
    
    void Think()
    {
        nextMove = Random.Range(-1, 2);
        
        Invoke("Think", 5);
    }
}
