using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fire : MonoBehaviour
{
    [SerializeField] GameObject m_goPrefab = null; //화살 프리팹을 담을 변수
    [SerializeField] Transform m_tfArrow = null;   //화살의 위치값을 담을 변수
    [SerializeField] Transform player = null;
    [SerializeField] float arrow_speed = 5f;    //화살 속도
    [SerializeField] float arrow_maxPower = 3f;    //화살 Max Power


    Camera m_cam = null; //카메라 변수

    float power = 0.0f;
    //public GameObject power_gage; //삭제 요망

    public static float arrowPowerSpeed;

    void Start()
    {
        m_cam = Camera.main;    //태그가 main인 카메라를 변수에 넣어준다.

        //power_gage = GameObject.FindGameObjectWithTag("power");  //삭제 요망
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
        Debug.Log("tryfire 들어옴");
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("tryfire 버튼 클릭함");
            GameObject t_arrow = Instantiate(m_goPrefab, m_tfArrow.position, m_tfArrow.rotation); //화살 생성
            t_arrow.GetComponent<Rigidbody2D>().velocity = t_arrow.transform.right * power * arrow_speed;  //화살 발사 속도 = x축 방향 * 파워 * 속도값
            arrowPowerSpeed = power * arrow_speed;

            if (power >= arrow_maxPower)
            {
                t_arrow.GetComponent<Rigidbody2D>().gravityScale = 0; //Max Power일때 직사로 발사된다. 중력 0
            }
            
            power = 0.0f;
            //power_gage.GetComponent<Text>().text = power.ToString(); //삭제 요망
        }

        if (Input.GetMouseButton(0))
        {
            power += Time.deltaTime;

            if (power > arrow_maxPower)
            {
                power = arrow_maxPower;
            }

            //power_gage.GetComponent<Text>().text = power.ToString(); //삭제 요망
        }
    }


    // Update is called once per frame
    void Update()
    {
        LookAtMouse();
        TryFire();
        m_tfArrow.transform.position = player.transform.position;  //화살의 위치 = 플레이어의 위치
    }
}
