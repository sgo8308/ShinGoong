using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Arrow Prefab의 폭발 샷 애니메이션을 보여주는 child gameobject에 붙어 있는 스크립트. 
/// </summary>
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
