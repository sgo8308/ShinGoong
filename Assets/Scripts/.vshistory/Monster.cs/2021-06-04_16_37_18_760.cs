using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Monster : MonoBehaviour
{
    GameObject _radar;
    GameObject _hpBarFrame;
    Image _hpBar;
    protected Animator _anim;
    protected Rigidbody2D _rigid;
    protected int _nextMove; // -1 , 0 , 1 -> left, stop, right
    protected float _speed;
    protected float _hp;
    protected float _defensivePower;
    public GameObject coin;
    public UnityEvent OnDead;

    virtual protected void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        _radar = this.transform.Find("MonsterCanvas").transform
                                .Find("RadarImage").gameObject;

        _hpBarFrame = this.transform.Find("MonsterCanvas").transform
                                    .Find("HpBarFrame").gameObject;

        _hpBar = _hpBarFrame.transform.Find("HpBar").GetComponent<Image>();

        OnDead = new UnityEvent();

        AddHitEvent();
    }

    protected abstract void Initialize();

    #region Move
    abstract protected void Think();

    protected void FlipSprite()
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

    abstract public void GetAngry();

    abstract public void GetPeaceful();
    
    #endregion

    #region When Monster get hit by arrow
    void OnHit(Collision2D collision)
    {
        if (collision.collider.tag == "Arrow" && gameObject.tag == "Monster")
        {
            collision.gameObject.transform.parent = this.transform;

            _hpBarFrame.SetActive(true);
            CancelInvoke("HideHpBarFrame");
            Invoke("HideHpBarFrame", 3);

            Arrow arrow = collision.gameObject.GetComponent<Arrow>();
            ReduceHp(arrow.damage);

            GetAngry();

            CheckIfDead();

            CancelInvoke("GetPeaceful");
            Invoke("GetPeaceful", 5);
        }
    }

    void AddHitEvent()
    {
        MonsterBody monsterBody = this.transform.Find("Body").GetComponent<MonsterBody>();
        monsterBody.OnMonsterHit.AddListener(OnHit);
    }

    void CheckIfDead()
    {
        if ((_hp / 100) <= 0)
        {
            Destroy(_hpBarFrame);
            Dead();
        }
    }

    void ReduceHp(float damage)
    {
        _hp -= damage - _defensivePower;
        _anim.SetFloat("Hp", _hp);

        if ((_hp / 100) <= 0)
            _hpBar.fillAmount = 0;
        else
            _hpBar.fillAmount = _hp / 100;
    }

    void Dead()
    {
        OnDead.Invoke();

        gameObject.tag = "Untagged";
        gameObject.transform.Find("Body").tag = "Untagged";

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

    void HideHpBarFrame()
    {
        _hpBarFrame.SetActive(false);
    }
    #endregion
}
