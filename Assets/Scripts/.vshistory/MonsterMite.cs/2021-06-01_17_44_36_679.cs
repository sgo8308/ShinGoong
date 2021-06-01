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
        //이동
        _rigid.velocity = new Vector2(nextMove * 3, _rigid.velocity.y);

        if (nextMove == 0)
        {
            _anim.SetBool("isWalking", false);
        }
        else
        {
            _anim.SetBool("isWalking", true);

            // 방향에 맞게 몬스터 스프라이트와 레이더 이미지를 돌려줌
            if (nextMove == 1) 
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

        //플랫폼 체크
        Vector2 frontVec = new Vector2(_rigid.position.x + nextMove, _rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0), 10.0f, false);

        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        
        if (rayHit.collider == null)
        {
            nextMove = nextMove * -1;
            CancelInvoke();
            Invoke("Think", 5);
        }

    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }
}
