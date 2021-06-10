using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fire : MonoBehaviour
{
    public GameObject _arrowPrefab = null; //화살 프리팹을 담을 변수
    public GameObject _ropeArrowPrefab = null; //화살 프리팹을 담을 변수
    
    public static GameObject _tfArrow = null;   //화살의 위치값을 담을 변수
    public GameObject _player = null;
    public float _arrow_speed = 5f;    //화살 속도
    public float _arrow_maxPower = 3f;    //화살 Max Power
    public float _ropeArrow_speed = 15f;    //화살 속도


    public Image gaugeBar;

    public static int arrowCount_int;

    Camera m_cam = null; //카메라 변수

    float power = 0.0f;
        
    public GameObject arrowCount; 

    public static float arrowPowerSpeed;

    public string arrowMaxCount;

    Vector2 MousePosition;

    public static bool _ropeArrowState = false;

    Vector2 t_mousePos;

    Vector2 arrow_startPosition;

    void Start()
    {
        m_cam = Camera.main;    //태그가 main인 카메라를 변수에 넣어준다.
        arrowCount = GameObject.FindGameObjectWithTag("arrowcount");
        _tfArrow = GameObject.Find("ArrowDirection");
        _tfArrow.SetActive(false);

        arrowCount.GetComponent<TextMeshProUGUI>().text = arrowMaxCount;
        arrowCount_int = Convert.ToInt32(arrowCount.GetComponent<TextMeshProUGUI>().text);

        
    }

    void LookAtMouse()
    {
        t_mousePos = m_cam.ScreenToWorldPoint(Input.mousePosition); //스크린상의 마우스좌표 -> 게임상의 2d 좌표로 치환
        Vector2 t_direction = new Vector2(t_mousePos.x - _tfArrow.transform.position.x,
                                          t_mousePos.y - _tfArrow.transform.position.y);   //마우스 좌표 - 화살 좌표 = 바라볼 방향
        
        _tfArrow.transform.right = t_direction;  //화살의 x축 방향을 '바라볼 방향'으로 정한다
    }

    void TryFire()
    {
        
        if (Input.GetAxisRaw("Horizontal") == 0 && !Player.jumpingState && !Input.GetKey(KeyCode.R))  //플레이어가 움직이지 않을때에만 화살이 발사된다.
        {
            if (Input.GetMouseButtonUp(0) && !_ropeArrowState) // 로프화살이 발사된 상태가 아닐때 마우스 클릭한 경우
            {
                print("마우스위치 : "+t_mousePos);
                Invoke("Attack", 0.1f);  //활시위를 놓을때 0.1초 후에 화살이 발사된다.
            }

            if (Input.GetMouseButton(0) && arrowCount_int != 0)
            {
                power += Time.deltaTime;
                gaugeBar.fillAmount = power / _arrow_maxPower;
                if (power > _arrow_maxPower)
                {
                    power = _arrow_maxPower;
                }
            }

            arrowCount.GetComponent<TextMeshProUGUI>().text = arrowCount_int.ToString();
        }

        Rope();
    }

    private void Attack()
    {
        
            if (arrowCount_int > 0)
            {
                arrow_startPosition = new Vector2(_tfArrow.transform.position.x , _tfArrow.transform.position.y );  //발사 위치 설정

                GameObject t_arrow = Instantiate(_arrowPrefab, arrow_startPosition, _tfArrow.transform.rotation); //화살 생성
                t_arrow.GetComponent<Rigidbody2D>().velocity = t_arrow.transform.right * power * _arrow_speed * 1 / 2;  //화살 발사 속도 = x축 방향 * 파워 * 속도값
                arrowPowerSpeed = power * _arrow_speed;


                if (power >= _arrow_maxPower)
                {
                    t_arrow.GetComponent<Rigidbody2D>().gravityScale = 0; //Max Power일때 직사로 발사된다. 중력 0
                    t_arrow.GetComponent<Rigidbody2D>().velocity = t_arrow.transform.right * power * _arrow_speed * 1 / 3;  //화살 발사 속도 = x축 방향 * 파워 * 속도값
                }

                power = 0.0f;

                arrowCount_int -= 1;

            }
            else if (arrowCount_int == 0)
            {
                power = 0.0f;
            }
       

        
    }

    private void Rope()
    {
        if (Input.GetKey(KeyCode.R))

        {
            if (Input.GetMouseButtonUp(0))
            {
                _ropeArrowState = true;
                Invoke("RopeFire", 0.8f);
            }
        }
    }


    private void RopeFire()
    {
        GameObject RopeArrow = Instantiate(_ropeArrowPrefab, _tfArrow.transform.position, _tfArrow.transform.rotation); //화살 생성
        RopeArrow.GetComponent<Rigidbody2D>().gravityScale = 0; //Max Power일때 직사로 발사된다. 중력 0
        RopeArrow.GetComponent<Rigidbody2D>().velocity = RopeArrow.transform.right * _arrow_maxPower * _ropeArrow_speed * 1 / 3;  //화살 발사 속도 = x축 방향 * 파워 * 속도값

       
    }


    


    void Update()
    {
        LookAtMouse();
        TryFire();



        //발사 직전 화살의 위치 = 플레이어의 위치 +0.3f
        _tfArrow.transform.position = new Vector2(_player.transform.position.x, _player.transform.position.y +0.3f);






    }
}
