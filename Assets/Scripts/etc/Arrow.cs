using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage;

    private const float ORIGINAL_DAMAGE = 70;
    private const float BOMB_SHOT_DAMAGE = 30;
    private const int USED_ARROW_LAYER_NUM = 14;
    private bool arrowState = true;
    private Vector2 zeroVelocity;
    private List<Vector2> arrowColList = new List<Vector2>();
    private int arrowColMaxCount = 4;
    
    Animator anim;

    PlayerSkill playerSkill;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerSkill = GameObject.Find("Player").GetComponent<PlayerSkill>();
    }

    void Start()
    {
        damage = ORIGINAL_DAMAGE;

        if (playerSkill.IsSkillOn())
            damage += BOMB_SHOT_DAMAGE;

        zeroVelocity = new Vector2(0, 0);
    }

    void Update()
    {
        if (arrowState)
        {
            transform.right = GetComponent<Rigidbody2D>().velocity;  //매 프레임마다 화살의 x축 벡터값을 2d의 속도로 정해준다. 화살촉 방향 조절
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MonsterBody" || collision.gameObject.tag == "Platform") 
        {
            if (collision.gameObject.tag == "MonsterBody")
            {
                RegisterDetachEvent(collision);
            }

            if (!isZeroGravityArrow()) //곡사가 충돌할때 화살이 박힌다.
            {
                if (playerSkill.IsSkillOn()) {
                    ShowSkillEffect();
                    Invoke("Destroy", 2);
                }

                Stop();

                if (!playerSkill.IsSkillOn())  //폭발샷이 아니라면 레이어를 14로 하여 이후에 화살 회수가 가능하도록 한다.
                    gameObject.layer = USED_ARROW_LAYER_NUM;
            }

            if (isZeroGravityArrow())  //직사가 충돌할때 화살이 반사된다.
            {
                Reflect(collision);

                arrowColList.Add(collision.contacts[0].point);  //매 충돌시 리스트에 충돌 좌표를 담는다. 

                //화살의 충돌 횟수가 ArrowCol_MaxCount와 같아지면 더이상 반사되지 않고 멈춘다.
                if (arrowColList.Count == arrowColMaxCount)
                {
                    Stop();

                    gameObject.layer = USED_ARROW_LAYER_NUM;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && this.gameObject.layer == USED_ARROW_LAYER_NUM)
        {
            Destroy(this.gameObject);

            Inventory.instance.AddArrow();
            MainUI.instance.UpdateArrowCountUI();
        }
    }

    private void Stop()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; //오브젝트를 움직이지 않게 한다.
        GetComponent<Rigidbody2D>().velocity = zeroVelocity;
        arrowState = false; //화살촉 방향 변화를 멈추게 한다.
    }

    private void ShowSkillEffect()
    {
        switch (playerSkill.GetSkillName())
        {
            case "Bomb Shot":
                anim.SetBool("isExploding", true);
                break;

            default:
                break;
        }
    }

    private void Reflect(Collision2D collision)
    {
        Vector2 inNormal = collision.contacts[0].normal;               //충돌 시 법선 벡터
        Vector2 newVelocity = Vector2.Reflect(transform.right, inNormal);  //반사각 벡터
        GetComponent<Rigidbody2D>().velocity = newVelocity * PlayerAttack.power * 1 / 3;   //반사된 화살 속도 = 반사각 벡터 * 파워 * 스피드
    }

    private bool isZeroGravityArrow()
    {
        if (GetComponent<Rigidbody2D>().gravityScale == 0)
            return true;

        return false;
    }

    private void RegisterDetachEvent(Collision2D collision)
    {
        Monster monster = collision.transform.parent.GetComponent<Monster>();
        if (monster.OnDead.GetPersistentEventCount() == 0)
            monster.OnDead.AddListener(DetachFromMonster);
    }

    private void DetachFromMonster()
    {
        transform.parent = null;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
