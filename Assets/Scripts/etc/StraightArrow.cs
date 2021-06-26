using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// StraightArrow when shoot arrow at max power
/// </summary>
public class StraightArrow : MonoBehaviour
{
    public float damage;
    private int arrowColMaxCount = 4;

    private const float ORIGINAL_DAMAGE = 70;
    private const int LAYER_NUM_ARROW_ON_PLATFORM = 14;
    private const int LAYER_NUM_ARROW_ON_MONSTER = 17;
    private const int LAYER_NUM_ELECTRICITY_FOR_ARROW = 25;

    private bool arrowState = true;
    private bool isSoundPlayed;
    private bool isUsed = false;

    private Vector2 zeroVelocity;
    private List<Vector2> arrowColList = new List<Vector2>();
    private CameraShake cameraShake;
    private Rigidbody2D rigid;
    private GameObject gameObjectAlreadyHit;
    private GameObject colliderBefore4Hit;
    private GameObject collider4Hit;

    protected void Awake()
    {
        cameraShake = Camera.main.transform.Find("CameraShake").GetComponent<CameraShake>();
        colliderBefore4Hit = transform.Find("ColliderBefore4Hit").gameObject;
        collider4Hit = transform.Find("Collider4Hit").gameObject;
    }


    void Start()
    {
        damage = ORIGINAL_DAMAGE;

        zeroVelocity = new Vector2(0, 0);

        rigid = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (arrowColList.Count > 2)
        {
            colliderBefore4Hit.SetActive(false);
            collider4Hit.SetActive(true);
        }

        if (arrowState)
        {
            transform.right = GetComponent<Rigidbody2D>().velocity;  //매 프레임마다 화살의 x축 벡터값을 2d의 속도로 정해준다. 화살촉 방향 조절
        }

        if (isUsed)
            return;

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, transform.right, 1, LayerMask.GetMask("MonsterBody"));

        if (rayHit.collider != null)
        {
            if (gameObjectAlreadyHit)
            {
                if (gameObjectAlreadyHit.gameObject.name == rayHit.collider.transform.parent.gameObject.name)
                    return;
            }

            gameObjectAlreadyHit = rayHit.collider.transform.parent.gameObject;
            gameObjectAlreadyHit.GetComponent<Monster>().OnHit(ORIGINAL_DAMAGE);
            cameraShake.StartShake();
            PlaySound(NonPlayerSounds.ARROW_PIERCE_MONSTER);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isUsed)
            return;

        if (collision.gameObject.layer == LAYER_NUM_ELECTRICITY_FOR_ARROW)
        {
            SoundManager.instance.PlayNonPlayerSound(NonPlayerSounds.ARROW_BURN);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag != "Platform")
            return;

        gameObjectAlreadyHit = collision.gameObject;

        Reflect(collision);

        arrowColList.Add(collision.contacts[0].point);  //매 충돌시 리스트에 충돌 좌표를 담는다. 

        if (arrowColList.Count == arrowColMaxCount)
        {
            Stop();
            gameObject.layer = LAYER_NUM_ARROW_ON_PLATFORM;
            PlaySound(NonPlayerSounds.ARROW_PIERCE_PLATFORM);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" &&
            (gameObject.layer == LAYER_NUM_ARROW_ON_PLATFORM || gameObject.layer == LAYER_NUM_ARROW_ON_MONSTER))
        {
            Destroy(this.gameObject);

            Inventory.instance.AddArrow();
            MainUI.instance.UpdateArrowCountUI();
            SoundManager.instance.PlayNonPlayerSound(NonPlayerSounds.ACQUIRE_ARROW);
        }
    }

    private void PlaySound(NonPlayerSounds sound)
    {
        if (isSoundPlayed)
        {
            if (sound == NonPlayerSounds.ARROW_PIERCE_MONSTER)
            {
                SoundManager.instance.PlayNonPlayerSound(sound);
                isSoundPlayed = true;
            }
            return;
        }

        SoundManager.instance.PlayNonPlayerSound(sound);
        isSoundPlayed = true;
    }

    private void Stop()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; 
        GetComponent<Rigidbody2D>().velocity = zeroVelocity;
        arrowState = false; 
        isUsed = true;
    }

    private void Reflect(Collision2D collision)
    {
        float power = PlayerAttack.nowPowerOfArrow;
        Vector2 inNormal = collision.contacts[0].normal;               //충돌 시 법선 벡터
        Vector2 newVelocity = Vector2.Reflect(transform.right, inNormal);  //반사각 벡터
        GetComponent<Rigidbody2D>().velocity = newVelocity * power;   //반사된 화살 속도 = 반사각 벡터 * 파워 * 스피드
    }
}
