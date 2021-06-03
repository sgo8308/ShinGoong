using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fire : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab = null; //화살 프리팹을 담을 변수
    [SerializeField] GameObject ropeArrowPrefab = null; //화살 프리팹을 담을 변수


    [SerializeField] Transform transformOfArrow = null;   //화살의 위치값을 담을 변수
    [SerializeField] Transform player = null;
    [SerializeField] float arrowSpeed = 5f;    //화살 속도
    [SerializeField] float arrowMaxPower = 3f;    //화살 Max Power
    [SerializeField] float ropeArrowSpeed = 15f;    //화살 속도


    public Image gaugeBar;

    public static int arrowCount;

    Camera _mainCamera = null; //카메라 변수

    float _power = 0.0f;
        
    public GameObject uIarrowCount; 

    public static float arrowPowerSpeed;

    public string arrowMaxCount;

    public static bool ropeArrowState = false;

    void Start()
    {
        _mainCamera = Camera.main;    //태그가 main인 카메라를 변수에 넣어준다.
        uIarrowCount = GameObject.FindGameObjectWithTag("ArrowCount");

        uIarrowCount.GetComponent<TextMeshProUGUI>().text = arrowMaxCount;
        arrowCount = Convert.ToInt32(uIarrowCount.GetComponent<TextMeshProUGUI>().text);
    }

    void LookAtMouse()
    {
        Vector2 t_mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition); //스크린상의 마우스좌표 -> 게임상의 2d 좌표로 치환
        Vector2 t_direction = new Vector2(t_mousePos.x - transformOfArrow.position.x,
                                          t_mousePos.y - transformOfArrow.position.y);   //마우스 좌표 - 화살 좌표 = 바라볼 방향
        
        transformOfArrow.right = t_direction;  //화살의 x축 방향을 '바라볼 방향'으로 정한다
    }

    void TryFire()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && !Player.jumpingState && !Input.GetKey(KeyCode.R))  //플레이어가 움직이지 않을때에만 화살이 발사된다.
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (arrowCount > 0)
                {
                    GameObject t_arrow = Instantiate(arrowPrefab, transformOfArrow.position, transformOfArrow.rotation); //화살 생성
                    t_arrow.GetComponent<Rigidbody2D>().velocity = t_arrow.transform.right * _power * arrowSpeed * 1 / 2;  //화살 발사 속도 = x축 방향 * 파워 * 속도값
                    arrowPowerSpeed = _power * arrowSpeed;

                    if (_power >= arrowMaxPower)
                    {
                        t_arrow.GetComponent<Rigidbody2D>().gravityScale = 0; //Max Power일때 직사로 발사된다. 중력 0
                        t_arrow.GetComponent<Rigidbody2D>().velocity = t_arrow.transform.right * _power * arrowSpeed * 1 / 3;  //화살 발사 속도 = x축 방향 * 파워 * 속도값
                    }

                    _power = 0.0f;

                    arrowCount -= 1;
                }
                else if (arrowCount == 0)
                    _power = 0.0f;
            }

            if (Input.GetMouseButton(0) && arrowCount != 0)
            {
                _power += Time.deltaTime;
                gaugeBar.fillAmount = _power / arrowMaxPower;
                if (_power > arrowMaxPower)
                {
                    _power = arrowMaxPower;
                }
            }

            uIarrowCount.GetComponent<TextMeshProUGUI>().text = arrowCount.ToString();
        }

        Rope();
    }


    private void Rope()
    {
        if (Input.GetKey(KeyCode.R))

        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject RopeArrow = Instantiate(ropeArrowPrefab, transformOfArrow.position, transformOfArrow.rotation); //화살 생성
                RopeArrow.GetComponent<Rigidbody2D>().gravityScale = 0; //Max Power일때 직사로 발사된다. 중력 0
                RopeArrow.GetComponent<Rigidbody2D>().velocity = RopeArrow.transform.right * arrowMaxPower * ropeArrowSpeed * 1 / 3;  //화살 발사 속도 = x축 방향 * 파워 * 속도값

                ropeArrowState = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        LookAtMouse();
        TryFire();
        transformOfArrow.transform.position = player.transform.position;  //발사 직전 화살의 위치 = 플레이어의 위치

       
    }
}
