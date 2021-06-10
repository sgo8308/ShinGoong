using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fire : MonoBehaviour
{
    public GameObject arrowPrefab = null; //화살 프리팹을 담을 변수
    public GameObject ropeArrowPrefab = null; //화살 프리팹을 담을 변수

    public static GameObject arrowDirection = null;   //화살의 위치값을 담을 변수
    public GameObject _player = null;
    public float arrowSpeed = 50f;    //화살 속도
    public float arrowMaxPower = 1f;    //화살 Max Power
    public float ropeArrowSpeed = 15f;    //화살 속도

    MainUI _mainUI;

    Camera _mainCamera = null; //카메라 변수

    float _power = 0.0f;
        
    public static float arrowPowerSpeed;

    public static bool ropeArrowState = false;

    Vector2 t_mousePos;

    void Start()
    {
        _mainUI = MainUI.instance;
        _player = GameObject.Find("Player");
        _mainCamera = Camera.main;    //태그가 main인 카메라를 변수에 넣어준다.
        arrowDirection = GameObject.Find("ArrowDirection");
        arrowDirection.SetActive(false);
    }

    void Update()
    {
        if (!Player.canMove)
            return;

        LookAtMouse();
        TryFire();
        arrowDirection.transform.position = _player.transform.position;  //발사 직전 화살의 위치 = 플레이어의 위치
    }

    void LookAtMouse()
    {
        t_mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition); //스크린상의 마우스좌표 -> 게임상의 2d 좌표로 치환
        Vector2 t_direction = new Vector2(t_mousePos.x - arrowDirection.transform.position.x,
                                          t_mousePos.y - arrowDirection.transform.position.y);   //마우스 좌표 - 화살 좌표 = 바라볼 방향

        arrowDirection.transform.right = t_direction;  //화살의 x축 방향을 '바라볼 방향'으로 정한다
    }

    void TryFire()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && !Player.jumpingState && !Input.GetKey(KeyCode.R))  //플레이어가 움직이지 않을때에만 화살이 발사된다.
        {
            if (Input.GetMouseButtonUp(0) && !ropeArrowState)// 로프화살이 발사된 상태가 아닐때 마우스 클릭한 경우
            {
                print("마우스위치 : " + t_mousePos);
                Invoke("Attack", 0.1f);  //활시위를 놓을때 0.1초 후에 화살이 발사된다.
            }                                

            if (Input.GetMouseButton(0) && _mainUI.arrowCount != 0)
            {
                _power += Time.deltaTime;

                _mainUI.UpdateGaugeBarUI(_power, arrowMaxPower);

                if (_power > arrowMaxPower)
                    _power = arrowMaxPower;
            }

            _mainUI.UpdateArrowCountUI();
        }

        Rope();
    }

    void Attack()
    {
        if (_mainUI.arrowCount > 0)
        {
            arrow_startPosition = new Vector2(_tfArrow.transform.position.x, _tfArrow.transform.position.y);  //발사 위치 설정
            
            GameObject t_arrow = Instantiate(arrowPrefab, arrowDirection.transform.position, arrowDirection.transform.rotation); //화살 생성
            t_arrow.GetComponent<Rigidbody2D>().velocity = t_arrow.transform.right * _power * arrowSpeed * 1 / 2;  //화살 발사 속도 = x축 방향 * 파워 * 속도값
            arrowPowerSpeed = _power * arrowSpeed;

            if (_power >= arrowMaxPower)
            {
                t_arrow.GetComponent<Rigidbody2D>().gravityScale = 0; //Max Power일때 직사로 발사된다. 중력 0
                t_arrow.GetComponent<Rigidbody2D>().velocity = t_arrow.transform.right * _power * arrowSpeed * 1 / 3;  //화살 발사 속도 = x축 방향 * 파워 * 속도값
            }

            _power = 0.0f;

            _mainUI.arrowCount -= 1;
        }
        else if (_mainUI.arrowCount == 0)
            _power = 0.0f;
    }

    private void Rope()
    {
        if (Input.GetKey(KeyCode.R))
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject RopeArrow = Instantiate(ropeArrowPrefab, arrowDirection.transform.position, arrowDirection.transform.rotation); //화살 생성
                RopeArrow.GetComponent<Rigidbody2D>().gravityScale = 0; //Max Power일때 직사로 발사된다. 중력 0
                RopeArrow.GetComponent<Rigidbody2D>().velocity = RopeArrow.transform.right * arrowMaxPower * ropeArrowSpeed * 1 / 3;  //화살 발사 속도 = x축 방향 * 파워 * 속도값

                ropeArrowState = true;
            }
        }
    }

    private void RopeFire()
    {
        GameObject RopeArrow = Instantiate(ropeArrowPrefab, arrowDirection.transform.position, arrowDirection.transform.rotation); //화살 생성
        RopeArrow.GetComponent<Rigidbody2D>().gravityScale = 0; //Max Power일때 직사로 발사된다. 중력 0
        RopeArrow.GetComponent<Rigidbody2D>().velocity = RopeArrow.transform.right * _arrow_maxPower * _ropeArrow_speed * 1 / 3;  //화살 발사 속도 = x축 방향 * 파워 * 속도값


    }
}
