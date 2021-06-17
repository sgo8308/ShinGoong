using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeArrowCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

         //   PlayerAttack.isRopeArrowMoving = false;
         //
         //   playerMove.SetIsRopeMoving(true);
         //
         //   currentRopeArrowPositionList.Add(transform.position);
         //
         //   this.gameObject.layer = ARROW_ON_PLATFORM_LAYER_NUM;
        }
    }
}
