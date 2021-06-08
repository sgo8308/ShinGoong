using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    bool _arrowState = true;
    public float damage;
    const float ORIGINAL_DAMAGE = 70;
    const float BOMB_SHOT_DAMAGE = 30;
    Vector2 _zeroVelocity;
    List<Vector2> _arrowColList = new List<Vector2>();
    const int USED_ARROW_LAYER_NUM = 14;
    public int arrowColMaxCount = 4;

    Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void Start()
    {
        damage = ORIGINAL_DAMAGE;
        if (BombShot.bombShotState)
            damage += BOMB_SHOT_DAMAGE;

        _zeroVelocity = new Vector2(0, 0);
    }

    void Update()
    {
        if (_arrowState)
        {
            transform.right = GetComponent<Rigidbody2D>().velocity;  //매 프레임마다 화살의 x축 벡터값을 2d의 속도로 정해준다. 화살촉 방향 조절
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MonsterBody")
        {
            Monster monster = collision.transform.parent.GetComponent<Monster>(); // 일단 쥐 몸을 바로 가지고 옴 후에 몬스터 클래스를 상속하면 바꿔줄 것 
            if (monster.OnDead.GetPersistentEventCount() == 0)
                monster.OnDead.AddListener(DetachedFromMonster);
        }

        if (collision.gameObject.tag == "MonsterBody" || collision.gameObject.tag == "Platform") {
            if (GetComponent<Rigidbody2D>().gravityScale != 0) //곡사가 충돌할때 화살이 박힌다.
            {
                if (BombShot.bombShotState)  //폭발샷이라면 폭발 애니메이션 동작한다.
                {
                    _anim.SetBool("isExploding", true);
                    Invoke("Destroy", 2);
                }

                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; //오브젝트를 움직이지 않게 한다.
                GetComponent<Rigidbody2D>().velocity = _zeroVelocity;
                _arrowState = false; //화살촉 방향 변화를 멈추게 한다.

                if (!BombShot.bombShotState)  //폭발샷이 아니라면 레이어를 14로 하여 이후에 화살 회수가 가능하도록 한다.
                {
                    this.gameObject.layer = USED_ARROW_LAYER_NUM;
                }
            }

            if (GetComponent<Rigidbody2D>().gravityScale == 0)  //직사가 충돌할때 화살이 반사된다.
            {
                //  Vector2 inDirection = GetComponent<Rigidbody2D>().velocity;    //충돌 전 화살 벡터
                Vector2 inNormal = collision.contacts[0].normal;               //충돌 시 법선 벡터

                Vector2 newVelocity = Vector2.Reflect(transform.right, inNormal);  //반사각 벡터
                GetComponent<Rigidbody2D>().velocity = newVelocity * Fire.arrowPowerSpeed * 1 / 3;   //반사된 화살 속도 = 반사각 벡터 * 파워 * 스피드

                _arrowColList.Add(collision.contacts[0].point);  //매 충돌시 리스트에 충돌 좌표를 담는다. 

                //화살의 충돌 횟수가 ArrowCol_MaxCount와 같아지면 더이상 반사되지 않고 멈춘다.
                if (_arrowColList.Count == arrowColMaxCount)
                {
                    GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static; //오브젝트를 움직이지 않게 한다.
                    _arrowState = false; //화살촉 방향 변화를 멈추게 한다.

                    this.gameObject.layer = USED_ARROW_LAYER_NUM;
                    GetComponent<BoxCollider2D>().isTrigger = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && this.gameObject.layer == USED_ARROW_LAYER_NUM)
        {
            Destroy(this.gameObject);

            Fire.arrowCount += 1;
        }
    }
    
    void DetachedFromMonster()
    {
        transform.parent = null;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
