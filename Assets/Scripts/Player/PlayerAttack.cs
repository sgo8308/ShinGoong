using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private Camera mainCamera;
    private Transform arrowDirection = null;
    private float aimAngle;
    private Vector2 mousePosition;

    private void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        arrowDirection = transform.Find("ArrowDirection");
    }

    private void Update()
    {
        AttackReady();
    }

    private void AttackReady()
    {
        if (Input.GetMouseButtonDown(0))  //down -> ready애니메이션 시작
        {
            CalculateBowAngle();
            animator.SetBool("isReady", true);
            Invoke("ReadyCancel", 0.8f);
        }

        if (Input.GetMouseButtonUp(0))  //up -> 0.2초 뒤에 angle애니메이션 취소
        {
            animator.SetBool("isAiming20", false);
            animator.SetBool("isFireFinish20", true);

            Invoke("AimingCancel", 0.3f);
        }
    }

    private void CalculateBowAngle()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); //스크린상의 마우스좌표 -> 게임상의 2d 좌표로 치환
        Vector2 t_direction = new Vector2(mousePosition.x - arrowDirection.position.x,
                                          mousePosition.y - arrowDirection.position.y);   //마우스 좌표 - 화살 좌표 = 바라볼 방향

        aimAngle = Mathf.Atan2(t_direction.y, t_direction.x) * Mathf.Rad2Deg;   //조준하고 있는 각도 세타 구하기
        aimAngle = Mathf.Abs(90 - aimAngle);
        print(aimAngle);
    }

    private void ReadyCancel()  //Ready애니메이션 끝나자 마자 Aiming애니메이션 시작
    {
        animator.SetBool("isReady", false);

        if (aimAngle >= 0 && aimAngle < 20) //마우스 각도가 0~20도 일때 Aiming20 애니메이션 시작
        {
            animator.SetBool("isAiming20", true);
        }
        if (aimAngle >= 20 && aimAngle < 30)
        {
            animator.SetBool("isAiming30", true);
        }
    }
}
