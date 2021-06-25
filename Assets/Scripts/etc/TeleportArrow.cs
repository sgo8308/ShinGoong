using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportArrow : MonoBehaviour
{
    private int arrowColMaxCount = 4;

    private const int LAYER_NUM_ARROW_ON_PLATFORM = 14;

    private bool arrowState = true;
    private bool isUsed = false;
    public bool isStop = false;

    private Vector2 zeroVelocity;
    private List<Vector2> arrowColList = new List<Vector2>();
    protected GameObject bombShotEffect;

    private GameObject teleportPosition;


    void Start()
    {
        zeroVelocity = new Vector2(0, 0);

        teleportPosition = transform.Find("TeleportPosition").gameObject;

        Invoke("DestroyThis", 5);
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
        if (isUsed)
            return;

        if (collision.gameObject.tag != "Platform")
            return;

        if (!isZeroGravityArrow()) //곡사가 충돌할때 화살이 박힌다.
        {
            Stop();

            gameObject.layer = LAYER_NUM_ARROW_ON_PLATFORM;
        }

        if (isZeroGravityArrow())  //직사가 충돌할때 화살이 반사된다.
        {
            Reflect(collision);

            arrowColList.Add(collision.contacts[0].point);  //매 충돌시 리스트에 충돌 좌표를 담는다. 

            //화살의 충돌 횟수가 ArrowCol_MaxCount와 같아지면 더이상 반사되지 않고 멈춘다.
            if (arrowColList.Count == arrowColMaxCount)
            {
                Stop();

                gameObject.layer = LAYER_NUM_ARROW_ON_PLATFORM;
            }
        }
    }

    private void Stop()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; //오브젝트를 움직이지 않게 한다.
        GetComponent<Rigidbody2D>().velocity = zeroVelocity;
        arrowState = false; //화살촉 방향 변화를 멈추게 한다.

        isUsed = true;
        isStop = true;

        teleportPosition.transform.parent = null;
        Destroy(this.gameObject);
    }

    private void Reflect(Collision2D collision)
    {
        float power = PlayerAttack.nowPowerOfArrow;
        Vector2 inNormal = collision.contacts[0].normal;               //충돌 시 법선 벡터
        Vector2 newVelocity = Vector2.Reflect(transform.right, inNormal);  //반사각 벡터
        GetComponent<Rigidbody2D>().velocity = newVelocity * power * 1/3;   //반사된 화살 속도 = 반사각 벡터 * 파워 * 스피드
    }

    private bool isZeroGravityArrow()
    {
        if (GetComponent<Rigidbody2D>().gravityScale == 0)
            return true;

        return false;
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
