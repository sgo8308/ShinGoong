using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMite : MonoBehaviour
{
    Animator _anim;
    Rigidbody2D _rigid;
    SpriteRenderer _spriteRenderer;
    RectTransform _radarRectTranform;
    public int nextMove; // -1 , 0 , 1 로 왼쪽 정지 오른쪽을 가리킴

    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();  
        _radarRectTranform = this.transform
                                .Find("MonsterCanvas").transform
                                .Find("RadarImage").GetComponent<RectTransform>();

        Invoke("Think", 5); // 5초마다 다음 행동 판단
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

            if (nextMove == 1) // 방향에 맞게 몬스터 스프라이트와 레이더 이미지를 돌려줌
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

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        Invoke("Think", 5);
    }
}
