using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombShot : Skill
{
    public Rigidbody2D rigid;
    void Awake()
    {
        damage = 30;

        rigid.bodyType = RigidbodyType2D.Dynamic;
    }

    private void Start()
    {
        rigid.bodyType = RigidbodyType2D.Kinematic;
    }
}
