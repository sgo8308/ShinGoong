using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fire : MonoBehaviour
{
    [SerializeField] GameObject m_goPrefab = null; //화살 프리팹을 담을 변수
    [SerializeField] Transform m_tfArrow = null;   //화살의 위치값을 담을 변수
    [SerializeField] Transform player = null;
    [SerializeField] float arrow_speed = 5f;    //화살 속도
    [SerializeField] float arrow_maxPower = 3f;    //화살 Max Power
    


    public Image gaugeBar;

    public static int arrowCount_int;

    Camera m_cam = null; //카메라 변수

    float power = 0.0f;
        
    public GameObject arrowCount; 

    public static float arrowPowerSpeed;

    public string arrowMaxCount;

    
    
    void Start()
    {
        m_cam = Camera.main;    //태그가 main인 카메라를 변수에 넣어준다.
        arrowCount = GameObject.FindGameObjectWithTag("arrowcount");

        arrowCount.GetComponent<TextMeshProUGUI>().text = arrowMaxCount;
        arrowCount_int = Convert.ToInt32(arrowCount.GetComponent<TextMeshProUGUI>().text);

        
    }

    void LookAtMouse()
    {
        Vector2 t_mousePos = m_cam.ScreenToWorldPoint(Input.mousePosition); //스크린상의 마우스좌표 -> 게임상의 2d 좌표로 치환
        Vector2 t_direction = new Vector2(t_mousePos.x - m_tfArrow.position.x,
                                          t_mousePos.y - m_tfArrow.position.y);   //마우스 좌표 - 화살 좌표 = 바라볼 방향
        
        m_tfArrow.right = t_direction;  //화살의 x축 방향을 '바라볼 방향'으로 정한다
    }

    void TryFire()
    {
        
        if (Input.GetAxisRaw("Horizontal") == 0 && !Player.jumpingState )  //플레이어가 움직이지 않을때에만 화살이 발사된다.
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (arrowCount_int > 0)
                {
                    GameObject t_arrow = Instantiate(m_goPrefab, m_tfArrow.position, m_tfArrow.rotation); //화살 생성
                    t_arrow.GetComponent<Rigidbody2D>().velocity = t_arrow.transform.right * power * arrow_speed * 1 / 2;  //화살 발사 속도 = x축 방향 * 파워 * 속도값
                    arrowPowerSpeed = power * arrow_speed;

                    if (power >= arrow_maxPower)
                    {
                        t_arrow.GetComponent<Rigidbody2D>().gravityScale = 0; //Max Power일때 직사로 발사된다. 중력 0
                        t_arrow.GetComponent<Rigidbody2D>().velocity = t_arrow.transform.right * power * arrow_speed * 1 / 3;  //화살 발사 속도 = x축 방향 * 파워 * 속도값
                    }

                    power = 0.0f;

                    arrowCount_int -= 1;

                }
                else if (arrowCount_int == 0)
                {
                    power = 0.0f;
                }
            }

            if (Input.GetMouseButton(0) && arrowCount_int != 0)
            {
                power += Time.deltaTime;
                gaugeBar.fillAmount = power / arrow_maxPower;
                if (power > arrow_maxPower)
                {
                    power = arrow_maxPower;
                }
            }

            arrowCount.GetComponent<TextMeshProUGUI>().text = arrowCount_int.ToString();
        }
    }

   


    // Update is called once per frame
    void Update()
    {
        LookAtMouse();
        TryFire();
        m_tfArrow.transform.position = player.transform.position;  //발사 직전 화살의 위치 = 플레이어의 위치

       
    }
}
