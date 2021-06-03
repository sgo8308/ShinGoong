using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMite : MonoBehaviour
{
    Animator _anim;
    Rigidbody2D rigid;
    public int nextMove
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    void Start()
    {
        // anum.SetBool("isWalking", false);
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(-1, rigid.velocity.y);
        _anim.SetBool("isWalking", true);
    }
}
