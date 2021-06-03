using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMite : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        // anum.SetBool("isWalking", false);
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(-1, rigid.velocity.y);
        anim.SetBool("isWalking", true);
    }
}
