using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnHit : UnityEvent<float>
{
}

public class MonsterBody : MonoBehaviour
{   
    public OnHit onHit;

    private void Awake()
    {
        if(onHit == null)
            onHit = new OnHit();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("콜맂런엔터 들어옴" + collision.collider.tag + gameObject.tag);
        if (collision.collider.tag == "Arrow" && gameObject.tag == "MonsterBody")
        {
            Debug.Log("콜맂런엔터 if 들어옴");

            collision.gameObject.transform.parent = this.transform.parent.transform;

            Arrow arrow = collision.gameObject.GetComponent<Arrow>();

            onHit.Invoke(arrow.damage);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("트리거엔터 들어옴" + collision.tag + gameObject.tag);

        if (collision.tag == "Arrow" && gameObject.tag == "MonsterBody")
        {
            collision.gameObject.transform.parent = this.transform.parent.transform;

            Arrow arrow = collision.gameObject.GetComponent<Arrow>();

            onHit.Invoke(arrow.damage);
        }
    }
}
