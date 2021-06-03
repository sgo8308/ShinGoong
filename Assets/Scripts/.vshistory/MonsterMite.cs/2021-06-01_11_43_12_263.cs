using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMite : MonoBehaviour
{
    Animator _anim;
    Rigidbody2D _rigid;
    public int nextMove;

    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    void Start()
    {
        // anum.SetBool("isWalking", false);
    }

    void FixedUpdate()
    {
        _rigid.velocity = new Vector2(-1, _rigid.velocity.y);
        _anim.SetBool("isWalking", true);
    }
    
    void Think()
    {
        int random = Random.Range(-1, 2);
    }
}
