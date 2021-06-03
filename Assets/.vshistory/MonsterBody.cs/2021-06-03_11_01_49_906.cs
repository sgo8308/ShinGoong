using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBody : MonoBehaviour
{
    MonsterMite _monsterMite; 
    private void Start()
    {
        _monsterMite = this.GetComponentInParent<MonsterMite>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "arrow")
        {
            collision.gameObject.transform.parent = this.transform;

            Arrow arrow = collision.gameObject.GetComponent<Arrow>();
            GetAngry();

            Hit(arrow.GetDamage());

            CancelInvoke("GetPeaceful");
            Invoke("GetPeaceful", 5);
        }
    }
}
