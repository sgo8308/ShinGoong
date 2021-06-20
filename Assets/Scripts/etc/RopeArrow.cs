using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeArrow : MonoBehaviour
{
    const int ARROW_ON_PLATFORM_LAYER_NUM = 14;
   
    public static List<Vector2> currentRopeArrowPositionList = new List<Vector2>();

    public GameObject ropePrefab = null; 

    private PlayerMove playerMove;

    void Start()
    {
        currentRopeArrowPositionList.Clear();
        playerMove = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    void Update()
    {
        if (PlayerAttack.isRopeArrowMoving) 
        {
            GameObject Rope = Instantiate(ropePrefab, transform.position, transform.rotation); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

            PlayerAttack.isRopeArrowMoving = false;

            playerMove.SetIsRopeMoving(true);

            currentRopeArrowPositionList.Add(transform.position);

            this.gameObject.layer = ARROW_ON_PLATFORM_LAYER_NUM;
        }
    }

   private void OnTriggerEnter2D(Collider2D collision)
   {
       if (this.gameObject.layer == ARROW_ON_PLATFORM_LAYER_NUM)
       {
            if (collision.gameObject.name == "Player")
            {
                Destroy(this.gameObject);  //박힌 화살 없애기
                playerMove.SetCanMove(true);
            }
        }
    }
}
