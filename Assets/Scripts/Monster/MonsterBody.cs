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
        if (collision.collider.tag == "Arrow" && gameObject.tag == "MonsterBody")
        {
            collision.gameObject.transform.parent = this.transform.parent.transform;

            Arrow arrow = collision.gameObject.GetComponent<Arrow>();

            onHit.Invoke(arrow.damage);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Arrow" && gameObject.tag == "MonsterBody")
        {
            collision.gameObject.transform.parent = this.transform.parent.transform;

            Arrow arrow = collision.gameObject.GetComponent<Arrow>();

            onHit.Invoke(arrow.damage);
        }
    }
}
