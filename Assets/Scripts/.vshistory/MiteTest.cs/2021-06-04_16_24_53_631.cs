﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiteTest : Monster
{
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
        _speed = 1;
        _hp = 100;
        _defensivePower = 0;

        Invoke("Think", 5);
    }
}
