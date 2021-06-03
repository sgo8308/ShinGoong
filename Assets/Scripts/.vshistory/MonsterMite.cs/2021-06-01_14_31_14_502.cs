using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMite : MonoBehaviour
{
    Animator _anim;
    Rigidbody2D _rigid;
    SpriteRenderer _spriteRenderer;
    RectTransform radarRectTranform;
    public int nextMove;

    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        radarRectTranform = this.gameObject.transform.Find("MonsterCanvas").transform
                                                     .Find("RadarImage").GetComponent<RectTransform>();
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
                radarRectTranform.rotation = 
                    Quaternion.Euler(radarRectTranform.rotation.x, 0, radarRectTranform.rotation.z);
            }
            else
            {
                _spriteRenderer.flipX = false;
                radarRectTranform.rotation = 
                    Quaternion.Euler(radarRectTranform.rotation.x, 180, radarRectTranform.rotation.z);
            }
        }  
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        Invoke("Think", 5);
    }
}
