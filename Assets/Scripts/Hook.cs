using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{

    public LineRenderer line;
    public Transform hook;

    Vector2 mousedir;

    public bool isHookActive;
    public bool isLineMax;
    public bool isAttach;
   

    // Start is called before the first frame update
    void Start()
    {
        hook.position = transform.position;

        line.positionCount = 2;
        line.endWidth = line.startWidth = 0.05f;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);
        line.useWorldSpace = true;
        isAttach = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!HookCollider.ropePull)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, hook.position);


            if (Input.GetKeyDown(KeyCode.E) && !isHookActive)
            {

                hook.position = transform.position;
                line.SetPosition(0, transform.position);
                line.SetPosition(1, hook.position);

                mousedir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                isHookActive = true;
                isLineMax = false;
                hook.gameObject.SetActive(true);
            }

            if (isHookActive && !isLineMax && !isAttach)
            {
                hook.Translate(mousedir.normalized * Time.deltaTime * 15);

                if (Vector2.Distance(transform.position, hook.position) > 25)
                {
                    isLineMax = true;
                }

            }
            else if (isHookActive && isLineMax && !isAttach)
            {

                hook.position = Vector2.MoveTowards(hook.position, transform.position, Time.deltaTime * 12);

                if (Vector2.Distance(transform.position, hook.position) < 0.01f)
                {
                    isHookActive = false;
                    isLineMax = false;
                    hook.gameObject.SetActive(false);
                    hook.position = transform.position;
                }
            }
            else if (isAttach)
            {

            }
        }

        if (HookCollider.ropePull)  //로프가 충돌했을때
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            transform.position = Vector2.MoveTowards(transform.position, hook.position, Time.deltaTime * 12);
            isHookActive = false;

            if (Vector2.Distance(transform.position, hook.position) < 2.0f)
            {
                isHookActive = false;
                isLineMax = false;
                hook.gameObject.SetActive(false);
                hook.position = transform.position;

                HookCollider.ropePull = false;
                GetComponent<Rigidbody2D>().gravityScale = 3;

            }
        }




    }


    
}
