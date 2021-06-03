using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]

public class MonsterBody : MonoBehaviour
{
    public UnityEvent<Collision2D> OnMonsterHit;

    void OnCollisionEnter2D(Collision2D collision)
    {
        OnMonsterHit.Invoke(collision);
    }


}
