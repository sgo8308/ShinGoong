﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage;

    private const float ORIGINAL_DAMAGE = 70;
    private const int LAYER_NUM_ARROW_ON_PLATFORM = 14;
    private const int LAYER_NUM_ARROW_ON_MONSTER = 17;

    private bool arrowState = true;
    private bool isSoundPlayed;
    private bool isSkillPlayed= false;
    private bool isUsed= false;

    private Vector2 zeroVelocity;
    private PlayerSkill playerSkill;
    private CameraShake cameraShake;
    protected GameObject bombShotEffect;

    protected void Awake()
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

        if (playerSkill.IsSkillOn())
        {
            Invoke("PlaySkillEffect", 0.2f);
            Destroy(this.gameObject, 0.2f);
        }

        Stop();

        if (!playerSkill.IsSkillOn())
            gameObject.layer = LAYER_NUM_ARROW_ON_PLATFORM;

        if (collision.gameObject.tag == "MonsterBody")
        {
            Monster monster = collision.transform.parent.GetComponent<Monster>();
            RegisterDetachEvent(monster);
            gameObject.layer = LAYER_NUM_ARROW_ON_MONSTER;
            cameraShake.StartShake();

            PlaySound(NonPlayerSounds.ARROW_PIERCE_MONSTER);
        }

        isUsed = true;
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
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; 
        GetComponent<Rigidbody2D>().velocity = zeroVelocity;
        arrowState = false; 
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
