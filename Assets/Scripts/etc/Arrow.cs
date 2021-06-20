using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage;
    private int arrowColMaxCount = 4;

    private const float ORIGINAL_DAMAGE = 70;
    private const int LAYER_NUM_ARROW_ON_PLATFORM = 14;
    private const int LAYER_NUM_ARROW_ON_MONSTER = 17;

    private bool arrowState = true;
    private bool isSoundPlayed;
    private bool isSkillPlayed= false;
    private bool isUsed= false;

    private Vector2 zeroVelocity;
    private List<Vector2> arrowColList = new List<Vector2>();
    private PlayerSkill playerSkill;
    private CameraShake cameraShake;
    private GameObject bombShotEffect;
    

    private void Awake()
    {
        playerSkill = GameObject.Find("Player").GetComponent<PlayerSkill>();
        cameraShake = Camera.main.transform.Find("CameraShake").GetComponent<CameraShake>();
        bombShotEffect = transform.Find("BombShotEffect").gameObject;
    }

    void Start()
    {
        damage = ORIGINAL_DAMAGE;

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
        if (isUsed)
            return;

        if (collision.gameObject.tag != "MonsterBody" && collision.gameObject.tag != "Platform")
            return;

        if (!isZeroGravityArrow()) //곡사가 충돌할때 화살이 박힌다.
        {
            if (playerSkill.IsSkillOn()) {
                Invoke("PlaySkillEffect", 0.2f);
                Destroy(this.gameObject, 0.2f);
            }

            Stop();

            if (!playerSkill.IsSkillOn())  //폭발샷이 아니라면 레이어를 14로 하여 이후에 화살 회수가 가능하도록 한다.
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

        if (collision.gameObject.tag == "MonsterBody")
        {
            Monster monster = collision.transform.parent.GetComponent<Monster>();
            RegisterDetachEvent(monster);
            gameObject.layer = LAYER_NUM_ARROW_ON_MONSTER;
            cameraShake.StartShake();

            PlaySound(NonPlayerSounds.ARROW_PIERCE_MONSTER);
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

        if (isUsed)
            return;

        if (collision.tag == "MonsterBody")
        {
            if (playerSkill.IsSkillOn())
            {
                Invoke("PlaySkillEffect", 0.2f);
                Destroy(this.gameObject, 0.2f);
            }

            Stop();
            Monster monster = collision.transform.parent.GetComponent<Monster>();
            RegisterDetachEvent(monster);
            gameObject.layer = LAYER_NUM_ARROW_ON_MONSTER;
            cameraShake.StartShake();

            PlaySound(NonPlayerSounds.ARROW_PIERCE_MONSTER);
        }

        if (collision.tag == "Platform")
            PlaySound(NonPlayerSounds.ARROW_PIERCE_PLATFORM);
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
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; //오브젝트를 움직이지 않게 한다.
        GetComponent<Rigidbody2D>().velocity = zeroVelocity;
        arrowState = false; //화살촉 방향 변화를 멈추게 한다.

        isUsed = true;

        PlayerAttack.power = 0.0f;
    }

    private void PlaySkillEffect()
    {
        if (isSkillPlayed)
            return;

        switch (playerSkill.GetSkillName())
        {
            case "Bomb Shot":
                bombShotEffect.transform.parent = null;
                bombShotEffect.SetActive(true);

                Destroy(bombShotEffect.gameObject, 1f);

                SoundManager.instance.PlayNonPlayerSound(NonPlayerSounds.SKILL_BOMB_SHOT);
                break;

            default:
                break;
        }

        isSkillPlayed = true;
    }

    private void Reflect(Collision2D collision)
    {
       
        Vector2 inNormal = collision.contacts[0].normal;               //충돌 시 법선 벡터
        Vector2 newVelocity = Vector2.Reflect(transform.right, inNormal);  //반사각 벡터
        GetComponent<Rigidbody2D>().velocity = newVelocity * PlayerAttack.power * 1/3;   //반사된 화살 속도 = 반사각 벡터 * 파워 * 스피드
        
       
    }

    private bool isZeroGravityArrow()
    {
        if (GetComponent<Rigidbody2D>().gravityScale == 0)
            return true;

        return false;
    }

    private void RegisterDetachEvent(Monster monster)
    {
        if (monster.OnDead.GetPersistentEventCount() == 0)
            monster.OnDead.AddListener(DetachFromMonster);
    }

    private void DetachFromMonster()
    {
        transform.parent = null;
        gameObject.layer = LAYER_NUM_ARROW_ON_PLATFORM;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
}
