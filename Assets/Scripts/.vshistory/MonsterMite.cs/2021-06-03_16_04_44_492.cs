using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MonsterMite : MonoBehaviour
{
    Animator _anim;
    Rigidbody2D _rigid;
    GameObject _radar;
    GameObject _hpBarFrame;
    Image _hpBar;
    float _speed;
    int _nextMove; // -1 , 0 , 1 -> left, stop, right
    float _hp;
    float _defensivePower;
    public GameObject coin;
    public UnityEvent OnDead;

    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        _radar = this.transform.Find("MonsterCanvas").transform
                                .Find("RadarImage").gameObject;

        _hpBarFrame = this.transform.Find("MonsterCanvas").transform
                                    .Find("HpBarFrame").gameObject;

        _hpBar = _hpBarFrame.transform.Find("HpBar").GetComponent<Image>();

        _speed = 1;
        _hp = 100;
        _defensivePower = 0;

        AddHitEvent();

        OnDead = new UnityEvent();

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
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                _hpBarFrame.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
                _hpBarFrame.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 0);
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
            CancelInvoke("GetPeaceful");
            Invoke("GetPeaceful", 5);
        }
    }

    public void GetAngry()
    {
        _radar.GetComponent<Image>().color = new Color(1, 0, 0, 0.75f);
        _speed = 3;
        if (_nextMove == 0 && this.transform.rotation == Quaternion.Euler(0, 0, 0))
        {
            _nextMove = 1;
        }
        else if (_nextMove == 0 && this.transform.rotation == Quaternion.Euler(0, 180, 0))
        {
            _nextMove = -1;
        }

        _anim.SetInteger("WalkSpeed", _nextMove);
        _anim.speed = 1.3f;

        CancelInvoke("Think");
        Invoke("Think", 3);
    }

    public void GetPeaceful()
    {
        _radar.GetComponent<Image>().color = new Color(1, 1, 0, 0.5f);
        _speed = 1;
        _anim.speed = 1;
    }
    #endregion

    #region When Monster get hit by arrow
    void OnHit(Collision2D collision)
    {
        if (collision.collider.tag == "arrow")
        {
            collision.gameObject.transform.parent = this.transform;

            Arrow arrow = collision.gameObject.GetComponent<Arrow>();
            HpReduced(arrow.GetDamage());

            GetAngry();

            CheckedIfDead();

            CancelInvoke("GetPeaceful");
            Invoke("GetPeaceful", 5);
        }
    }

    void AddHitEvent()
    {
        MonsterBody monsterBody = this.transform.Find("Body").GetComponent<MonsterBody>();
        monsterBody.OnMonsterHit.AddListener(OnHit);
    }

    void CheckedIfDead()
    {
        if ((_hp / 100) <= 0)
        {
            Destroy(_hpBarFrame);
            Dead();
        }
    }

    void HpReduced(float damage)
    {
        _hp -= damage - _defensivePower;
        _anim.SetFloat("Hp", _hp);

        if ((_hp / 100) <= 0)
        {
            _hpBar.fillAmount = 0;
        }
        else
        {
            _hpBar.fillAmount = _hp / 100;
        }
    }

    void Dead()
    {
        OnDead.Invoke();

        _speed = 0;

        CancelInvoke();

        Invoke("Destroy", 3);

        Instantiate(coin, this.transform.position, transform.rotation);

        Destroy(_radar);
    }

    void Destroy()
    {
        Destroy(this.gameObject);
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("콜리젼 들어옴");
    }
}
