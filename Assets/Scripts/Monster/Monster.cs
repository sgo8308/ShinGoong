﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Monster : MonoBehaviour
{
    GameObject hpBarFrame;
    Image hpBar;
    protected GameObject radar;
    protected Animator anim;
    protected Rigidbody2D rigid;
    protected int nextMove; // -1 , 0 , 1 -> left, stop, right
    protected float speed;
    protected float hp;
    protected float defensivePower;
    protected float experiencePoint;
    public GameObject coin;
    public UnityEvent OnDead;

    virtual protected void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        radar = this.transform.Find("MonsterCanvas").transform
                                .Find("RadarImage").gameObject;

        hpBarFrame = this.transform.Find("MonsterCanvas").transform
                                    .Find("HpBarFrame").gameObject;

        hpBar = hpBarFrame.transform.Find("HpBar").GetComponent<Image>();

        OnDead = new UnityEvent();

        AddHitEvent();
    }

    protected abstract void Initialize();

    #region Move
    abstract protected void Think();

    protected void FlipSprite()
    {
        // Flip Monster Sprite and Radar image 
        if (nextMove != 0)
        {
            if (nextMove == 1)
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                hpBarFrame.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
                hpBarFrame.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 0);
            }
        }
    }
    #endregion

    #region RadarDetection
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            OnDetectPlayer();
        }
    }

    abstract protected void OnDetectPlayer();

    abstract public void GetAngry();

    abstract public void GetPeaceful();
    
    #endregion

    #region When Monster get hit by arrow
    void OnHit(Collision2D collision)
    {
        if (collision.collider.tag == "Arrow" && gameObject.tag == "Monster")
        {
            collision.gameObject.transform.parent = this.transform;

            hpBarFrame.SetActive(true);
            CancelInvoke("HideHpBarFrame");
            Invoke("HideHpBarFrame", 3);

            Arrow arrow = collision.gameObject.GetComponent<Arrow>();
            ReduceHp(arrow.damage);

            if (hp <= 30)
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
        if ((hp / 100) <= 0)
        {
            Destroy(hpBarFrame);
            Dead();
        }
    }

    void ReduceHp(float damage)
    {
        hp -= damage - defensivePower;
        anim.SetFloat("Hp", hp);

        if ((hp / 100) <= 0)
            hpBar.fillAmount = 0;
        else
            hpBar.fillAmount = hp / 100;
    }

    protected virtual void Dead()
    {
        OnDead.Invoke();

        gameObject.tag = "Untagged";
        gameObject.transform.Find("Body").tag = "Untagged";

        speed = 0;

        CancelInvoke();
        Invoke("Destroy", 3);

        Destroy(radar);
    }

    void Destroy()
    {
        Destroy(this.gameObject);
    }

    void HideHpBarFrame()
    {
        hpBarFrame.SetActive(false);
    }
    #endregion
}
