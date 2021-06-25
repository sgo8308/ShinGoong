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
        if ((collision.collider.tag == "Arrow" || collision.collider.tag == "Skill") && gameObject.tag == "MonsterBody")
        {
            if (collision.collider.tag == "Arrow")
            {
                collision.gameObject.transform.parent = this.transform.parent.transform;

                Arrow arrow = collision.gameObject.GetComponent<Arrow>();

                onHit.Invoke(arrow.damage);
            }

            if (collision.collider.tag == "Skill")
            {
                Skill skill = collision.transform.GetComponent<Skill>();

                onHit.Invoke(skill.damage);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Arrow" || collision.tag == "Skill") && gameObject.tag == "MonsterBody")
        {
            if (collision.tag == "Arrow")
            {
                collision.gameObject.transform.parent = this.transform.parent.transform;

                Arrow arrow = collision.gameObject.GetComponent<Arrow>();

                onHit.Invoke(arrow.damage);
            }

            if (collision.tag == "Skill")
            {
                Skill skill = collision.transform.GetComponent<Skill>();

                onHit.Invoke(skill.damage);
            }
        }
    }
}
