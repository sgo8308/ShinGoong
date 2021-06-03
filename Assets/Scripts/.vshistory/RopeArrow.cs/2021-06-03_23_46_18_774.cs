using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeArrow : MonoBehaviour
{
    const int USED_ARROW_LAYER_NUM = 14;

   

    public static List<Vector2> Current_RopeArrowPosition_List = new List<Vector2>();

    //  [SerializeField] int ArrowCol_MaxCount = 4;

    [SerializeField] GameObject _ropePrefab = null; //로프 프리팹을 담을 변수

    void Start()
    {
        Current_RopeArrowPosition_List.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Fire._ropeArrowState) //로프화살이 발사 중이라면
        {
            GameObject Rope = Instantiate(_ropePrefab, transform.position, transform.rotation); //로프 생성
        }

    
    }

    private void OnCollisionEnter2D(Collision2D collision)

    {



        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static; //오브젝트를 움직이지 않게 한다.
        Fire._ropeArrowState = false;  //로프화살이 정지됨
        Player._ropeMove = true;

        Current_RopeArrowPosition_List.Add( transform.position);

        this.gameObject.layer = 0;
        GetComponent<BoxCollider2D>().isTrigger = true;
     

    }


   private void OnTriggerEnter2D(Collider2D collision)
   {
       if (collision.gameObject.name == "Player" && this.gameObject.layer == USED_ARROW_LAYER_NUM)
       {
           print("화살 회수");
           Destroy(this.gameObject);  //박힌 화살 없애기
 
       
 
       }
 
   }

}
