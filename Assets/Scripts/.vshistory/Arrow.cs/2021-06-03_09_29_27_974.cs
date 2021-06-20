using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    bool arrowState = true;
    float _damage;
    List<Vector2> ArrowColList = new List<Vector2>();

    [SerializeField] int ArrowCol_MaxCount = 4;

    void Start()
    {
        _damage = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (arrowState)
        {
            transform.right = GetComponent<Rigidbody2D>().velocity;  //매 프레임마다 화살의 x축 벡터값을 2d의 속도로 정해준다. 화살촉 방향 조절
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GetComponent<Rigidbody2D>().gravityScale != 0) //곡사가 충돌할때 화살이 박힌다.
        {
            Debug.Log("곡사 충돌 시작!");

            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static; //오브젝트를 움직이지 않게 한다.

            arrowState = false; //화살촉 방향 변화를 멈추게 한다.

            this.gameObject.layer = 0;
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        if (GetComponent<Rigidbody2D>().gravityScale == 0)  //직사가 충돌할때 화살이 반사된다.
        {
            Debug.Log("직사 충돌 시작!");

            //  Vector2 inDirection = GetComponent<Rigidbody2D>().velocity;    //충돌 전 화살 벡터
            Vector2 inNormal = collision.contacts[0].normal;               //충돌 시 법선 벡터

            // print(collision.contacts[0].point);

            Vector2 newVelocity = Vector2.Reflect(transform.right, inNormal);  //반사각 벡터
            GetComponent<Rigidbody2D>().velocity = newVelocity * Fire.arrowPowerSpeed * 1/3;   //반사된 화살 속도 = 반사각 벡터 * 파워 * 스피드

            ArrowColList.Add(collision.contacts[0].point);  //매 충돌시 리스트에 충돌 좌표를 담는다. 

            //화살의 충돌 횟수가 ArrowCol_MaxCount와 같아지면 더이상 반사되지 않고 멈춘다.
            if (ArrowColList.Count == ArrowCol_MaxCount)
            {

                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static; //오브젝트를 움직이지 않게 한다.
                arrowState = false; //화살촉 방향 변화를 멈추게 한다.

                this.gameObject.layer = 0;
                GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "SunBee" && this.gameObject.layer == 0)
        {
            print("화살 회수");
            Destroy(this.gameObject);  //박힌 화살 없애기

            Fire.arrowCount_int += 1;
        }
    }

    public float GetDamage()
    {
        return _damage;
    }

}
