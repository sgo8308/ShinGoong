using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterMite : MonoBehaviour
{
    Animator _anim;
    Rigidbody2D _rigid;
    SpriteRenderer _spriteRenderer;
    GameObject _radar;
    RectTransform _radarRectTranform;
    GameObject _hpBarFrame;
    Image _hpBar;
    float _speed;
    int _nextMove; // -1 , 0 , 1 -> left, stop, right
    float _hp;

    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _radar = this.transform.Find("MonsterCanvas").transform
                                .Find("RadarImage").gameObject;

        _radarRectTranform = _radar.GetComponent<RectTransform>();

        _hpBarFrame = this.transform.Find("MonsterCanvas").transform
                                    .Find("HpBarFrame").gameObject;

        _hpBar = _hpBarFrame.transform.Find("HpBar").GetComponent<Image>();

        _speed = 1;
        _hp = 100;

        Invoke("Think", 5); // think per 5 seconds
    }

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
        {
            Turn();
        }
    }
    #region Move
    void Think()
    {
        _nextMove = Random.Range(-1, 2);

        _anim.SetInteger("WalkSpeed", _nextMove);

        FlipMoster();

        //Recursive
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        _nextMove = _nextMove * -1;
        FlipMoster();

        CancelInvoke("Think");
        Invoke("Think", 3);
    }

    void FlipMoster()
    {
        // Flip Monster Sprite and Radar image 
        if (_nextMove != 0)
        {
            if (_nextMove == 1)
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
    #endregion

    #region RadarDetection
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            GetAngry();
            Invoke("GetPeaceful", 5);
        }
    }

    void GetAngry()
    {
        _radar.GetComponent<Image>().color = new Color(1, 0, 0, 0.75f);
        _speed = 3;
        if (_nextMove == 0 && _spriteRenderer.flipX)
        {
            _nextMove = -1;
        }
        else if (_nextMove == 0 && !_spriteRenderer.flipX)
        {
            _nextMove = 1;
        }

        _anim.SetInteger("WalkSpeed", _nextMove);
        _anim.speed = 1.3f;

        CancelInvoke("Think");
        Invoke("Think", 3);
    }

    void GetPeaceful()
    {
        _radar.GetComponent<Image>().color = new Color(1, 1, 0, 0.5f);
        _speed = 1;
        _anim.speed = 1;
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "arrow")
        {
            Arrow arrow = collision.gameObject.GetComponent<Arrow>();

            Hit(arrow.GetDamage());


        }
    }

    void Hit(float damage)
    {
        _hp -= damage;
        _anim.SetFloat("Hp", _hp);

        if ((_hp / 100) <= 0 )
        {
            _hpBar.fillAmount = 0;
            Destroy(_hpBarFrame);
            Dead();
        }
        else
        {
            _hpBar.fillAmount = _hp / 100;
        }
    }

    void Dead()
    {
        _speed = 0;
        CancelInvoke("Think");
        Invoke("Destroy", 3);
        Destroy(_radar);
    }

    void Destroy()
    {
        Destroy(this.gameObject);
    }
}
