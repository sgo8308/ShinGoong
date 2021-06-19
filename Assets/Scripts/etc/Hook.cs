using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{

    public LineRenderer line;

    public GameObject hook;
    public GameObject ropeArrow;

    Vector2 mousedir;
    Vector2 mousePosition;
    Vector2 direction;


    public bool isHookActive;
    public bool isLineMax;
    public bool isAttach;

    Transform arrowDirection;

    

    void Start()
    {
       

        hook.SetActive(false);

        hook.transform.position = transform.position;

        line.positionCount = 2;
        line.endWidth = line.startWidth = 0.05f;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.transform.position);
        line.useWorldSpace = true;
        isAttach = false;

        arrowDirection = transform.Find("ArrowDirection");
    }

    float angle;
    Vector2 target, mouse;

    // Update is called once per frame
    void Update()
    {
    //    SetRopeArrowDirection();


        if (!HookCollider.ropePull)
        {
            

            line.SetPosition(0, transform.position);
            line.SetPosition(1, hook.transform.position);


            if (Input.GetKey(KeyCode.E) && !isHookActive && Input.GetMouseButtonDown(0))
            {
                 SetRopeArrowDirection();
               

                hook.SetActive(true);
             //   hook.transform.position = transform.position;

                line.SetPosition(0, transform.position);
                line.SetPosition(1, hook.transform.position);

                mousedir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - hook.transform.position;


                isHookActive = true;
                isLineMax = false;

                hook.gameObject.SetActive(true);


            }

            if (isHookActive && !isLineMax && !isAttach)
            {

                hook.transform.Translate(mousedir.normalized * Time.deltaTime * 15);

                if (Vector2.Distance(transform.position, hook.transform.position) > 25)
                {
                    isLineMax = true;
                }

            }
            else if (isHookActive && isLineMax && !isAttach)
            {

                hook.transform.position = Vector2.MoveTowards(hook.transform.position, transform.position, Time.deltaTime * 12);

                if (Vector2.Distance(transform.position, hook.transform.position) < 0.01f)
                {
                    isHookActive = false;
                    isLineMax = false;
                    hook.transform.position = transform.position;


                    hook.SetActive(false);

                }
            }
            else if (isAttach)
            {

            }
        }



        if (HookCollider.ropePull)  //로프가 충돌했을때
        {
     
            hook.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            print("여기?");
     
            GetComponent<Rigidbody2D>().gravityScale = 0;
            transform.position = Vector2.MoveTowards(transform.position, hook.transform.position, Time.deltaTime * 12);
            isHookActive = false;
     
            if (Vector2.Distance(transform.position, hook.transform.position) < 2.0f)
            {
                isHookActive = false;
                isLineMax = false;
                hook.gameObject.SetActive(false);
                hook.transform.position = transform.position;
     
                HookCollider.ropePull = false;
                GetComponent<Rigidbody2D>().gravityScale = 3;

                hook.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

                hook.SetActive(false);
     
     
            }
        }




    }

    private void SetRopeArrowDirection()
    {
        hook.transform.position = transform.position;


        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //스크린상의 마우스좌표 -> 게임상의 2d 좌표로 치환
        direction = new Vector2(mousePosition.x - hook.transform.position.x,
                                          mousePosition.y - hook.transform.position.y);   //마우스 좌표 - 화살 좌표 = 바라볼 방향

        ropeArrow.transform.right = direction;  //화살의 x축 방향을 '바라볼 방향'으로 정한다

        

    }

}
