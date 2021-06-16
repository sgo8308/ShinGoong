using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Monster : MonoBehaviour
{
    Image hpBar;
    protected GameObject hpBarFrame;
    protected GameObject radar;
    protected Animator anim;
    protected Rigidbody2D rigid;
    protected int nextMove; // -1 , 0 , 1 -> left, stop, right
    public float speed;
    protected float hp;
    protected float defensivePower;
    public float expPoint { get; protected set; }
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
    abstract protected void ThinkAndMove();

    protected void FlipSprite()
    {
        // Flip Monster Sprite and HpHarFrame
        if (nextMove != 0)
        {
            if (nextMove == 1)
            {
                FlipSpriteToRight();
            }
            else
            {
                FlipSpriteToLeft();
            }
        }
    }
    #endregion

    protected void FlipSpriteToRight()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        hpBarFrame.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 180, 0);
    }

    protected void FlipSpriteToLeft()
    {
        this.transform.rotation = Quaternion.Euler(0, 180, 0);
        hpBarFrame.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 0);
    }

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
    void AddHitEvent()
    {
        MonsterBody monsterBody = this.transform.Find("Body").GetComponent<MonsterBody>();
        monsterBody.onHit.AddListener(OnHit);
    }

    #region When Monster get hit by arrow
    protected abstract void OnHit(float damage);

    protected void CheckIfDead()
    {
        if ((hp / 100) <= 0)
        {
            Destroy(hpBarFrame);
            Dead();
        }
    }

    protected void ReduceHp(float damage)
    {
        hp -= damage - defensivePower;
        anim.SetFloat("Hp", hp);

        if ((hp / 100) <= 0)
            hpBar.fillAmount = 0;
        else
            hpBar.fillAmount = hp / 100;
    }

    public virtual void Dead()
    {
        OnDead.Invoke();

        gameObject.tag = "Untagged";
        gameObject.transform.Find("Body").tag = "Untagged";

        speed = 0;

        PlayerInfo.instance.AddExpPoint(this);

        StageManager.instance.AddNumOfMonsterKilled();

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
