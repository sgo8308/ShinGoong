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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "arrow")
        {
            collision.gameObject.transform.parent = this.transform;

            Arrow arrow = collision.gameObject.GetComponent<Arrow>();
            _monsterMite.GetAngry();

            _monsterMite.Hit(arrow.GetDamage());

            _monsterMite.CancelInvoke("GetPeaceful");
            _monsterMite.Invoke("GetPeaceful", 5);
        }
    }


}
