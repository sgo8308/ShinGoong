using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyCollisionEvent : UnityEvent<Collision2D>
{
}

public class MonsterBody : MonoBehaviour
{
    public MyCollisionEvent OnMonsterHit;

    private void Awake()
    {
        if(OnMonsterHit == null)
            OnMonsterHit = new MyCollisionEvent();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        OnMonsterHit.Invoke(collision);
    }
}
